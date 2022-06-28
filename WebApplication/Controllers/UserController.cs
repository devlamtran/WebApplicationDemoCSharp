using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplicationData.Enties;
using WebApplicationLogic.Catalog.Users;
using WebApplicationLogic.Catalog.Users.Dto;

namespace WebApplication.Controllers
{

    [Authorize(Roles = "USER,ADMIN")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(UserManager<User> userManager,IUserService userService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _userService = userService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            
            if (user != null)
            {
                var userNameCurrent = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                if (userNameCurrent != userName)
                {
                    return RedirectToAction("Forbidden", "Account");
                }
                var updateRequest = new UserUpdateRequest()
                {
                    Id = user.Id,
                    Dob = user.Dob,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    Image = user.ImagePath
                    
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

           
            var result = await _userService.Update(request);
            if (result)
            {
               // _userService.Logout();
                return View(request);
            }

           
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> EditKey(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                var userNameCurrent = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                if (userNameCurrent != userName)
                {
                    return RedirectToAction("Forbidden", "Account");
                }

                var updateRequest = new ResetPasswordViewModel()
                {
                    Email = user.Email,
                    

                };

                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> EditKey(ResetPasswordViewModel request)
        {
            if (!ModelState.IsValid)
                return View();


            var result = await _userService.ResetPassword(request);
            if (result)
            {
                _userService.Logout();
                return RedirectToAction("Index", "Home");
            }


            return View(request);
        }

        [HttpGet]
        public IActionResult Profile()
        {

            return View();
        }
    }
}
