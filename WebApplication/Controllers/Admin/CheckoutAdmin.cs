using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApplicationLogic.Catalog.Products.Dto;
using WebApplicationLogic.Catalog.Sales;
using WebApplicationLogic.Catalog.Sales.Dto;
using WebApplicationLogic.Catalog.Users;

namespace WebApplication.Controllers.Admin
{
    [Authorize(Roles = "ADMIN")]
    public class CheckoutAdmin : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ISaleService _saleService;

        public CheckoutAdmin(IUserService userService, IConfiguration configuration, ISaleService saleService)
        {
            _userService = userService;
            _configuration = configuration;
            _saleService = saleService;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            var request = new GetOrderPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _saleService.GetOrdersPaging(request);
            ViewBag.Keyword = keyword;

            return View(data);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new OrderDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(OrderDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _saleService.Delete(request.Id);
            if (result > 0)
            {
                //TempData["result"] = "Xóa hóa đơn thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "không thành công");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string keyword,int id, int pageIndex = 1, int pageSize = 6)
        {
            var request = new GetOrderDetailPagingRequest()
            {
                OrderId = id,
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                              
            };
            var result = await _saleService.GetOrderDetailPaging(request);
            return View(result);
        }
    }
}
