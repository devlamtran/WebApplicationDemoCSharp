using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication.Models;
using WebApplicationLogic.Catalog.Products;
using WebApplicationLogic.Catalog.Sales.Dto;

namespace WebApplication.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;

        public CartController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            return View(GetCheckoutViewModel());
        }
        private CheckoutViewModel GetCheckoutViewModel()
        {
            var session = HttpContext.Session.GetString("CartSession");
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            var checkoutViewModel = new CheckoutViewModel()
            {
                CartItems = currentCart,
                CheckoutModel = new CheckoutRequest()
            };
            return checkoutViewModel;
        }



        public async Task<IActionResult> AddToCart(int id, string languageId)
        {
            var product = await _productService.GetById(id, languageId);

            var session = HttpContext.Session.GetString("CartSession");
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);

            int quantity = 1;
            if (currentCart.Any(x => x.ProductId == id))
            {
                quantity = currentCart.First(x => x.ProductId == id).Quantity + 1;
                currentCart.Remove(currentCart.First(x => x.ProductId == id));

            }


            var cartItem = new CartItemViewModel()
            {
                ProductId = id,
                Description = product.Description,
                Image = product.ThumbnailImage,
                Name = product.Name,
                Price = product.Price,
                Quantity = quantity
            };

            currentCart.Add(cartItem);

            HttpContext.Session.SetString("CartSession", JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }

        [HttpGet]
        public IActionResult GetListItems()
        {
            var session = HttpContext.Session.GetString("CartSession");
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            return Ok(currentCart);
        }

        public IActionResult UpdateCart(int id, int quantity)
        {
            var session = HttpContext.Session.GetString("CartSession");
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);

            foreach (var item in currentCart)
            {
                if (item.ProductId == id)
                {
                    if (quantity == 0)
                    {
                        currentCart.Remove(item);
                        break;
                    }
                    item.Quantity = quantity;
                }
            }

            HttpContext.Session.SetString("CartSession", JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }
    }
}
