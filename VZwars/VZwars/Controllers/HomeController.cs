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
using MinionWarsEntitiesLib.Combat;
using MinionWarsEntitiesLib.Battlegroups;
using MinionWarsEntitiesLib.Structures;
using System.Threading.Tasks;

namespace VZwars.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                UserDataModel userModel = new UserDataModel(Convert.ToInt32(Session["UserId"]));
                return View(userModel);
            }
            else
            {
                return RedirectToAction("Login");
            }
            /*UserDataModel userModel = new UserDataModel(2);
            Session["UserName"] = userModel.userModel.username;
            Session["UserId"] = userModel.userModel.id;*/
            //System.Diagnostics.Debug.WriteLine("USERNAME: " + userModel.userModel.user.username);
            //return View(userModel);
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

        public ActionResult SendMinions(int bg_id, double lat, double lon, double clat, double clon)
        {
            //string path = Server.MapPath("~/Content/");
            var point = string.Format("POINT({1} {0})", lat, lon);
            var cpoint = string.Format("POINT({1} {0})", clat, clon);
            bool result = BattlegroupManager.SendRemoteGroup(bg_id, point, cpoint);

            return Json(result);
        }

        public ActionResult BuildCamp(double lat, double lon)
        {
            var point = string.Format("POINT({1} {0})", lat, lon);
            Camp c = CampManager.CreateUserCamp(point, Convert.ToInt32(Session["UserId"]));
            if (c == null) return Json("Camp couldn't be built!");
            else return Json("Camp built!");
        }

        public ActionResult UpdateUserPosition(double lat, double lon)
        {
            UsersManager.UpdateUserPosition(Convert.ToInt32(Session["UserId"]), lon, lat);
            return Json(true);
        }

        public ActionResult RefreshMap(double lat, double lon)
        {
            var point = string.Format("POINT({1} {0})", lat, lon);
            MapDataModel mdm = MapManager.GetMapData(Convert.ToInt32(Session["UserId"]), point, 1000);
            return Json(mdm.objectList);
        }

        public ActionResult AddMinionsToGroup(int? o_id, int? amount, int? line, int? bg_id, string name)
        {
            string result = OwnershipManager.ProcessAddition(o_id, amount, line, bg_id, name);
            return Json(result);
        }

        public ActionResult RemoveMinions(int a_id)
        {
            bool result = BattlegroupManager.RemoveMinions(a_id);
            return Json(result);
        }

        public ActionResult IncreaseTrait(int trait)
        {
            bool result = ExperienceManager.IncreaseTrait(Convert.ToInt32(Session["UserId"]), trait);
            return Json(result);
        }

        public ActionResult InitiateCombat(int pbg_id, int target_id)
        {
            CombatLog log = CombatManager.StartCombat(pbg_id, target_id);
            string message = "";

            if (log == null) message = "You cannot attack your own minions!";
            else if (log.winner.id == Convert.ToInt32(Session["UserId"])) message = "You won! +50 exp";
            else message = "You lost!";

            return Json(message);
        }
    }
}