using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CookieDemo.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string userName, string password)
        {
            if(string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                TempData["danger"] = "Invalid Username or Password!!";
                return View();
                //return RedirectToAction("Login");
            }

            //Check the user name and password
            //Here can be implemented checking logic from the database
            ClaimsIdentity identity = null;
            bool isAuthenticated = false;

            if (userName=="Admin" && password == "password"){
                
                //Create the identity for the user
                identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Role, "Admin")//you can decide your own claim type
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                isAuthenticated = true;
            }

            if (userName == "Mukesh" && password == "password")
            {
                //Create the identity for the user
                identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Role, "User")
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                isAuthenticated = true;
            }

            if (isAuthenticated)
            {
                var principal = new ClaimsPrincipal(identity);

                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.User = principal;
                Thread.CurrentPrincipal = principal;

                var usr = HttpContext.User;
                var usrClaims = HttpContext.User.Claims;
                var usrIdentities = HttpContext.User.Identities;
                var usrAdminRole = HttpContext.User.IsInRole("Admin");

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        
        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}