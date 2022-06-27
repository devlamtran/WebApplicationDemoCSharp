using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;
using WebApplicationLogic.Catalog.Contacts;
using WebApplicationLogic.Catalog.Contacts.Dto;

namespace WebApplication.Controllers
{
    public class ContactController : Controller
    {
       
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
           
            _contactService = contactService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _contactService.sendContact(request);
            if (result<0)
            {
               
                ModelState.AddModelError("", "send contact failure");
                return View(request);
            }
            EmailHelper emailHelper = new EmailHelper();
            bool emailResponse = emailHelper.SendEmailContact(request);

            if (emailResponse)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(request);

        }
    }
}
