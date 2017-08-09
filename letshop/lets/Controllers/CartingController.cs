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


        private int isExisting(int id)
        {
            List<product> cart = (List<product>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].product_id == id)
                    return i;
            return -1;

        }



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


       

     
       




    }






}
