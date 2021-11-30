using Microsoft.AspNetCore.Mvc;
using System;
using CA_Project.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA_Project.Data;

namespace CA_Project.Controllers
{
    public class CartController : Controller
    {
        private readonly SessionDictionary _sessionDict;
        private readonly ApplicationDBContext _db;
        public CartController(ApplicationDBContext db, SessionDictionary sessiondict)
        {
            _sessionDict = sessiondict;
            _db = db;
        }
        public IActionResult Index()
        {
            if (!CheckLoginStatus())
            {
                return RedirectToAction("index", "home");
            }
            
            //get prodname, prod descrip and prod img, product price and quantity from cart_products table

            //find userid
            string sessionid = GetSessionID();
            Session sess1 = _db.Sessions.FirstOrDefault(x =>
                x.Id.ToString() == sessionid
            );

            //use userid to query for the cartid
            Cart cart1 = _db.Carts.FirstOrDefault(x =>
                x.User.Id == sess1.User.Id
            );
            if (cart1 == null)
            {
                ViewData["cartid"] = null;
                ViewData["cartProducts"] = null;
            }
            else
            {
                List<CartProduct> cartProducts = (List<CartProduct>)cart1.CartProducts;
                ViewData["cartid"] = cart1.Id;
                ViewData["cartProducts"] = cartProducts;
            }
            return View();
        }

        public IActionResult UpdateQuantity([FromBody] CartProduct cp)
        {
            if (!CheckLoginStatus())
            {
                return LocalRedirect("/home/index");
            }
            CartProduct cartProduct = _db.CartProducts.FirstOrDefault(x =>
                x.Id == cp.Id
            );
            if (cartProduct == null)
            {
                return Json(null);
            }
            cartProduct.Quantity = cp.Quantity;
            _db.SaveChanges();
            return Json(new
            {
                success = true
            });

        }

        public IActionResult RemoveProduct([FromBody] CartProduct cp)
        {
            string sessionId = GetSessionID();

            Session session = _db.Sessions.FirstOrDefault(x =>
               x.Id.ToString() == sessionId //find session
           );

            if (!CheckLoginStatus())
            {
                return LocalRedirect("home/index");
            }
            CartProduct cartProduct = _db.CartProducts.FirstOrDefault(x =>
                x.Id == cp.Id
            );
            if (cartProduct == null)
            {
                return Json(null);
            }
            _db.CartProducts.Remove(cartProduct);
            _db.SaveChanges();
            Cart RemoveCArt = _db.Carts.FirstOrDefault(x =>
                x.User.Id == session.User.Id);


            if (RemoveCArt.CartProducts.Count == 0)// check again 
            {

                _db.Carts.Remove(RemoveCArt);
                _db.SaveChanges();
                return Json(new
                {
                    deletecart =true,
                    delete = true
                });

            }

            return Json(new
            {
                delete = true

            });
        }

        public IActionResult Checkout([FromBody] Cart cart)
        {
            //populate the purchase table => purchaseProducts table
            //get the cartid from ajax
            string sessionid1 = GetSessionID();
            Cart cart1 = _db.Carts.FirstOrDefault(x =>
               x.Id == cart.Id
           );
            if (cart1 == null)
            {
                return Json(null);
            }
            if (_sessionDict.CheckGuestListContainsGuestId(sessionid1))
            {
                return Json(false);
            }
            Purchase purchase1 = new Purchase() { User = cart1.User, Date = DateTime.Now };
            //calculate total amount
            float total = 0.0f;
            List<CartProduct> productsToDelete = new List<CartProduct>();
            foreach (CartProduct cp in cart1.CartProducts)
            {
                total += cp.Product.Price * cp.Quantity;
                for (int i = 0; i < cp.Quantity; i++)
                {
                    purchase1.PurchaseProducts.Add(new PurchaseProduct()
                    {

                        Product = cp.Product
                    });
                }
                _db.CartProducts.Remove(cp);
            }
            purchase1.TotalAmount = total;
            _db.Purchases.Add(purchase1);
            _db.Carts.Remove(cart1);

            _db.SaveChanges();
            return Json(true);
            //use cartid to retrieve cart
            //retrieve the products from cart
            //get the userid
        }


        private string GetSessionID()
        {
            return Request.Cookies["sessionid"];
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
    }
}
