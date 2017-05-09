using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VZwars.Models;

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
            return View();
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
            Battlegroup group = new Battlegroup();

            for(int i=0; i < count; count++)
            {
                Minion minion = MinionGenotype.generateRandomMinion();
                group.frontline.Add(minion);
            }

            return Json(group);
        }
    }
}