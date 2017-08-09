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

        //ADMIN
        [HttpGet]
        public ActionResult admin()
        {
            //ViewBag.subcat = new SelectList(db.sub_category, "subcat_id", "subcat_name");
            // return View();

            List<SelectListItem> subcatlist = new List<SelectListItem>();
            foreach (sub_category d in db.sub_category)
            {
                SelectListItem li = new SelectListItem();
                li.Text = d.subcat_name;
                li.Value = d.subcat_name;
                subcatlist.Add(li);
            }

            ViewBag.subcat = subcatlist;
            return View();
        }

        [HttpPost]
        [ActionName("admin")]
        public ActionResult admin_post(HttpPostedFileBase postedFile, FormCollection formcollection)
        {


            product pb = new product();
            pb.subcat_name = formcollection["value"];

            string relativePath = "/Content/img/" + pb.subcat_name + "/";
            string path = Server.MapPath("~" + relativePath);
            // string path = Server.MapPath("~/Content/img/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
            string loc = (path + Path.GetFileName(postedFile.FileName)).ToString();

            // pb.category = formcollection["category"];
            pb.quantity = Convert.ToInt32(formcollection["quantity"]);
            pb.price = Convert.ToInt32(formcollection["price"]);
            pb.description = formcollection["Description"];

            pb.img_loc = loc;

            TryUpdateModel(pb);

            if (ModelState.IsValid)
            {

                db.spADDProduct(pb.quantity, pb.price, pb.description, pb.subcat_name, pb.img_loc);
                return RedirectToAction("admin");

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


        private int isExisting(int id)
        {
            List<product> cart = (List<product>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].product_id == id)
                    return i;
            return -1;

        }


        //CARTING SYSTEM


        [HttpPost]

        public ActionResult cart(FormCollection form)
        {
            int id = Convert.ToInt32(form["id"]);

            if (Session["cart"] == null)
            {
                Session["count"] = 0;
                List<product> cart = new List<product>();

                var addedProduct = db.products.Single(product => product.product_id == id);

                cart.Add(addedProduct);

                Session["cart"] = cart;

                Session["count"] = Convert.ToInt32(Session["count"]) + 1;
                //Session["count"] = 1;

            }
            else
            {
                List<product> cart = (List<product>)Session["cart"];
                int index = isExisting(id);
                if (index == -1)
                {
                    var addedProduct = db.products.Single(product => product.product_id == id);

                    cart.Add(addedProduct);

                    Session["count"] = Convert.ToInt32(Session["count"]) + 1;
                }
                else
                    cart[index].quantity++;


                Session["cart"] = cart;



            }

            return View();


        }




        //cart product remove
        [HttpGet]
        public ActionResult remove_cart(int id)
        {

            List<product> cart = (List<product>)Session["cart"];
            cart.RemoveAll(x => x.product_id == id);

            Session["count"] = Convert.ToInt32(Session["count"]) - 1;
            return View("cart");


        }

        /////show all cart

        [HttpGet]
        public ActionResult showallcart()
        {
            if (Session["cart"] != null)
            {


                return View((List<product>)Session["cart"]);
            }


            return RedirectToAction("Index");

        }


        //CART UPDATE

        public ActionResult update(FormCollection form)
        {
            List<product> cart = (List<product>)Session["cart"];
            int id = Convert.ToInt32(form["product_id"]);
            int quantity = Convert.ToInt32(form["quantity"]);

            foreach (var item in cart.Where(w => w.product_id == id))
            {
                item.quantity = quantity;
            }




            return RedirectToAction("showallcart");
        }





        //CHECKOUT
        [HttpGet]
        public ActionResult checkout()
        {
            if (Session["username"] == null)
            {

                return RedirectToAction("register");
            }

            return View();
        }


        //LOGOUT

        public ActionResult cancel()
        {
            Session.Remove("cart");
            Session.Remove("count");
            return RedirectToAction("Index");
        }

        //placeorder
        [HttpPost]
        public ActionResult placeorder(FormCollection form)
        {
            List<product> cart = (List<product>)Session["cart"];
            foreach (var item in cart)
            {
                order ord = new order();
                //ord.product_id = item.product_id;
                ord.customer_name = (string)Session["username"];
               // ord.quantity = item.quantity;
               // ord.total_amount = item.quantity * item.price;
               // TryUpdateModel(ord);
               

                   // db.spAddOrder(ord.product_id, ord.customer_name, ord.quantity, ord.total_amount);

                db.spAddOrder(item.product_id, ord.customer_name, item.quantity, item.quantity * item.price);

               
                }


            Session.Remove("cart");
            Session.Remove("count");
            return RedirectToAction("account");
        }


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
