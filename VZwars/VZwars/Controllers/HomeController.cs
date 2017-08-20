using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VZwars.Models;
using MinionWarsEntitiesLib;
using MinionWarsEntitiesLib.Geolocations;
using MinionWarsEntitiesLib.Models;
using MinionWarsEntitiesLib.EntityManagers;

namespace VZwars.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            /*if (Session["UserName"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }*/
            UserDataModel userModel = new UserDataModel(1);
            Session["UserName"] = "TheDirector";
            Session["UserId"] = 1;
            //System.Diagnostics.Debug.WriteLine("USERNAME: " + userModel.userModel.user.username);
            return View(userModel);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            Users result = AccountManager.LoginUser(username, password);
            if (result != null)
            {
                Session["UserName"] = result.username;
                Session["UserId"] = result.id;

                return RedirectToAction("Index", "Home");
            }
            else return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["UserName"] = null;
            return RedirectToAction("Login");
        }

        public ActionResult SendMinions(int count)
        {
            string path = Server.MapPath("~/Content/");
            /*Battlegroup group = new Battlegroup();

            for(int i=0; i < count; count++)
            {
                Minion minion = MinionGenotype.generateRandomMinion();
                group.frontline.Add(minion);
            }*/

            //return Json(group);
            return Json(path);
        }

        public ActionResult RefreshMap(double lat, double lon)
        {
            var point = string.Format("POINT({1} {0})", lat, lon);
            MapDataModel mdm = MapManager.GetMapData(1, point, 1000);
            //System.Diagnostics.Debug.WriteLine("TEST!: " + mdm.objectList.Count);
            //System.Diagnostics.Debug.WriteLine(Json(mdm.bgList));
            //System.Diagnostics.Debug.WriteLine(Json(mdm.objectList));
            return Json(mdm.objectList);
        }
    }
}