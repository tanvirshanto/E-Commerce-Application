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



        ///Register

        [HttpGet]
        public ActionResult register()
        {
            return View();
        }

        [HttpPost]
        [ActionName("register")]
        public ActionResult register_post(FormCollection formcollection)
        {
            user us = new user();
            us.Name = formcollection["name"];
            us.Email = formcollection["email"];
            us.Contact_Number = formcollection["contact"];
            us.Gender = formcollection["gender"];
            us.Address = formcollection["address"];
            us.Date_of_birth = formcollection["birthdate"];
            us.Username = formcollection["uname"];
            us.Password = formcollection["password"];
            us.user_type = formcollection["user_type"];

            TryUpdateModel(us);

            if (ModelState.IsValid)
            {
                db.spUsers(us.Name, us.Email, us.Contact_Number, us.Gender, us.Address, us.Date_of_birth, us.Username, us.Password, us.user_type);
                return RedirectToAction("register");

            }
            return RedirectToAction("register");
        }


       
        

        ///CONTACT

        [HttpGet]
        public ActionResult contact()
        {


            return View();
        }



        [HttpPost]
        [ActionName("contact")]
        public ActionResult contact_post()
        {


            return View();
        }

        

     
       




    }






}
