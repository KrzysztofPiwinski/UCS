using System.Web.Mvc;
using UCS.Web.Models;

namespace UCS.Web.Controllers
{
    public class AccountController : AbstractController
    {
        public ActionResult Login()
        {
            string url = UsosHelpers.Request();

            return Redirect(url);
        }

        public ActionResult UsosCallback(string oauth_token, string oauth_verifier)
        {
            UsosHelpers.Access(oauth_token, oauth_verifier);

            return RedirectToAction("Menu", "Home");
        }
    }
}