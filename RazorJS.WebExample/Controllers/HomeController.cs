using System.Web.Mvc;

namespace MvcApplication7.Controllers
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
