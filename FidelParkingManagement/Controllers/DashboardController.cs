using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class DashboardController : Controller
{
    public IActionResult Index()
    {
        var model = new DashboardViewModel
        {
            Name = "Jane Dunkley",
            Membership = "Silver",
            WalletAmount = 12500.00
        };

        return View(model);
    }

    public IActionResult LoadSummary()
    {
        var model = new DashboardViewModel
        {
            Name = "Jane Dunkley",
            Membership = "Silver",
            WalletAmount = 12500.00
        };

        return PartialView("_UserSummary", model);
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
    public string Name { get; set; }
    public string Membership { get; set; }
    public double WalletAmount { get; set; }
}

