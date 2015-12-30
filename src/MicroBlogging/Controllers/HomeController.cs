using Microsoft.AspNet.Mvc;

namespace MicroBlogging.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}