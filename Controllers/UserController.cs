using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using bright_ideas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace bright_ideas.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        private BIdeaContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(
            BIdeaContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        [HttpGet]
        [Route("log/reg")]

        public async Task<IActionResult> Home()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            await _signInManager.SignOutAsync();
            return View("Home");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                User NewUser = new User { UserName = model.Alias, Name = model.Name, Email = model.Email };
                IdentityResult result = await _userManager.CreateAsync(NewUser, model.Password);

                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(NewUser, isPersistent: false);
                    return RedirectToAction("Index", "Idea");
                }
                List<string> passErrors = new List<string>();
                foreach( var error in result.Errors )
                {
                    passErrors.Add(error.Description);
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                ViewBag.passErrors = passErrors;
            }
            return View("Home", model);
        }


        [HttpPost]
        [Route("user/login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                User returnUser = await _userManager.FindByEmailAsync(model.LogEmail);      
                if(returnUser != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(returnUser, model.LogPassword, isPersistent: false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Idea");
                    }
                    else
                    {
                        ViewBag.PassError = "Incorrect password";
                    }

                }
                else
                {
                    ViewBag.EmailError = "That email has not been registered";
                }            
            }
            return View("Home", model);
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Home");
        }

        private Task<User> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

    }
}