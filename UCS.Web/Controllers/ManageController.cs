using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UCS.Db.Entities;
using UCS.Web.ViewModels;

namespace UCS.Web.Controllers
{
    public class ManageController : AbstractController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        public ActionResult ChangePassword()
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_isAdministrator)
            {
                return RedirectToAction("Menu", "Home");
            }

            ChangePasswordViewModel model = new ChangePasswordViewModel();
            model.UserName = GetEmail();
            model.HasError = false;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (string.IsNullOrEmpty(model.OldPassword))
            {
                ModelState.AddModelError("", "Należy podać aktualne hasło");
            }
            if (string.IsNullOrEmpty(model.NewPassword))
            {
                ModelState.AddModelError("", "Należy podać nowe hasło");
            }
            if (string.IsNullOrEmpty(model.ConfirmNewPassword))
            {
                ModelState.AddModelError("", "Należy potwierdzić nowe hasło");
            }
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                ModelState.AddModelError("", "Potwierdzenie nie jest zgodne z nowym hasłem");
            }

            if (ModelState.IsValid)
            {
                var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Menu", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Nowe hasło musi zawierać przynajmniej jeden znak niebędący literą, cyfrę, jak również małą i wielką literę");
                }
            }

            model.HasError = true;
            return View(model);
        }
    }
}