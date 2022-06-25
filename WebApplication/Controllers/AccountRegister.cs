using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationData.Enties;
using WebApplicationLogic.Catalog.Users;
using WebApplicationLogic.Catalog.Users.Dto;

namespace WebApplication.Controllers
{
    public class AccountRegister : Controller
    {
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountRegister(IUserService userService, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _configuration = configuration;
            _userService = userService;

        }
        [HttpGet]
        public IActionResult Register()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {

                return View(registerRequest);
            }

            var result = await _userService.Register(registerRequest);
            if (!result)
            {

                return View(registerRequest);

            }


            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public IActionResult Logout()
        {
            _userService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
