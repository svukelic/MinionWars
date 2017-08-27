﻿using System;
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

        public ActionResult UpdateUserPosition(double lat, double lon)
        {
            UsersManager.UpdateUserPosition(Convert.ToInt32(Session["UserId"]), lon, lat);
            return Json(true);
        }

        public ActionResult RefreshMap(double lat, double lon)
        {
            var point = string.Format("POINT({1} {0})", lat, lon);
            MapDataModel mdm = MapManager.GetMapData(1, point, 1000);
            return Json(mdm.objectList);
        }

        public ActionResult AddMinionsToGroup(int? o_id, int? amount, int? line, int? bg_id, string name)
        {
            string result = OwnershipManager.ProcessAddition(o_id, amount, line, bg_id, name);
            /*if (result.Equals("ok"))
            {
                UserDataModel userModel = new UserDataModel(Convert.ToInt32(Session["UserId"]));
                return View("Index", userModel);
            }
            else*/
            return Json(result);
        }

        public ActionResult InitiateCombat(int pbg_id, int target_id)
        {
            CombatLog log = CombatManager.StartCombat(pbg_id, target_id);

            return Json(log.winner.id);
        }
    }
}