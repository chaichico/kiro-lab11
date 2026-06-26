using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using wildrydes.net.Models;

namespace wildrydes.net.Controllers;

public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
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
            var user = await _userManager.FindByEmailAsync(userModel.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, userModel.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
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
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = userModel.Email,
                Email = userModel.Email
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(userModel);
    }
}
