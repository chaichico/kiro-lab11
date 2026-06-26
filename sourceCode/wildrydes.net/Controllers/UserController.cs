using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wildrydes.net.Context;
using wildrydes.net.Models;

namespace wildrydes.net.Controllers;

public class UserController : Controller
{
    private readonly DefaultContext _db;

    public UserController(DefaultContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(UserModel userModel)
    {
        if (!string.IsNullOrEmpty(userModel.Email) && !string.IsNullOrEmpty(userModel.Password))
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == userModel.Email);
            if (user != null)
            {
                if (GetHash(userModel.Password) == user.Password)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, "user")
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    var returnUrl = Request.Query["or"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }
        }

        ViewBag.Error = "Invalid login attempt";
        return View(userModel);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Email,Password")] UserModel userModel)
    {
        if (ModelState.IsValid)
        {
            userModel.Id = Guid.NewGuid();
            userModel.Password = GetHash(userModel.Password);
            _db.Users.Add(userModel);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        return View(userModel);
    }

    public static string GetHash(string input)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        var builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
    }
}
