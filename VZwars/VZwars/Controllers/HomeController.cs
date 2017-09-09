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
using MinionWarsEntitiesLib.Resources;

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

        public ActionResult BuildCamp(double lat, double lon, string name)
        {
            var point = string.Format("POINT({1} {0})", lat, lon);
            Camp c = CampManager.CreateUserCamp(point, Convert.ToInt32(Session["UserId"]), name);
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

        public ActionResult ConsumeResource(int target_id)
        {
            ResourceManager.ConsumeResourceNode(Convert.ToInt32(Session["UserId"]), target_id);
            string message = "You gained 30 resources!";

            return Json(message);
        }

        public ActionResult ConsumeHive(int target_id)
        {
            HiveManager.ConsumeHiveNode(Convert.ToInt32(Session["UserId"]), target_id);
            string message = "You gained 10 minions!";

            return Json(message);
        }

        public ActionResult ConsumeCaravan(int target_id)
        {
            string res = CampManager.ConsumeCaravan(Convert.ToInt32(Session["UserId"]), target_id);
            string message = "You gained 30 " + res + "!";

            return Json(message);
        }

        public ActionResult AddNewBuilding(int camp_id, int building_id)
        {
            bool result = CampManager.CreateBuilding(camp_id, building_id);
            string message = "";
            if (result) message = "Building created!";
            else message = "Insufficient resources!";

            return Json(message);
        }

        public ActionResult DestroyBuilding(int camp_id, int building_id, int type)
        {
            CampManager.DestroyBuilding(camp_id, building_id, type);

            return Json("Building destroyed");
        }

        public ActionResult GetBuildingCosts(int building_id)
        {
            List<CostObject> col = CostManager.GetBuildingCosts(building_id);

            return Json(col);
        }

        public ActionResult GetCampCaravan(int camp_id, double lat, double lon)
        {
            Caravan car = CampManager.GetCampCaravan(camp_id);
            CaravanDisplayModel cdm;
            if (car == null)
            {
                var point = string.Format("POINT({1} {0})", lat, lon);
                List<Camp> cl = CampManager.ReturnCamps(point, 1000);
                cdm = new CaravanDisplayModel(0, camp_id, null, cl, Convert.ToInt32(Session["UserId"]));
            }
            else
            {
                cdm = new CaravanDisplayModel(1, camp_id, car, null, Convert.ToInt32(Session["UserId"]));
            }

            return Json(cdm);
        }

        public ActionResult SendUserCaravan(int source, int destination)
        {
            string message = "Initial";
            Caravan car = CampManager.GenerateUserCaravan(source, destination);
            if (car == null) message = "Error sending Caravan!";
            else message = "Caravan sent!";

            return Json(message);
        }

        public ActionResult AttachMinions(int m_id, int ob_id)
        {
            string message = "Initial";
            bool result = CampManager.AttachMinions(ob_id, m_id);
            if (result) message = "Minions attached!";
            else message = "Error attaching minions!";

            return Json(message);
        }

        public ActionResult BuildMinions(int ob_id, int amount)
        {
            string message = "Initial";
            bool result = CampManager.BuildMinions(ob_id, Convert.ToInt32(Session["UserId"]), amount);
            if (result) message = "Minions succesfully built!";
            else message = "Error building minions!";

            return Json(message);
        }

        public ActionResult CheckTradingPost(int camp_id)
        {
            List<TradeModel> tml = new List<TradeModel>();
            List<Trading> tradeList = TradeManager.CheckTradingPost(camp_id);
            if(tradeList == null)
            {
                return Json(-1);
            }
            else
            {
                if (tradeList.Count > 0)
                {
                    foreach(Trading t in tradeList)
                    {
                        tml.Add(new TradeModel(t));
                    }

                    return Json(tml);
                }
                else
                {
                    return Json(0);
                }
            }
        }

        public ActionResult AddToTradingPost(int mo_id, int camp_id, int amount)
        {
            bool result = TradeManager.AddToTradingPost(mo_id, amount, camp_id);
            string message = "Initial";
            if (result) message = "Minions succesfully added to Trading Post!";
            else message = "Error working with Trading Post!";

            return Json(message);
        }

        public ActionResult BuyMinions(int trade_id)
        {
            bool result = TradeManager.BuyMinions(trade_id, Convert.ToInt32(Session["UserId"]));
            string message = "Initial";
            if (result) message = "Minions succesfully bought!";
            else message = "Error working with Trading Post!";

            return Json(message);
        }

        public ActionResult BuySubscription(int sub)
        {
            string message = "Subscription succesful!";
            UsersManager.SetSubscription(Convert.ToInt32(Session["UserId"]), sub);

            return Json(message);
        }
    }
}