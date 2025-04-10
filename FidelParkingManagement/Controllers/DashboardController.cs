using FidelParkingManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;

public class DashboardController : Controller
{
    private readonly FidelParkingDbContext _context; 

    public DashboardController(FidelParkingDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        // Sample data - replace with actual data from database
        var dashboardData = new DashboardViewModel
        {
            WalletAmount = 12500.00M, // Fetch from DB
            MembershipStatus = "Silver" // Fetch from DB
        };

        return View(); // ✅ This loads Index.cshtml as a full page
    }


    public async Task<IActionResult> MyCar()
    {
        var userName = TempData["username"];
        if (userName != null)
        {
            try
            {
                // Keep the TempData for the next request
                TempData.Keep("username");
                //Get vehicle data for the specific user license plate
                var vehicle = await _context.VehiclesDetecteds
                    .Include(v => v.Media)
                    .Include(v => v.Payment)
                .FirstOrDefaultAsync(v => v.LicensePlateNumber.Equals(userName));

                var img = vehicle.Media!.Url;
                return View(vehicle);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching vehicle data: " + ex.Message);
                return View("Error"); 
                // Handle the error, maybe redirect to an error page or show a message
            }

           
        }
        return NotFound();
    }

    public IActionResult LiveView(string data)
    {
        var liveFeed = new LiveFeed
        {
            url = "https://jarentals.net/wp-content/carLiveVids/" + data + ".mp4",
            autoPlay = true,
            loop = true
        };
        return View(liveFeed); 
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
