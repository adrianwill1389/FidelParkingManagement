using Microsoft.AspNetCore.Mvc;

namespace FidelParkingManagement.Controllers
{
    public class ReservationController : Controller
    {
        public IActionResult Index()
        {
            return View();

        }
    }
}
