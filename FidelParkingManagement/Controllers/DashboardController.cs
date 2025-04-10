using Azure;
using FidelParkingManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Drawing;
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

    public async Task<IActionResult> PayOnline()
    {
        try
        {
            var vehicle = await _context.VehiclesDetecteds
                                .Include(v => v.Media)
                                .Include(v => v.Payment)
                            .FirstOrDefaultAsync(v => v.LicensePlateNumber.Equals(TempData["username"]));

            TempData.Keep("username");
            if (vehicle == null)
            {
                ViewBag.Error = "Vehicle not found";
            }
            else
            //check if vehicle operation is entry
            if (vehicle.Operation == "Entry")
            {
                //calculate the duration the vehicle was in the lot
                DateTime startDateTime = vehicle!.EntryDate.ToDateTime(vehicle.EntryTime);
                DateTime endDateTime = DateTime.Now;
                TimeSpan durationTemp = endDateTime - startDateTime;
                double totalDuration = durationTemp.TotalHours;
                calculatePrice(vehicle, totalDuration, false);
            }
            else
            {
               // TempData["Amount"] = 0;
                TempData["IsPaid"] = true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching vehicle data: " + ex.Message);
            ViewBag.Error = "Error fetching vehicle data";
        }


        return View();
    }

    [HttpPost]
    public async Task<IActionResult> PayOnlineAsync()
    {
        try
        {
            //TempData.Keep("Amount");
            var vehicle = await _context.VehiclesDetecteds
                   .Include(v => v.Media)
                   .Include(v => v.Payment)
               .FirstOrDefaultAsync(v => v.LicensePlateNumber.Equals(TempData["username"]));

            TempData.Keep("username");
            //calculate the duration the vehicle was in the lot
            DateTime startDateTime = vehicle!.EntryDate.ToDateTime(vehicle.EntryTime);
            DateTime endDateTime = DateTime.Now;
            TimeSpan durationTemp = endDateTime - startDateTime;
            double totalDuration = durationTemp.TotalHours;

            //calculate fees and save to database
            calculatePrice(vehicle, totalDuration, true);

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving payment data: " + ex.Message);
            ViewBag.Error = "Error saving payment";
        }

        return RedirectToAction("PayOnline", "Dashboard");
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

    private void calculatePrice(VehiclesDetected vehiclesExiting, double minutes, bool shouldSaveDB)
    {

        try
        {
            //calculate the total hours and round up to the nearest hour
            int roundedHours = (int)Math.Ceiling(minutes);
            double cost = roundedHours * 250;
            //calculate overtime charges
            if (roundedHours > 10)
            {
                cost += (roundedHours - 10) * 100;
            }

            TempData["Amount"] = (int) cost;
            if (shouldSaveDB)
            {
                //save the payment details to the database
                Payment payment = new Payment();
                payment.Paid = (decimal)cost;
                payment.TimeStamp = DateTime.Now;

                _context.Payments.Add(payment);
                _context.SaveChanges();

                //update the vehicle details with the payment id
                vehiclesExiting.PaymentId = payment.Id;
                vehiclesExiting.Operation = "Exit";
                vehiclesExiting.ExitDate = DateOnly.FromDateTime(DateTime.Now);
                vehiclesExiting.ExitTime = TimeOnly.FromDateTime(DateTime.Now);
                _context.VehiclesDetecteds.Update(vehiclesExiting);
                _context.SaveChanges();

                TempData["IsPaid"] = true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error calculating price: " + ex.Message);
            ViewBag.Error = "Error processing payment";
        }

    }


}


// ✅ Make sure DashboardViewModel only contains needed properties
public class DashboardViewModel
{
    public decimal WalletAmount { get; set; }
    public string MembershipStatus { get; set; }
}
