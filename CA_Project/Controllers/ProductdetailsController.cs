using Microsoft.AspNetCore.Mvc;
using CA_Project.Data;
using System;
using CA_Project.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace CA_Project.Controllers
{
    public class ProductdetailsController : Controller
    {
        private readonly ApplicationDBContext db;
        private readonly SessionDictionary _sessionDict;
        public ProductdetailsController(ApplicationDBContext db, SessionDictionary sessionDict)
        {
            this.db = db;
            _sessionDict = sessionDict;
        }

        [Route("/productdetails/details/{name}")]
        public IActionResult details(string name)//Get feedback and product detail from databse with data from route parameter, then show specific view
        {
            string session_id = GetSessionID();
            bool guest = _sessionDict.CheckGuestListContainsGuestId(session_id);
            if (!CheckLoginStatus())
            {
                return LocalRedirect("/home/index");
            }

            //get from database list of products and move to view

            Product product = db.Products.FirstOrDefault(x =>
           x.ProductName == name
                );

            List<Feedback> feedbacks = db.Feedbacks.Where(x =>
            x.Product == product
                ).ToList();

            ViewData["cart"] = GetCart();

            ViewData["feedbacks"] = feedbacks;
            ViewData["guest"] = guest;


            ViewData["product"] = product;
            // ViewData["cart"] = GetCart();
            return View();
        }


        public Cart GetCart()
        {
            string sessionId = GetSessionID();
            Session session1 = db.Sessions.FirstOrDefault(x =>
                x.Id.ToString() == sessionId
            );
            if (session1 == null)
            {
                return null;
            }
            Cart cart1 = db.Carts.FirstOrDefault(x =>
                x.User.Id == session1.User.Id
            );
            return cart1;
        }
        private Session GetSession()
        {
            string sessionId = GetSessionID();
            Session session1 = db.Sessions.FirstOrDefault(x =>
                x.Id.ToString() == sessionId
            );
            if (session1 == null)
            {
                return null;
            }
            return session1;
        }



        public IActionResult AddToCart([FromBody] AddToCartItem item)//Proccess Ajax 'Adding to cart' request, update database and return Json
        {
            int num = item.num;



            Product prod1 = db.Products.FirstOrDefault(x =>
                x.Id == item.Id
            );
            string sessionid = GetSessionID();



            //use session to query for userid
            Session session1 = db.Sessions.FirstOrDefault(x =>
                x.Id.ToString() == sessionid
            );
            User user1 = db.Users.FirstOrDefault(x =>
                x.Id == session1.User.Id
            );
            //find existing cart
            Cart cart1 = db.Carts.FirstOrDefault(x =>
                x.User.Id == user1.Id
            );
            if (num > 99) // quantity limit = 99 for single purchase of 1 product
            {
                return Json(new
                {
                    updated = false,
                    limit = true
                });
            }
            if (cart1 == null)
            {
                //create new cart
                cart1 = new Cart();
                cart1.User = user1;
                cart1.CartProducts.Add(new CartProduct
                {
                    Quantity = num,
                    Product = prod1
                });
                db.Carts.Add(cart1);
                db.SaveChanges();
                return Json(new
                {
                    updated = true
                });
            }
            
            else if (cart1 != null)
            {
                //existing cart present, return true only if item is not already present in cart
                bool isPresent = false;
                foreach (CartProduct c in cart1.CartProducts)
                {

                    //find if product is already inside
                    if (c.Product.Id == prod1.Id)
                    {
                        if (c.Quantity + num <= 99) //less than 99,normal adding
                        {
                            c.Quantity += num;
                            isPresent = true;
                            db.SaveChanges();
                            return Json(new
                            {
                                updated = true
                            });
                        }
                        else //quantity limit = 99 for single purchase of 1 product
                        {
                            return Json(new
                            {
                                updated = false,
                                limit = true
                            });

                        }

                    }
                }

                if (!isPresent)
                {
                    cart1.CartProducts.Add(new CartProduct
                    {
                        Quantity = num,
                        Product = prod1
                    });
                    db.SaveChanges();
                    bool[] response = new bool[2];
                    response[0] = true;
                    response[1] = true;
                    return Json(new
                    {
                        updated = true
                    });
                }
            }
            return Json(new
            {
                updated = false
            });
        }




        private string GetSessionID()
        {
            return Request.Cookies["sessionid"];
        }







        [Route("/Productdetails/Renew/{details}/{Rating}/{name}")]
        public IActionResult Renew(string details, int Rating, string name)//Process with Ajax request of updating product feedback and refresh the page
        {

            Session session = GetSession();

            User user = db.Users.FirstOrDefault(x => x == session.User);

            Product pro = db.Products.FirstOrDefault(x =>
           x.ProductName == name
                );

            db.Feedbacks.Add(new Models.Feedback
            {
                Details = details,
                Rating = Rating,
                Product = pro,
                User = user


            });
            db.SaveChanges();

            string path = "https://localhost:44328/Productdetails/details/" + name;
            return Redirect(path);
        }


        private bool CheckLoginStatus()
        {
            string sessionid = Request.Cookies["sessionid"];
            if (sessionid == null)
            {
                return false;
            }
            long? time = _sessionDict.CheckSessionPresence(sessionid);
            if (time == null)
            {
                return false;
            }
            else
            {
                //check if time is expired
                if ((time + Session.timeout) < DateTimeOffset.Now.ToUnixTimeSeconds())
                {
                    return false;
                }
            }
            return true;
        }




        public IActionResult Index()
        {
            return View();
        }
    }
}
