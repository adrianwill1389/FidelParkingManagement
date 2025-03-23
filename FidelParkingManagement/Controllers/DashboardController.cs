using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class DashboardController : Controller
{
    public IActionResult Index()
    {
     
        return View(); // ✅ This loads Index.cshtml as a full page
    }

    public IActionResult MyCar()
    {
        return PartialView(); // ✅ Load the full MyCar.cshtml page
    }

    public IActionResult PayOnline()
    {
        return PartialView(); // ✅ Load the full PayOnline.cshtml page
    }

    public IActionResult FindASpot()
    {
        return PartialView(); // ✅ Load the full FindASpot.cshtml page
    }

    public IActionResult ReserveParking()
    {
        return PartialView(); // ✅ Load the full ReserveParking.cshtml page
    }

    public IActionResult ReportIssue()
    {
        return PartialView(); // ✅ Load the full ReportIssue.cshtml page
    }

}

// ✅ Make sure DashboardViewModel only contains needed properties
public class DashboardViewModel
{
    public decimal WalletAmount { get; set; }
    public string MembershipStatus { get; set; }
}
