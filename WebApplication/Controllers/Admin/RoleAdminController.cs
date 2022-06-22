using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebApplicationLogic.Catalog.Roles;
using Microsoft.Extensions.Configuration;
using WebApplicationLogic.Catalog.Roles.Dto;
using Microsoft.AspNetCore.Identity;
using WebApplicationData.Enties;

namespace WebApplication.Controllers.Admin
{
    
    public class RoleAdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IRoleService _roleService;
        

        public RoleAdminController(IConfiguration configuration, IRoleService roleService)
        {
            _configuration = configuration;
            _roleService = roleService;

        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            var request = new GetRolePagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _roleService.GetRolesPaging(request);
            ViewBag.Keyword = keyword;

            return View(data);
            
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _roleService.Create(request);
            if (result)
            {
                TempData["result"] = "Thêm mới role thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm role thất bại");
            return View(request);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            return View(new RoleDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RoleDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _roleService.Delete(request);
            if (result)
            {
                TempData["result"] = "Xóa role thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Xóa role thất bại");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleService.GetById(id);
            var editVm = new RoleUpdateRequest()
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description


            };
            return View(editVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _roleService.Update(request);
            if (result)
            {
                TempData["result"] = "cập nhật role thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "cập nhật role thất bại");
            return View(request);
        }
    }
}
