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


        
        /// CATEGORY


        [HttpGet]
        
        public ActionResult category(string id)
        {


            dynamic mymodel = new ExpandoObject();

            mymodel.PostBasic = db.products.ToList().Where(x => x.subcat_name == id);

            return View(mymodel);

        }
        ////search

       [HttpPost]
        [ActionName("category")]
        public ActionResult category_search(FormCollection form)
        {
            string a = form["search"];
            
           
            dynamic mymodel = new ExpandoObject();
            var persons = from p in db.products select p;
            if (!string.IsNullOrWhiteSpace(a))
            {

               
                mymodel.PostBasic = persons.Where(r=>r.subcat_name.Contains(a));
                return View(mymodel);
            }
            return RedirectToAction("Index");

        }

        
        //PRODUCT VIEW

        [HttpGet]
        public ActionResult product_details(int id)
        {

            dynamic mymodel = new ExpandoObject();
            //  mymodel.PostBasic = db.products.ToList();
            mymodel.PostBasic = db.products.Single(x => x.product_id == id);

            return View(mymodel);

        }

        [HttpPost]
        [ActionName("product_details")]
        public ActionResult product_details_post()
        {

            return View();

        }


       

     
       




    }






}
