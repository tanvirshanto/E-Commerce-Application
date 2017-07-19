using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using lets.Models;
using System.IO;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using System.Data.SqlClient;



namespace lets.Controllers
{
    public class AdminController : Controller
    {
        letsshopEntities2 db = new letsshopEntities2();

        public ActionResult registereduser()
        {

            ViewBag.registereduser = db.users.ToList();
            return View();
        }

        public ActionResult orderlist()
        {

            ViewBag.orderlist = db.orders.ToList();
            return View();
        }

        public ActionResult Productlist()
        {

            ViewBag.Productlist = db.products.ToList();

            return View();
        }

      

        public ActionResult dashboard()
        {
            var q = from x in db.users select x;

            var total = q.Count();
            ViewBag.count = total;

            var r = from y in db.customer_contact select y;
            var totalmsg=r.Count();
            ViewBag.message = totalmsg;

             var employees = from e in db.orders select e.total_amount  ;
             
             int amount = employees.Sum();
             ViewBag.amount = amount;


             var categories = from p in db.orders
                              group p by p.customer_name into g
                              select new test { Name = g.Key, Total_Amount = g.Sum(p => p.total_amount) };

             ViewBag.ct = categories;


             var user = from u in db.orders.Where(u => u.status == "Processing") select u;
            var neworder=user.Count();
            ViewBag.neworder = neworder;
             ViewBag.requser = user;

            return View();
       

        }


        public ActionResult adminprofile()
        {
            string name = (string)Session["username"];
            if (Session["username"] != null)
            {
                ViewBag.account = db.users.Single(x => x.Username == name);

                return View();

            }

            return RedirectToAction("Index");

        }

        public ActionResult customerfeedback()
        {
            ViewBag.report = db.customer_contact.ToList();

            return View();
        
        }


        public ActionResult approve(int id)

        {
            order o = new order();
            o.product_id = id;
            db.spApprove(o.product_id);
      
      return RedirectToAction("orderlist");
        }

        [HttpGet]
        public ActionResult deleteuser(int id)
        {
        

            db.spDeleteuser(id);

            return RedirectToAction("registereduser");
        }


        [HttpGet]
        public ActionResult deleteorder(int id)
        {


            db.spOrderemove(id);

            return RedirectToAction("orderlist");
        }

        [HttpGet]
        public ActionResult deleteproducts(int id)
        {


            db.spremoveProducts(id);

            return RedirectToAction("Productlist");
        }

    }
}
