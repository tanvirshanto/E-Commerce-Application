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


        //LOGIN

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index_post(FormCollection formcollection)
        {
            user usr = new user();
            usr.Username = formcollection["uname"];
            usr.Password = formcollection["pass"];



            var s = db.splogin(usr.Username, usr.Password);
            
   
            if (s.FirstOrDefault() != null)
            {
               
                Session["username"] = usr.Username;
                if (Session["username"].ToString() == "admin")
                {
                    // Session["username"] = usr.Username;
                    return RedirectToAction("dashboard","Admin");
                }
                else if (Session["cart"] != null)
                {
                    return RedirectToAction("showallcart");
                }
                else
                    return RedirectToAction("account");
            }

            return RedirectToAction("Index");

        }


 

        //LOGOUT

        public ActionResult logout()
        {
            Session.Remove("username");
            Session.Remove("cart");
            Session.Remove("count");
            return RedirectToAction("Index");
        }


     
       




    }






}
