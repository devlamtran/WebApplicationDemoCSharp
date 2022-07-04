using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using WebApplicationLogic.Catalog.Categories;
using WebApplicationLogic.Catalog.Categories.Dto;
using WebApplicationLogic.Catalog.Products;
using WebApplicationLogic.Catalog.Products.Dto;
using WebApplicationLogic.Catalog.Roles.Dto;

namespace WebApplication.Controllers.Admin
{
    [Authorize(Roles = "ADMIN")]
    public class ProductAdminController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IConfiguration _configuration;


        public ProductAdminController(IProductService productService, ICategoryService categoryService, IConfiguration configuration)
        {
            _productService = productService;
            _categoryService = categoryService;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 5)
        {
            var languageId = HttpContext.Session.GetString("DefaultLanguageId");

            var request = new GetManageProductPagingRequest()
            {
                KeyWord = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = "vi",
                CategoryId = categoryId
            };
            var data = await _productService.GetAllPaging(request);
            ViewBag.Keyword = keyword;
            ViewBag.CategoryId = categoryId;
            var categories = await _categoryService.GetAll("vi");
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });

            //if (TempData["result"] != null)
            //{
            //    ViewBag.SuccessMsg = TempData["result"];
            //}
            //Console.WriteLine(data.Items[0].Name);
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _productService.Create(request);
            if (result > 0)
            {
                TempData["result"] = "Thêm mới sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm sản phẩm thất bại");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryAssign(int id)
        {
            var roleAssignRequest = await GetCategoryAssignRequest(id);
            return View(roleAssignRequest);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryAssign(CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productService.CategoryAssign(request.Id, request);

            if (result)
            {
                //TempData["result"] = "Cập nhật danh mục thành công";
                return RedirectToAction("Index");
            }

            //ModelState.AddModelError("", result.Message);
            var roleAssignRequest = await GetCategoryAssignRequest(request.Id);

            return View(roleAssignRequest);
        }

        private async Task<CategoryAssignRequest> GetCategoryAssignRequest(int id)
        {
            //var languageId = HttpContext.Session.GetString("vi");

            var productObj = await _productService.GetById(id, "vi");
            var categories = await _categoryService.GetAll("vi");
            var categoryAssignRequest = new CategoryAssignRequest();
            foreach (var category in categories)
            {
                categoryAssignRequest.Categories.Add(new SelectItem()
                {
                    Id = category.Id.ToString(),
                    Name = category.Name,
                    Selected = productObj.Categories.Contains(category.Name)
                });
            }
            return categoryAssignRequest;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            var product = await _productService.GetById(id, "vi");
            var editVm = new ProductUpdateRequest()
            {
                Id = product.Id,
                Description = product.Description,
                Details = product.Details,
                Name = product.Name,
                Price = product.Price,
                OriginalPrice = product.OriginalPrice,
                SeoAlias = product.SeoAlias,
                SeoDescription = product.SeoDescription,
                SeoTitle = product.SeoTitle,
                Brand = product.Brand,
                LanguageId = product.LanguageId,
                IsFeatured = product.IsFeatured,
              
               
            };
            return View(editVm);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _productService.Update(request);
            if (result > 0)
            {
                //TempData["result"] = "Cập nhật sản phẩm thành công";
                return RedirectToAction("Index");
            }

            //ModelState.AddModelError("", "Cập nhật sản phẩm thất bại");
            return View(request);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new ProductDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productService.Delete(request.Id);
            if (result > 0)
            {
               // TempData["result"] = "Xóa sản phẩm thành công";
                return RedirectToAction("Index");
            }

            //ModelState.AddModelError("", "Xóa không thành công");
            return View(request);
        }

        [HttpGet]
        public async Task<string> PagingProductAdminAjax(string keyword, int? categoryId, int page, string languageId)
        {
            string html = "";
            var request = new GetManageProductPagingRequest()
            {
                CategoryId = categoryId,
                KeyWord = keyword,
                PageIndex = page,
                PageSize = 5,
                LanguageId = languageId

            };
            var data = await _productService.GetAllPaging(request);

            foreach (ProductViewModel p in data.Items)
            {
                html += "<tr>"

                      + "<td>" + p.Id + "</td>"
                      + "<td><img width='80' src='/" + p.ThumbnailImage + "' alt=''></td>"
                      + "<td>" + p.Name + "</td>"
                      + "<td>" + p.ToStringOriginalPrice() + "</td>"
                      + "<td>" + p.ToStringPrice() + "</td>"
                      + "<td>" + p.ViewCount + "</td>"
                      + "<td>"
                      + "<div class='row'>"
                      +             "<div class='col-md-4'>"
                      +             "<form action = '/"+languageId+"/ProductAdmin/Delete/"+ p.Id+ "'>"

                      +             "<input type='submit' value='Delete' class='btn btn-danger' />"
                      +             "</form>"

                      +             "</div>"
                      +             "|"
                      +             "<div class='col-md-4'>"
                      +             "<form action = '/" + languageId+"/ProductAdmin/Edit/"+ p.Id+"' >"

                      +             "<input type='submit' value='Update' class='btn btn-warning' />"
                      +             "</form>"

                      +             "</div>"
                      +             "|"
                      +             "<div class='col-md-1'>"
                      +             "<form action = '/" + languageId + "/ProductAdmin/CategoryAssign/" + p.Id+"' >"

                      +              "<input type='submit' value='Assign' class='btn btn-primary' />"
                      +               "</form>"
                      +              "</div>"
                      +         "</div>"
                      + "</td >"

                      + "</tr>";
            }

            return html;
        }


    }
}
