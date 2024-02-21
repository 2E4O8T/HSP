using Microsoft.AspNetCore.Mvc;

namespace HMI.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
