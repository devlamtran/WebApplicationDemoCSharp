using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PayPal;
using PayPal.Api;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using WebApplication.Models;
using WebApplicationLogic.Catalog.Sales;
using WebApplicationLogic.Catalog.Sales.Dto;
using Item = PayPal.Api.Item;
using Payer = PayPal.Api.Payer;

namespace WebApplication.Controllers
{

    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ISaleService _saleService;
        private readonly string _clientId;
        private readonly string _secretKey;
        public double TyGiaUSD = 23300;
        public CheckoutController(ISaleService saleService, IConfiguration config)
        {
            
            _saleService = saleService;
            _clientId = config["PaypalSettings:ClientId"];
            _secretKey = config["PaypalSettings:SecretKey"];
        }

        
        public IActionResult Index()
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

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel request)
        {
            var model = GetCheckoutViewModel();
            var orderDetails = new List<OrderDetailViewModel>();
            foreach (var item in model.CartItems)
            {
                orderDetails.Add(new OrderDetailViewModel()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }
            var checkoutRequest = new CheckoutRequest()
            {
                UserName = request.CheckoutModel.UserName,
                Address = request.CheckoutModel.Address,
                Name = request.CheckoutModel.Name,
                Email = request.CheckoutModel.Email,
                PhoneNumber = request.CheckoutModel.PhoneNumber,
                OrderDetails = orderDetails
            };
            //TODO: Add to API
            TempData["SuccessMsg"] = "Order puschased successful";
            var result = await _saleService.Create(checkoutRequest);
            if (result > 0)
            {
               
                return RedirectToAction("Index", "Home");
            }

           
            return View(request);

            
        }

        public async Task<IActionResult> Bill(string userName,string keyword, int pageIndex = 1, int pageSize = 5)
        {
            var request = new GetOrderPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                
            };
            var data = await _saleService.GetOrdersPaging(request,userName);
            ViewBag.Keyword = keyword;

            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string keyword, int id, int pageIndex = 1, int pageSize = 6)
        {
            var request = new GetOrderDetailPagingRequest()
            {
                OrderId = id,
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,

            };
            var result = await _saleService.GetOrderDetailPaging(request);
            decimal total = 0;
            foreach (var items in result.Items)
            {
               
                var amount = items.Price * items.Quantity;
                total += amount;
            }
            ViewBag.TotalPrice = total;
            return View(result);
        }

        [Authorize]
        public  IActionResult PaypalCheckout()
        {
            var session = HttpContext.Session.GetString("CartSession");
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            var environment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(environment);

            #region Create Paypal Order
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            var total = Math.Round(currentCart.Sum(p => p.Price * p.Quantity), 2);
            foreach (var item in currentCart)
            {
                itemList.items.Add(new Item()
                {
                    name = item.Name,
                    currency = "USD",
                    price = Math.Round(item.Price, 2).ToString(),
                    quantity = item.Quantity.ToString(),
                    sku = "sku",
                    tax = "0"
                });
            }
            #endregion

            var paypalOrderId = DateTime.Now.Ticks;
            var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var payment = new Payment()
            {
                intent = "sale",
                transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        amount = new Amount()
                        {
                            total = total.ToString(),
                            currency = "USD",
                            details = new  Details
                            {
                                tax ="0",
                                shipping = "0",
                                subtotal = total.ToString()
                            }
                        },
                        item_list = itemList,
                        description = $"Invoice #{paypalOrderId}",
                        invoice_number = paypalOrderId.ToString()
                    }
                },
                redirect_urls = new RedirectUrls()
                {
                    cancel_url = $"{hostname}/GioHang/CheckoutFail",
                    return_url = $"{hostname}/GioHang/CheckoutSuccess"
                },
                payer = new Payer()
                {
                    payment_method = "paypal"
                }
            };

            //var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(_clientId,_secretKey).GetAccessToken();
            var apiContext = new APIContext(accessToken);

            //var createdPayment = payment.Create(apiContext);
            //request.RequestBody(payment);

            try
            {
                var createdPayment = payment.Create(apiContext);
                //var response = await client.Execute(request);
                //var statusCode = response.StatusCode;
                ///Payment result = response.Result<Payment>();

                //var links = result.links.GetEnumerator();
                var links = createdPayment.links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    Links lnk = links.Current;
                    if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment  
                        paypalRedirectUrl = lnk.href;
                    }
                }
               
                return Redirect(paypalRedirectUrl);
            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

                //Process when Checkout with Paypal fails
                return Redirect("CheckoutFail");
            }
        }

    }
}
