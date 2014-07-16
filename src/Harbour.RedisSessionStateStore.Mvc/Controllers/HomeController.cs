using System.Web.Mvc;

namespace Harbour.RedisSessionStateStore.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string name, int age)
        {
            Session["name"] = name;
            Session["age"] = age;
            return RedirectToAction("index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult AbandonSession()
        {
            Session.Abandon();

            return RedirectToAction("Index");
        }
    }
}