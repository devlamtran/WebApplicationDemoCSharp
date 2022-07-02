using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using WebApplication.Models;
using WebApplicationLogic.Catalog.Contacts;
using WebApplicationLogic.Catalog.Contacts.Dto;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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

        public IActionResult ExportContentToExcel()
        {
            var listContactViewModel = _contactService.GetAllContact();
            using (var workbook = new XLWorkbook()) {
                var worksheet = workbook.Worksheets.Add("Contact");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Name";
                worksheet.Cell(currentRow, 3).Value = "Email";
                worksheet.Cell(currentRow, 4).Value = "PhoneNumber";
                worksheet.Cell(currentRow, 5).Value = "Message";
                worksheet.Cell(currentRow, 6).Value = "Status";

                foreach (var contactView in listContactViewModel)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = contactView.Id;
                    worksheet.Cell(currentRow, 2).Value = contactView.Name;
                    worksheet.Cell(currentRow, 3).Value = contactView.Email;
                    worksheet.Cell(currentRow, 4).Value = contactView.PhoneNumber;
                    worksheet.Cell(currentRow, 5).Value = contactView.Message;
                    worksheet.Cell(currentRow, 6).Value = contactView.Status;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content,"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","Contact.xlsx");
                }

            }
           
        }

        public IActionResult ExportContentToCSV()
        {
            var listContactViewModel = _contactService.GetAllContact();
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("");

            foreach (var contactView in listContactViewModel)
            {
                stringBuilder.AppendLine($"{ contactView.Id},{contactView.Name},{contactView.Email},{contactView.PhoneNumber},{contactView.Message},{contactView.Status}");
            }
            return File(Encoding.UTF8.GetBytes(stringBuilder.ToString()), "text/csv", "Contact.csv");


        }

    }
}
