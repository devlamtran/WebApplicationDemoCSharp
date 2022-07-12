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

        [HttpGet]
        public async Task<string> PagingOrderAdminAjax(string keyword, int page, string languageId)
        {
            string html = "";
            var request = new GetOrderPagingRequest()
            {
                Keyword = keyword,
                PageIndex = page,
                PageSize = 5
            };
            var data = await _saleService.GetOrdersPaging(request);

            foreach (OrderViewModel o in data.Items)
            {
                html += "<tr>"

                      + "<td>" + o.Id + "</td>"
                      + "<td>" + o.ShipName + "</td>"
                      + "<td>" + o.ShipPhoneNumber + "</td>"
                      + "<td>" + o.ShipEmail + "</td>"
                      + "<td>" + o.OrderDate + "</td>"
                      + "<td>" + o.Status + "</td>"
                      + "<td>"
                      + "<div class='row'>"
                      + "<div class='col-md-4'>"
                      + "<form action = '/" + languageId + "/CheckoutAdmin/Delete/" + o.Id + "'>"

                      + "<input type='submit' value='Delete' class='btn btn-danger' />"
                      + "</form>"

                      + "</div>"
                      + "|"
                      + "<div class='col-md-4'>"
                      + "<form action = '/" + languageId + "/CheckoutAdmin/Details/" + o.Id + "' >"

                      + "<input type='submit' value='Detail' class='btn btn-warning' />"
                      + "</form>"

                      + "</div>"
                      + "|"
                      + "<div class='col-md-1'>"
                      + "<form action = '/" + languageId + "/CheckoutAdmin/Edit/" + o.Id + "' >"

                      + "<input type='submit' value='Update' class='btn btn-primary' />"
                      + "</form>"
                      + "</div>"
                      + "</div>"
                      + "</td >"

                      + "</tr>";
            }

            return html;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var order = await _saleService.GetById(id);
            var editVm = new OrderUpdateRequest()
            {
                Id = order.Id,

                Status = order.Status


            };
            return View(editVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _saleService.Update(request);
            if (result > 0)
            {
                //TempData["result"] = "Cập nhật sản phẩm thành công";
                return RedirectToAction("Index");
            }

            //ModelState.AddModelError("", "Cập nhật sản phẩm thất bại");
            return View(request);
        }
    }
}
