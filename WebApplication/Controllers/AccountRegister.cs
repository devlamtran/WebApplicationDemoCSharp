using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;
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

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([Required] string email)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var token = await _userService.GeneratePasswordResetToken(email);
            if (token != null)
            {

                var link = Url.Action("ResetPassword", "AccountRegister", new { token, email = email }, Request.Scheme);
                // Console.WriteLine(link);
                EmailHelper emailHelper = new EmailHelper();
                bool emailResponse = emailHelper.SendEmailPasswordReset(email, link);

                if (emailResponse)
                {
                    return RedirectToAction("ForgotPasswordConfirmation");
                }

                return View();
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            if (!ModelState.IsValid)
                return View(resetPassword);



            var resetPassResult = await _userService.ResetPassword(resetPassword.Email, resetPassword);
            if (!resetPassResult)
            {

                return View(resetPassword);
            }

            return RedirectToAction("ResetPasswordConfirmation");
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
