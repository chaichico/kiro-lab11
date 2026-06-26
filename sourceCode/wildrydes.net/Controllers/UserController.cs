using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using wildrydes.net.Context;
using wildrydes.net.Models;

namespace wildrydes.net.Controllers
{
    public class UserController : Controller
    {
        private DefaultContext db = new DefaultContext();

        // GET: User/Logout
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        // GET: User/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserModel userModel)
        {
            if (!string.IsNullOrEmpty(userModel.Email) && !string.IsNullOrEmpty(userModel.Password))
            {
                // get user from database
                UserModel user = db.Users.Where(u => u.Email == userModel.Email).FirstOrDefault();
                if (user != null)
                {
                    // check password
                    if (GetHash(userModel.Password) == user.Password)
                    {
                        // set session
                        Session["user"] = user.Email;
                        Session["role"] = "user";
                        if (!string.IsNullOrEmpty(Request.QueryString["or"]))
                        {
                            return Redirect(Request.QueryString["or"]);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            ViewBag.Error = "Invalid login attempt";
            return View(userModel);
        }


        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,Password")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                userModel.Id = Guid.NewGuid();
                // update password to save hashed value
                userModel.Password = GetHash(userModel.Password);
                db.Users.Add(userModel);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(userModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public static string GetHash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
