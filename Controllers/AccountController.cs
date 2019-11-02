using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using LampWebStore.Models;
using LampWebStore.Views.ViewModels;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace LampWebStore.Controllers
{
    /// <summary>
    /// Controller for authentication stuff.
    /// </summary>
    public class AccountController: Controller
    {
        /// <summary>Used to get and verify password's hashes.</summary>
        private readonly IPasswordHasher<User> passwordHasher;


        /// <summary>DB context to work with lamps store DB.</summary>
        private readonly LampsContext db;


        public AccountController(LampsContext db, IPasswordHasher<User> passwordHasher) //DI...
        {
            this.db = db;
            this.passwordHasher = passwordHasher;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserSingInViewModel user)
        {
            if(!ModelState.IsValid)
                return View(user);

            User u = db.Users.FirstOrDefault(o => o.Login == user.Login.ToLower() &&
                passwordHasher.VerifyHashedPassword(null, o.PasswordHash, user.Password) != PasswordVerificationResult.Failed);

            if(u is null)
            {
                ModelState.AddModelError("", "Wrong password or login");

                return View(user);
            }

            await Authenticate(u);

            return RedirectToAction("Products", "Home"); //Actually we should send user right back where he/she came from...
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserSingUpViewModel user)
        {
            if(ModelState.IsValid)
            {
                if(db.Users.Any(o => o.Login == user.Login.ToLower()))
                { ModelState.AddModelError("", "This login is taken already please use other one"); }
                else if(db.Users.Any(o => o.Email == user.Email.ToLower()))
                { ModelState.AddModelError("", "This EMail is taken already please use other one"); }
                else
                {
                    var u = new User {
                        Login = user.Login.ToLower(),
                        Email = user.Email.ToLower(),
                        PasswordHash = passwordHasher.HashPassword(null, user.Password)
                    };

                    db.Users.Add(u);
                    await db.SaveChangesAsync();

                    await Authenticate(u);

                    return RedirectToAction("Products", "Home");
                }
            }

            return View(user);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Products", "Home");
        }

        [Authorize]
        [ActionName("RemoveAccount")]
        public IActionResult RemoveAccountConfirm()
        {
            return View();
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAccount()
        {
            //User login has been put in this property, see Authenticate()
            string login = HttpContext.User.Identity.Name;

            User u = db.Users.FirstOrDefault(o => o.Login == login) ??
                throw new InvalidOperationException($"Can't find user ({login}) in DB although he/she was authenticated.");

            //Log out
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //Remove the user from DB
            db.Users.Remove(u);
            await db.SaveChangesAsync();

            return RedirectToAction("Products", "Home");
        }

        /// <summary>
        /// Sing in this user.
        /// </summary>
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
                //Here could be added more user's claims...
            };

            var identity = new ClaimsIdentity(claims, 
                "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            
            //Set up the auth cookie
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }
    }
}