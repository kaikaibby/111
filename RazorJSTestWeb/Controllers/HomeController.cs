using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RazorJSTestWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            object model = null;
            return View(model);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}