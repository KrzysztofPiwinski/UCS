﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UCS.Db.Entities;
using UCS.Web.Models;
using UCS.Web.Models.Repositories;
using UCS.Web.ViewModels;

namespace UCS.Web.Controllers
{
    public class AccountController : AbstractController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private UserRepository _repository = null;

        public AccountController() : base()
        {
            _repository = new UserRepository();
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            model.HasError = false;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError("", "Należy podać adres e-mail");
            }
            else
            {
                if (!model.Email.Contains('@'))
                {
                    ModelState.AddModelError("", "Należy podać prawidłowy adres e-mail");
                }
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("", "Należy podać hasło");
            }

            if (ModelState.IsValid)
            {
                User admin = _repository.GetByUserName(model.Email);
                if (admin != null && admin.IsActive)
                {
                    var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                    if (result == SignInStatus.Success)
                    {
                        return RedirectToAction("Menu", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Nieprawidłowe hasło");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Użytkownik nie istnieje lub jest nieaktywny");
                }
            }

            model.HasError = true;
            return View(model);
        }

        public ActionResult LoginStudent()
        {
            string url = UsosHelpers.Request();

            return Redirect(url);
        }

        public ActionResult UsosCallback(string oauth_token, string oauth_verifier)
        {
            UsosHelpers.Access(oauth_token, oauth_verifier);

            return RedirectToAction("Menu", "Home");
        }

        public ActionResult Logout()
        {
            Init(User);

            if (_isAdministrator)
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            }

            if (_isStudent)
            {
                Session["ucs_student_guid"] = null;

                return Redirect("https://cas.prz.edu.pl/cas-server/logout");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}