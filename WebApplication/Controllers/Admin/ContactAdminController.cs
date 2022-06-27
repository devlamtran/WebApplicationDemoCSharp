using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;
using WebApplicationLogic.Catalog.Contacts;
using WebApplicationLogic.Catalog.Contacts.Dto;

namespace WebApplication.Controllers.Admin
{
    [Authorize(Roles = "ADMIN")]
    public class ContactAdminController : Controller
    {
        private readonly IContactService _contactService;

        public ContactAdminController(IContactService contactService)
        {
            _contactService = contactService;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 6)
        {
           

            var request = new GetManageContactPagingRequest()
            {
                KeyWord = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
               
              
            };
            var data = await _contactService.GetAllPaging(request);
            
            return View(data);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new ContactDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ContactDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _contactService.Delete(request.Id);
            if (result > 0)
            {
               
                return RedirectToAction("Index");
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Process(int id)
        {
            var contactProcess = await _contactService.GetById(id);
            return View(new ContactProcessRequest()
            {
                Id = contactProcess.Id,
                Name = contactProcess.Name,
                Email = contactProcess.Email,
                Message = contactProcess.Message
            });
        }

        [HttpPost]
        public async Task<IActionResult> Process(ContactProcessRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _contactService.Process(request);
            if (result < 0)
            {
                //TempData["result"] = "Cập nhật sản phẩm thành công";
                return View(request);
            }
            EmailHelper emailHelper = new EmailHelper();
            bool emailResponse = emailHelper.SendEmailContact(request);

            if (emailResponse)
            {
                return RedirectToAction("Index");
            }

            return View(request);


        }

    }
}
