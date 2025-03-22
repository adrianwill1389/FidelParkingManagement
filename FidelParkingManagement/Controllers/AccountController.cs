using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        if (username == "admin" && password == "1234") //HardCore for now until database added
        {
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
