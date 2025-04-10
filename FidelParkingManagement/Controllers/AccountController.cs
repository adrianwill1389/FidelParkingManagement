using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using FidelParkingManagement.Models;
using System;
using System.Drawing;
using System.Text;
using System.Security.Cryptography;

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
            TempData.Remove("success");
            TempData.Remove("Error");
        
            TempData.Remove("IsPaid");
            TempData.Remove("Amount");
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
        try
        {
            SHA256 sha = SHA256.Create();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Invalid Username or Password";
                return View();
                
            }
            
            var userName = username.Trim();

            //Convert the password to a byte array and hash it
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha.ComputeHash(bytes);
            password = Convert.ToBase64String(hash);


            var user = _context.UserAccounts.Where(u => u.UserName == userName && u.Password == password).FirstOrDefault();
           
            if (user != null )
            {
                
                if (user.Role!.Trim() == "admin")
                {
                    ViewBag.Error = "You do not have access to this system";
                    return View();
                }
                TempData["username"] = userName;
                TempData["success"] = "success";
                return RedirectToAction("MyCar", "Dashboard");

            }
            else
            {
                ViewBag.Error = "Invalid Username or Password";
            }

        }
        catch (Exception ex)
        {
            ViewBag.Error = "Invalid Username or Password: "+ex.Message;
        }
       
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
