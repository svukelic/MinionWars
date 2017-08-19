﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VZwars.Models;
using MinionWarsEntitiesLib;
using MinionWarsEntitiesLib.Geolocations;

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
            //System.Diagnostics.Debug.WriteLine("USERNAME: " + userModel.userModel.user.username);
            return View(userModel);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username)
        {
            Session["UserName"] = username;
            return RedirectToAction("Index");
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
            MapDataModel mdm = MapManager.GetMapData(point, 1000);
            //System.Diagnostics.Debug.WriteLine("TEST!: " + mdm.bgList.Count);
            //System.Diagnostics.Debug.WriteLine(Json(mdm.bgList));
            System.Diagnostics.Debug.WriteLine(Json(mdm.objectList));
            return Json(mdm.objectList);
        }
    }
}