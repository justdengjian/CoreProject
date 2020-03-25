using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Models;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        public UserManager<ResultUser> UserManager { get; }
        public SignInManager<ResultUser> SignInManager { get; }

        public AccountController(UserManager<ResultUser> userManager, SignInManager<ResultUser> signInManager, ILogger<AccountController> logger)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

            if(result.Succeeded)
            {
                _logger.LogInformation("Logged in {userName}.", model.UserName);
                return RedirectToAction("Index", "Result");
            }
            else
            {
                _logger.LogWarning("Failed to log in {userName}", model.UserName);
                ModelState.AddModelError("", "用户名或密码错误");
                return View(model);
            }
            
        }

        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ResultUser { UserName = model.UserName,Email=model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User{userName} was created.", model.UserName);
                    return RedirectToAction("Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);



        }
    }
}