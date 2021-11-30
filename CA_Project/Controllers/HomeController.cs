using CA_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using CA_Project.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CA_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext db;
        private readonly SessionDictionary _sessionDict;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext db, SessionDictionary sessionDict)
        {
            _logger = logger;
            this.db = db;
            _sessionDict = sessionDict;
        }

        public IActionResult Index()
        {
            if (CheckLoginStatus())
            {
                return LocalRedirect("/producthome/index");
            }
            List<Session> sessions = db.Sessions.ToList();
            _sessionDict.InputStoredSessions(sessions);
            return View();
        }

        //Add sign up user into database
        public IActionResult Create(string id, string password, string email, string fullname) //Add sign up user into database
        {
            db.Users.Add(new Models.User
            {
                Username = id,
                Password = password,
                Email = email,
                FullName = fullname
            });
            db.SaveChanges();
            return LocalRedirect("/home/signupsuccess");
      
        }

        //Receive username from Ajax the validate using database data
        public IActionResult CheckUser([FromBody] User user)
        {
            
         List<User> u = db.Users.Where(x => x.Id != null).ToList();
                if (u != null)
                {
                    foreach (User id in u)
                    {
                        if (user.Username.ToLower() == id.Username.ToLower())
                        {
                            return Json(new { isOkay = false });
                        }
                    }
                }
                return Json(new { isOkay = true });
        }

        public IActionResult Register()
        {
            return View();
        }


        private bool CheckLoginStatus()
            {

                string sessionid = Request.Cookies["sessionid"];
                bool guest = _sessionDict.CheckGuestListContainsGuestId(sessionid);

                if (sessionid == null||guest==true)
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


        public IActionResult Logout() //Logout and remove cookies and sessions
        {
            string s = Request.Cookies["sessionid"];
            //dont allow guest to logout, hide logout display button

            if (s == null)
            {
                return LocalRedirect("/home/index");
            }
            if (_sessionDict.CheckGuestListContainsGuestId(s))
            {
                //create a cookie to store the current sessionid for guest
                //so when log in again, can retrieve the previous sessionid
                //and go to cart database to retrieve items and change the user 
                //to current user
                Response.Cookies.Append("guestid", s);
            }
            else
            {
                Session sess = db.Sessions.FirstOrDefault(x =>
                x.Id.ToString() == s
                );
                Response.Cookies.Delete("name");
                db.Sessions.Remove(sess);
                db.SaveChanges();
            }
            Response.Cookies.Delete("sessionid");
            _sessionDict.RemoveSession(s);
            return LocalRedirect("/home/index");
        }

        public IActionResult LoginFailed() // return login failed page
        {
            return View();
        }


        public IActionResult Login(string username, string password)
        {
            if (CheckLoginStatus())
            {
                return LocalRedirect("/producthome/index");
            }
            //validate username and password
            User user1 = db.Users.FirstOrDefault(x =>
                x.Username.ToLower() == username.ToLower() && x.Password == password
            );
            if (user1 == null) 
            {
                return LocalRedirect("/home/LoginFailed");
            }
            //validate successfully
            else
            {
                //check cookie if there is any guestid
                string guestid = Request.Cookies["sessionid"]; // get current session id
                bool Guest = _sessionDict.CheckGuestListContainsGuestId(guestid); // check if current session id in guest list
                if (Guest == true) //if  guest session exist
                {
                    //use the guestid which is previously the sessionid of guest
                    //search the database for similar sessionid for fake userid which is tagged to cart
                    Session prevGuestSess = db.Sessions.FirstOrDefault(x =>
                        x.Id.ToString() == guestid
                    );
                    Cart guestCart = db.Carts.FirstOrDefault(x => //get guest cart using guest session id
                        x.User.Id == prevGuestSess.User.Id
                    );
                    Cart existingcart = db.Carts.FirstOrDefault(x => //get existing cart
                        x.User.Id == user1.Id);
                    if (guestCart != null && existingcart == null) //if user have no existing cart, make guest cart into user cart
                    {
                        guestCart.User = user1;
                    }
                    else if (guestCart != null && existingcart != null) //Merge the cart if both login user cart and guest cart is not null
                    {
                        foreach (CartProduct item in guestCart.CartProducts)
                        {
                            CartProduct DuplicateProduct = existingcart.CartProducts.FirstOrDefault(x => x.Product == item.Product);
                            if (DuplicateProduct != null) // if product is duplicate, get the sum of quantity
                            {
                                if (DuplicateProduct.Quantity + item.Quantity <= 99) //Normal adding if quantity less than 99 after merging
                                {
                                    DuplicateProduct.Quantity += item.Quantity; //Quantity = 99 if exceed limit(99) after merging
                                }
                                else
                                {
                                    DuplicateProduct.Quantity = 99;
                                }
                            } 
                            else {
                                item.Cart = existingcart;
                            } //if product not duplicate, add into existing cart
                        }
                        db.Carts.Remove(guestCart); //remove guest cart from database
                        db.SaveChanges();
                    }

                    //update&remove guest cookies,session & database
                    User guest = db.Users.FirstOrDefault(x => x.Id == prevGuestSess.User.Id);
                    _sessionDict.RemoveGuestList(guestid);
                    db.Sessions.Remove(prevGuestSess);
                    db.SaveChanges();
                    db.Users.Remove(guest);
                    db.SaveChanges();
                    Response.Cookies.Delete("guestid");
                }
                //create a session
                Session userSession = new Session();
                userSession.User = user1;

                //add session to db
                db.Sessions.Add(userSession);
                db.SaveChanges();

                //add session to sessiondict
                _sessionDict.AddSession(userSession.Id.ToString(), userSession.TimeStamp);

                //set cookie
                Response.Cookies.Append("sessionid", userSession.Id.ToString());
                Response.Cookies.Append("name", user1.FullName);
                ViewData["sessionTime"] = userSession.TimeStamp;
                return LocalRedirect("/ProductHome/Index");
            }
        }


        public IActionResult SignupSuccess()
        {
            return View();
        }

        public IActionResult GuestUserSession() //create session and fake user 
        {
            if (CheckLoginStatus())
            {
                //if logged in already
                return LocalRedirect("producthome/index");
            }
            else
            {
                Session userSession = new Session();
                //create fake user for guest
                User guestUser = new User()
                {
                    Username = "guestUser",
                    Password = "guestUser",
                    FullName = "guestUser",
                    Email = "guestUser"
                };
                userSession.User = guestUser;

                //add session to db
                db.Sessions.Add(userSession);
                db.SaveChanges();

                //add session to sessiondict
                _sessionDict.AddSession(userSession.Id.ToString(), userSession.TimeStamp);

                //set cookie
                Response.Cookies.Append("sessionid", userSession.Id.ToString());
                Response.Cookies.Append("name", "Guest");
                ViewData["sessionTime"] = userSession.TimeStamp;

                _sessionDict.InitialiseGuestList(userSession.Id.ToString());

          
                return LocalRedirect("/producthome/index");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
} 
