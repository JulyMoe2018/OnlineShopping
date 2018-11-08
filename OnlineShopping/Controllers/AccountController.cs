using OnlineShopping.Models;
using OnlineShopping.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineShopping.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        [HttpGet]
        public ActionResult Registration()
        {
            var citylist = _ctx.Cities.ToList();

            var ViewModel = new NewCityViewModel
            {
                ListofCity = citylist
            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(Registration register)
        {
            if (ModelState.IsValid)
            {
                _ctx.Registrations.Add(register);
                _ctx.SaveChanges();
                return RedirectToAction("LogIn", "Account");
            }
            else
            {
                ModelState.AddModelError("", "Data is not correct");
            }

            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(OnlineShopping.Models.Register user)
        {  
            if (IsValid(user.Email, user.Password))
            {
                Session["User"] = user.Email;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Login details are wrong.");
            }
            return View(user);
        }

        private bool IsValid(string email, string password)
        {
            bool IsValid = false;

                var user = _ctx.Registrations.FirstOrDefault(u => u.Email == email && u.Password == password);
                if (user != null)
                {
                    IsValid = true;                 
                }
            return IsValid;
        }
        public ActionResult LogOut()
        {
             Session["User"] = null;
             return RedirectToAction("LogIn", "Account");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword model)
        {
            if(ModelState.IsValid)
            {
                string email = Session["User"].ToString();

                var user = (from u in _ctx.Registrations
                            where u.Email == email
                            && u.Password == model.OldPassword
                            select u).FirstOrDefault();

                if(user != null)
                {
                    user.Password = model.NewPassword;
                    _ctx.SaveChanges();
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect");
                }
            }

            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
    }
}