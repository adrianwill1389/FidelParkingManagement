using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class DashboardController : Controller
{
    public IActionResult Index()
    {
        // Sample data - replace with actual data from database
        //var dashboardData = new DashboardViewModel
        //{
        //    WalletAmount = 12500.00M, // Fetch from DB
        //    MembershipStatus = "Silver" // Fetch from DB
        //};

        return View(); // ✅ This loads Index.cshtml as a full page
    }

    public IActionResult MyCar()
    {
        return View(); // ✅ Load the full MyCar.cshtml page
    }

    public IActionResult PayOnline()
    {
        return View(); // ✅ Load the full PayOnline.cshtml page
    }

    public IActionResult FindASpot()
    {
        return View(); // ✅ Load the full FindASpot.cshtml page
    }

    public IActionResult ReserveParking()
    {
        return View(); // ✅ Load the full ReserveParking.cshtml page
    }

    public IActionResult ReportIssue()
    {
        return View(); // ✅ Load the full ReportIssue.cshtml page
    }

    public IActionResult MyWallet()
    {
        var walletData = new DashboardViewModel
        {
            WalletAmount = 9000.00M,
            MembershipStatus = "Silver"
        };

        return View(walletData); // ✅ Correctly loads MyWallet.cshtml inside `_Layout.cshtml`
    }


}

// ✅ Make sure DashboardViewModel only contains needed properties
public class DashboardViewModel
{
    public decimal WalletAmount { get; set; }
    public string MembershipStatus { get; set; }
}
