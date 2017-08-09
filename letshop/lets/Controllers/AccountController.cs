using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using lets.Models;
using System.IO;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Dynamic;

namespace lets.Controllers
{
    public class HomeController : Controller
    {
        letsshopEntities2 db = new letsshopEntities2();


      
        ///MYORDERSLIST
       
        public ActionResult myorders()
        {
            string name=(string)Session["username"];
            if (Session["username"] != null)
            {
                ViewBag.orderlist = db.orders.Where(x => x.customer_name == name).ToList();
               // List<order> od = db.orders.Where(x => x.customer_name == name).ToList();

                return View();
            
            }

            return RedirectToAction("Index");
        
        }

    [HttpGet]
        public ActionResult account()
        {
            string name = (string)Session["username"];
            if (Session["username"] != null)
            {
                ViewBag.account = db.users.Single(x => x.Username == name);

                return View();

            }

            return RedirectToAction("Index");

        }


     
       




    }






}
