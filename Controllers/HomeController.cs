using RockPaperScissors.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RockPaperScissors.Controllers
{
    public class HomeController : Controller
    {
        private RockPaperScissorsContext db = new RockPaperScissorsContext();

        // GET
        public ActionResult Index()
        {
            ViewBag.Title = "Rock Paper Scissors";

            List<SelectListItem> listOfPlayers = new List<SelectListItem>();
            Session["ListOfPlayers"] = listOfPlayers;
            return View();
        }
        // GET
        public ActionResult About()
        {
            ViewBag.Title = "About";
            
            return View();
        }


        [HttpGet]
        public JsonResult GetTop()
        {
            List<SelectListItem> listOfPlayers = new List<SelectListItem>();
            try
            {
                var tempListOfPlayers = db.Player.OrderByDescending(pl => pl.Points).ToList();
                string tempText = "";

                for (int i = 0; i < tempListOfPlayers.Count; i++)
                {
                    tempText = tempListOfPlayers.ElementAt(i).Name + " (" +
                        tempListOfPlayers.ElementAt(i).Points + ")";
                    listOfPlayers.Add(new SelectListItem
                    {
                        Text = tempText,
                        Value = tempListOfPlayers.ElementAt(i).Id.ToString()
                    });

                }
                Session["ListOfPlayers"] = listOfPlayers;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return Json("Something went wrong.");
            }


            return Json(listOfPlayers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Uploads a file in the Server
        /// </summary>
        /// <param name="id">file id</param>
        /// <returns>JsonResult with process status</returns>
        [HttpPost]
        public JsonResult UploadFile(string id)
        {
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // Gets the information
                        var stream = fileContent.InputStream;

                        // Write to disk
                        var fileName = "championship.txt";
                        var path = Path.Combine(Server.MapPath("~/App_Data/UploadedFiles"), fileName);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }

            return Json("File uploaded successfully");
        }

        /// <summary>
        /// Removes all the records from the Database
        /// </summary>
        /// <returns>JsonResult with process status</returns>
        [HttpPost]
        public JsonResult ResetDatabase()
        {
            if (db.Player.ToList().Count == 0)
            {
                return Json("Database its empty.");
            }
            try
            {
                foreach (var entity in db.Player)
                    db.Player.Remove(entity);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Someting went wrong.");
            }
            return Json("Database is empty.");
        }

        /// <summary>
        /// Downloads a file from Server to Client
        /// </summary>
        /// <param name="fileName">File to be downloaded</param>
        /// <returns>JsonResult with process status</returns>
        [HttpPost]
        public JsonResult DownloadFile(string fileName)
        {
            try
            {
                fileName += ".txt";
                var path = Path.Combine(Server.MapPath("~/App_Data/Championships/"), fileName);
                WebClient myWebClient = new WebClient();
                myWebClient.UseDefaultCredentials = true;
                myWebClient.DownloadFile(path, fileName);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Someting went wrong.");
            }
            return Json("Downloading");

        }
    }
}
