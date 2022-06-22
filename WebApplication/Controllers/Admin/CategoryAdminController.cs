using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationLogic.Catalog.Categories;
using WebApplicationLogic.Catalog.Categories.Dto;

namespace WebApplication.Controllers.Admin
{
    public class CategoryAdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ICategoryService _categoryService;

        public CategoryAdminController(IConfiguration configuration, ICategoryService categoryService)
        {
            _configuration = configuration;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            var languageId = HttpContext.Session.GetString("DefaultLanguageId");
            var request = new GetCategoryPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = "vi"

            };
            var data = await _categoryService.GetCategoriesPaging(request);
            ViewBag.Keyword = keyword;

            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _categoryService.Create(request);
            if (result > 0)
            {
                TempData["result"] = "Thêm mới danh mục thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm danh mục thất bại");
            return View(request);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new CategoryDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CategoryDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _categoryService.Delete(request);
            if (result > 0)
            {
                // TempData["result"] = "Xóa danh mục thành công";
                return RedirectToAction("Index");
            }

            //ModelState.AddModelError("", "Xóa không thành công");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            var category = await _categoryService.GetById("vi",id);
            var editVm = new CategoryUpdateRequest()
            {
                Id = category.Id,
                Name = category.Name,
                SeoAlias = category.SeoAlias,
                SeoDescription = category.SeoDescription,
                SeoTitle = category.SeoTitle,
                LanguageId = category.LanguageId,
                

            };
            return View(editVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _categoryService.Update(request);
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
