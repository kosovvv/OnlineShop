using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.WebAPI.Controllers
{
    public class ReviewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
