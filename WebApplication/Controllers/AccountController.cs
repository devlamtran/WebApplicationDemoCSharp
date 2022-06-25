using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using WebApplication.Models;
using WebApplicationData.Enties;
using WebApplicationLogic.Catalog.Users;
using WebApplicationLogic.Catalog.Users.Dto;
using Microsoft.AspNetCore.Identity;
namespace WebApplication.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(IUserService userService, SignInManager<User> signInManager,IConfiguration configuration)
        {
            _signInManager = signInManager;
            _configuration = configuration;
            _userService = userService;
        }

        [HttpGet("Account/Login")]
        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost("Account/Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _userService.Authencate(request);
            if (result == null)
            {
                ModelState.AddModelError("", "Login failure");
                return View();
            }
            var userPrincipal = ValidateToken(result);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            HttpContext.Session.SetString("Token", result);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);

            return RedirectToAction("Index", "Home");

        }


        public IActionResult Login1()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();

        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login1(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _userService.Authencate(request);
            if (result == null)
            {
                ModelState.AddModelError(" ", "tên đăng nhập hoặc mật khẩu không đúng");
                return View();
            }
            var userPrincipal = ValidateToken(result);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            HttpContext.Session.SetString("Token", result);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);

            return RedirectToAction("Index", "Home");
            
        }

        private ClaimsPrincipal ValidateToken(String jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
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

        [AllowAnonymous]
        public  IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public  async Task<IActionResult> ForgotPassword([Required]string email)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            var token = await _userService.GeneratePasswordResetToken(email);
            if (token!= null)
            {
              
                var link = Url.Action("ResetPassword", "Account", new { token, email = email }, Request.Scheme);
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

        [HttpGet]
        public IActionResult Profile()
        {

            return View();
        }

        [HttpGet("/Account/AccessDenied")]
        public IActionResult Forbidden()
        {
            return View();
        }


    }
}
