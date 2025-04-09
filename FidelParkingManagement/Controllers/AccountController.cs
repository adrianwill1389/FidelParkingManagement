using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using FidelParkingManagement.Models;
using System;

public class AccountController : Controller
{

    private readonly FidelParkingDbContext _context;

    public AccountController(FidelParkingDbContext context)
    {
        _context = context;
    }
    public IActionResult Login()
    {
        try
        {
            TempData.Remove("username");
            Console.WriteLine("TempData User Deleted");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Deleting TempData" + ex.Message);

        }
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LoginAsync(string username, string password)
    {
        
        if (username == "8854TW" && password == "1234") //HardCode for now until database added
        {
            TempData["username"] = username;

            //if user logs in successfully, load the vehicle data for the user
            //var vehicle = await _context.VehiclesDetecteds
          
            //    .FirstOrDefaultAsync(v => v.LicensePlateNumber == username);
             
            //TempData["Vehicle"] = JsonSerializer.Serialize(vehicle);
            return RedirectToAction("Index", "Dashboard"); 
        }
        ViewBag.Error = "Invalid Username or Password";
        return View();
    }


    [HttpPost]
    public IActionResult Register(string username, string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            ViewBag.Error = "Passwords do not match!";
            return View();
        }

        TempData["SuccessMessage"] = "🎉 Congratulations! Thank you for registering with Fidel Parking Management System.";

        // Save user data to the database (not implemented yet)
        return RedirectToAction("Login");
    }
     
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account"); // Redirects to Login Page
    }

}
