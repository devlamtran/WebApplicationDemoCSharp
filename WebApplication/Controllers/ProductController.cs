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
            ViewBag.KeySearch = keyword;

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
            ViewBag.CategoryId = id;
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
            });
        }
        [HttpGet]
        public async Task<string> SearchAjax(string keyword, int? categoryId, string languageId)
        {
            string html = "";
            var request = new GetManageProductPagingRequest()
            {
                KeyWord = keyword,
                PageIndex = 1,
                PageSize = 6,
                LanguageId = languageId,
                CategoryId = categoryId
            };
            var data = await _productService.GetAllPaging(request);

            foreach (ProductViewModel p in data.Items)
            {
                html += "<div class='col-sm-4'>"
                + "<div class='product-image-wrapper'>"
                + "<div class='single-products'>"
                   + "<div class='productinfo text-center'> "
                     + " <img src = '/" + p.ThumbnailImage + "' alt='" + p.Name + "' />"
                      + "  <h2>" + p.ToStringPrice() + "đ</h2>"
                       + " <p>" + p.Name + "</p>"
                       + " <a href = '' class='btn btn-default add-to-cart' data-id='" + p.Id + "' data-culture= 'vi'><i class='fa fa-shopping-cart'></i>Add to cart</a>"

                   + " </div>"

               + " </div>"
               + " <div class='choose'>"
                 + "   <ul class='nav nav-pills nav-justified'>"
                  + "  <li><a href = '/vi/Product/Detail/" + p.Id + "'><i class='fa fa-plus-square'></i>Detail</a></li>"
                    + "  <li><a href = '' ><i class='fa fa-plus-square'></i>Add to compare</a></li>"
                   + " </ul>"
                + "</div>"
            + "</div>"
        + "</div>";
            }

            return html;
        }
        [HttpGet]
        public async Task<string> FilterPriceBrandProductAjax(string keyword,int categoryId, decimal priceRange, string brandRange, string languageId)
        {
            string html = "";
            var request = new GetProductFilterPagingRequest()
            {
                CategoryId = categoryId,
                PriceRange = priceRange,
                BrandRange = brandRange,
                PageIndex = 1,
                PageSize = 6,
                LanguageId = languageId

            };
            var data = await _productService.GetAllFilterPaging(request);

            foreach (ProductViewModel p in data.Items)
            {
                html += "<div class='col-sm-4'>"
                + "<div class='product-image-wrapper'>"
                + "<div class='single-products'>"
                   + "<div class='productinfo text-center'> "
                     + " <img src = '/" + p.ThumbnailImage + "' alt='" + p.Name + "' />"
                      + "  <h2>" + p.ToStringPrice() + "đ</h2>"
                       + " <p>" + p.Name + "</p>"
                       + " <a href = '' class='btn btn-default add-to-cart' data-id='" + p.Id + "' data-culture= 'vi'><i class='fa fa-shopping-cart'></i>Add to cart</a>"

                   + " </div>"

               + " </div>"
               + " <div class='choose'>"
                 + "   <ul class='nav nav-pills nav-justified'>"
                  + "  <li><a href = '/vi/Product/Detail/" + p.Id + "'><i class='fa fa-plus-square'></i>Detail</a></li>"
                    + "  <li><a href = '' ><i class='fa fa-plus-square'></i>Add to compare</a></li>"
                   + " </ul>"
                + "</div>"
            + "</div>"
        + "</div>";
            }
          
            return html;
        }
        [HttpGet]
        public async Task<string> PagingPriceBrandProductAjax(string keyword, decimal priceRange, string brandRange, int categoryId, string languageId)
        {
            string html = "";
            var request = new GetProductFilterPagingRequest()
            {
                CategoryId = categoryId,
                PriceRange = priceRange,
                BrandRange = brandRange,
                PageIndex = 1,
                PageSize = 6,
                LanguageId = languageId

            };
            var data = await _productService.GetAllFilterPaging(request);

            
            for (int i = 1; i <= data.PageCount; i++)
            {
                if (i == data.PageIndex) { html += "<li class='paging-item active' data-page='" + i + "' data-categoryId='"+ categoryId+"' ><a href=''>" + i + "</a></li>"; }
                else
                {
                    html += "<li class='paging-item' data-page='" + i + "' data-categoryId='" + categoryId + "'><a href=''>" + i + "</a></li>";
                }


            }
            
            return html;
        }

        [HttpGet]
        public async Task<string> PagingProductAjax(string keyword, decimal priceRange, string brandRange, int categoryId, int page, string languageId)
        {
            string html = "";
            var request = new GetManageProductPagingRequest()
            {
                BrandRange = brandRange,
                PriceRange = priceRange,
                CategoryId = categoryId,
                KeyWord = keyword,
                PageIndex = page,
                PageSize = 6,
                LanguageId = languageId
               
            };
            var data = await _productService.GetAllPaging(request);

            foreach (ProductViewModel p in data.Items)
            {
                html += "<div class='col-sm-4'>"
                + "<div class='product-image-wrapper'>"
                + "<div class='single-products'>"
                   + "<div class='productinfo text-center'> "
                     + " <img src = '/" + p.ThumbnailImage + "' alt='" + p.Name + "' />"
                      + "  <h2>" + p.ToStringPrice() + "đ</h2>"
                       + " <p>" + p.Name + "</p>"
                       + " <a href = '' class='btn btn-default add-to-cart' data-id='" + p.Id + "' data-culture= 'vi'><i class='fa fa-shopping-cart'></i>Add to cart</a>"

                   + " </div>"

               + " </div>"
               + " <div class='choose'>"
                 + "   <ul class='nav nav-pills nav-justified'>"
                  + "  <li><a href = '/vi/Product/Detail/" + p.Id + "'><i class='fa fa-plus-square'></i>Detail</a></li>"
                    + "  <li><a href = '' ><i class='fa fa-plus-square'></i>Add to compare</a></li>"
                   + " </ul>"
                + "</div>"
            + "</div>"
        + "</div>";
            }

            return html;
        }

    }

}
