using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApplicationLogic.Catalog.Roles;
using WebApplicationLogic.Catalog.Roles.Dto;
using WebApplicationLogic.Catalog.Users;
using WebApplicationLogic.Catalog.Users.Dto;

namespace WebApplication.Controllers.Admin
{

    [Authorize(Roles = "ADMIN")]
    public class UserAdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IRoleService _roleService;

        public UserAdminController(IUserService userService, IConfiguration configuration, IRoleService roleService)
        {
            _userService = userService;
            _configuration = configuration;
            _roleService = roleService;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            var request = new GetUserPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _userService.GetUsersPaging(request);
            ViewBag.Keyword = keyword;
           
            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userService.Register(request);
            if (result)
            {
                //TempData["result"] = "Thêm mới người dùng thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Tạo mới không thành công");
            return View(request);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            return View(new UserDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userService.Delete(request.Id);
            if (result)
            {
                //TempData["result"] = "Xóa người dùng thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "không thành công");
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> RoleAssign(string id)
        {
            var roleAssignRequest = await GetRoleAssignRequest(id);
            return View(roleAssignRequest);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userService.RoleAssign(request.Id, request);

            if (result)
            {
               // TempData["result"] = "Cập nhật quyền thành công";
                return RedirectToAction("Index");
            }

            //ModelState.AddModelError("", result.Message);
            var roleAssignRequest = await GetRoleAssignRequest(request.Id);

            return View(roleAssignRequest);
        }

        private async Task<RoleAssignRequest> GetRoleAssignRequest(string id)
        {
            var userObj = await _userService.GetById(id);
            var roleObj = await _roleService.GetAll();
            var roleAssignRequest = new RoleAssignRequest();
            foreach (var role in roleObj)
            {
                roleAssignRequest.Roles.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = userObj.Roles.Contains(role.Name)
                });
            }
            return roleAssignRequest;
        }

        [HttpGet]
        public async Task<string> PagingUserAdminAjax(string keyword, int page, string languageId)
        {
            string html = "";
            var request = new GetUserPagingRequest()
            {
                Keyword = keyword,
                PageIndex = page,
                PageSize = 5
            };
            var data = await _userService.GetUsersPaging(request);

            foreach (UserViewModel user in data.Items)
            {
                html += "<tr>"                
                      + "<td>" + user.FirstName + "</td>"
                      + "<td>" + user.LastName + "</td>"
                      + "<td>" + user.PhoneNumber + "</td>"
                      + "<td>" + user.UserName + "</td>"
                      + "<td>" + user.Email + "</td>"
                      + "<td>"
                      + "<div class='row'>"
                      + "<div class='col-md-4'>"
                      + "<form action = '/" + languageId + "/UserAdmin/Delete/" + user.Id + "'>"

                      + "<input type='submit' value='Delete' class='btn btn-danger' />"
                      + "</form>"

                      + "</div>"
                      + "|"
                      + "<div class='col-md-4'>"
                      + "<form action = '/" + languageId + "/UserAdmin/RoleAssign/" + user.Id + "' >"

                      + "<input type='submit' value='Gán quyền' class='btn btn-warning' />"
                      + "</form>"

                      + "</div>"
                     
                      + "</div>"
                      + "</td >"

                      + "</tr>";
            }

            return html;
        }


    }
}
