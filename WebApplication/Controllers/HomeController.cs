using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Models;
using WebApplicationLogic.Catalog.Products;
using WebApplicationLogic.Catalog.Slides;

namespace WebApplication.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISlideService _slideService;
        private readonly IProductService _productService;
       


        public HomeController(ILogger<HomeController> logger, ISlideService slideService, IProductService productService)
        {
            _logger = logger;
            _slideService = slideService;
            _productService = productService;
            
        }

        
        public async Task<IActionResult> Index()
        {
            
            var culture = CultureInfo.CurrentCulture.Name;
            var slides = await _slideService.GetAll();
            var featuredProducts = await _productService.GetFeaturedProducts(culture,6);
            var latestProducts = await _productService.GetFeaturedProducts(culture, 6);
            var viewModel = new HomeViewModel();
            viewModel.Slides = slides;
            viewModel.FeaturedProducts = featuredProducts;
            viewModel.LatestProducts = latestProducts;          

            return View(viewModel);
        }

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult SetCultureCookie(string cltr, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }
    }
}
