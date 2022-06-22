using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication.Models;
using WebApplicationLogic.Catalog.Categories;
using WebApplicationLogic.Catalog.Products;
using WebApplicationLogic.Catalog.Products.Dto;

namespace WebApplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string keyword, int? categoryId, int page = 1, int pageSize = 6)
        {
           

            var request = new GetManageProductPagingRequest()
            {
                KeyWord = keyword,
                PageIndex = page,
                PageSize = pageSize,
                LanguageId = "vi",
                CategoryId = categoryId
            };
            var data = await _productService.GetAllPaging(request);
            //ViewBag.Keyword = keyword;

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
        public async Task<IActionResult> Detail(int id, string culture)
        {
            var product = await _productService.GetById(id, culture);
            return View(new ProductDetailViewModel()
            {
                Product = product


            });
        }
        public async Task<IActionResult> Category(int id, string culture, int page = 1)
        {

            var products = await _productService.GetAllPaging(new GetManageProductPagingRequest()
            {
                CategoryId = id,
                PageIndex = page,
                LanguageId = culture,
                PageSize = 6
            });
            return View(new ProductCategoryViewModel()
            {
                Category = await _categoryService.GetById(culture, id),
                Products = products
            }); ;
        }
        public async Task<IActionResult> CategoryPaging(int id, string culture, int page)
        {

            var products = await _productService.GetAllPaging(new GetManageProductPagingRequest()
            {
                CategoryId = id,
                PageIndex = page,
                LanguageId = culture,
                PageSize = 6
            });
            return View(new ProductCategoryViewModel()
            {
                Category = await _categoryService.GetById(culture, id),
                Products = products
            }); ;
        }
    }
       
}
