using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCS.Web.ViewModels;

namespace UCS.Web.Controllers
{
    public class HomeController : AbstractController
    {
        public ActionResult Menu()
        {
            if (!Init(User))
            {
                return RedirectToAction("Index", "Home");
            }

            LayoutViewModel model = new LayoutViewModel();
            model.UserName = GetEmail();
            return View(model);
        }

        public ActionResult Index()
        {
            if (Init(User))
            {
                return RedirectToAction("Menu", "Home");
            }

            LayoutViewModel model = new LayoutViewModel();
            return View(model);
        }
    }
}