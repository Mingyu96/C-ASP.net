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
    public class ProductHomeController : Controller
    {
        private readonly ApplicationDBContext db;
        private readonly SessionDictionary _sessionDict;
        public ProductHomeController(ApplicationDBContext db, SessionDictionary sessionDict)
        {
            this.db = db;
            _sessionDict = sessionDict;
        }
        public IActionResult Index()//return view for shopping main page
        {
            if (!CheckLoginStatus())
            {
                return LocalRedirect("/home/GuestUserSession");
            }
            //get from database list of products and move to view
            List<Product> products = db.Products.ToList();
            ViewData["products"] = products;
            ViewData["cart"] = GetCart();
            return View();
        }

        public Cart GetCart()//
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

        public IActionResult AddToCart([FromBody] Product product)//Proccess Ajax 'Adding to cart' request, update database and return Json
        {
            Product prod1 = db.Products.FirstOrDefault(x =>
                x.Id == product.Id
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
            if (cart1 == null)
            {
                //create new cart
                cart1 = new Cart();
                cart1.User = user1;
                cart1.CartProducts.Add(new CartProduct
                {
                    Quantity = 1,
                    Product = prod1
                });
                db.Carts.Add(cart1);
                db.SaveChanges();
                return Json(new
                {
                    updated = true
                });
            }
            else if(cart1 !=null)
            {
                //existing cart present, return true only if item is not already present in cart
                bool isPresent = false;
                foreach (CartProduct c in cart1.CartProducts)
                {
                    //find if product is already inside
                    if (c.Product.Id == prod1.Id)
                    {
                        if (c.Quantity < 99) //less than 99,normal adding
                        {
                            c.Quantity += 1;
                            isPresent = true;
                            db.SaveChanges();
                            return Json(new
                            {
                                updated = true,
                                limit = false
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
                        Quantity = 1,
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

        

        public string GetSessionID() //Get current session id
        {
            return Request.Cookies["sessionid"];
        }

       

        private bool CheckLoginStatus()// Check session for log in status
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
    }
    
}
