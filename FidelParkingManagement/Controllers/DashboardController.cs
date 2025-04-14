
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

        return View();
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
            }

        }
        return NotFound();
    }

    public IActionResult LiveView(string data)
    {
        //create the live feed object with the url
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
        return View();
    }

    public async Task<IActionResult> ReserveParkingAsync()
    {
        var reservations = await _context.VehiclesDetecteds
    .Where(v => v.LicensePlateNumber.Equals(TempData["username"]) && v.Operation.Equals("Reservation"))
    .ToListAsync();

        TempData.Keep("username");
        return View(reservations);
    }

    [HttpPost]
    public async Task<IActionResult> Reserve(DateOnly _entryDate, TimeOnly _entryTime)
    {
        var plate = TempData["username"]?.ToString();
        TempData.Keep("username");

        var vehicle = await _context.VehiclesDetecteds
            .FirstOrDefaultAsync(v => v.LicensePlateNumber == plate);

        if (vehicle != null)
        {
            var myReservation = new VehiclesDetected
            {
                Operation = "Reservation",
                LicensePlateNumber = vehicle.LicensePlateNumber,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Color = vehicle.Color,
                EntryDate = _entryDate,
                EntryTime = _entryTime
            };


            _context.Add(myReservation);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("ReserveParking");
    }


    [HttpPost]
    public async Task<IActionResult> Delete(int ticketnumber)
    {
        var ticket = TempData["ticketNumber"];
        var reservation = await _context.VehiclesDetecteds.FindAsync(ticket);
        if (reservation != null)
        {
            _context.VehiclesDetecteds.Remove(reservation);
            await _context.SaveChangesAsync();
        }


        return RedirectToAction("ReserveParking");
    }

    public IActionResult ReportIssue()
    {
        return View();
    }

    public IActionResult MyWallet()
    {
       
        return View();
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

            TempData["Amount"] = (int)cost;
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

