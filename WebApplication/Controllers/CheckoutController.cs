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
        public CheckoutController(ISaleService saleService, IConfiguration configuration)
        {
            
            _saleService = saleService;
            _clientId = configuration["PaypalSettings:ClientId"];
            _secretKey = configuration["PaypalSettings:SecretKey"];
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

        
        private Payment payment;

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var listItems = new ItemList()
            {
                items = new List<Item>()
            };
            var session = HttpContext.Session.GetString("CartSession");
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null) { 
            currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }
            foreach (var cart in currentCart)
            {
                listItems.items.Add(new Item()
                {
                    name = cart.Name,
                    currency = "USD",
                    price = Math.Round(cart.Price, 2).ToString(),
                    quantity = cart.Quantity.ToString(),
                    sku = "sku",
                    tax = "0"
                });
            }
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };
            var details = new Details()
            {
                tax = "1",
                shipping ="2",
                subtotal = currentCart.Sum(x => x.Quantity * x.Price).ToString()
            };
            var amount = new Amount()
            {
                currency ="USD",
                total = (Convert.ToDouble(details.tax) + Convert.ToDouble(details.shipping) + Convert.ToDouble(details.subtotal)).ToString()
            };
            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "testing transaction description",
                invoice_number = Convert.ToString((new Random()).Next(100000)),
                amount = amount,
                item_list = listItems
            }) ;
            payment = new Payment()
            {
                intent = "sale",
                payer  = payer,
                transactions = transactionList ,
                redirect_urls = redirUrls
                
            };
            return payment.Create(apiContext);

        }
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId,
                
            };
            payment = new Payment()
            {
                id = paymentId
            };
            return payment.Execute(apiContext,paymentExecution);
        }

        [Authorize]
        public IActionResult PaypalCheckout()
        {
            var accessToken = new OAuthTokenCredential(_clientId, _secretKey).GetAccessToken();
            var apiContext = new APIContext(accessToken);

            try
            {
                
               
                    string baseURI = "/vi/Checkout/PaypalCheckout";
                    var createdPayment = CreatePayment(apiContext, baseURI);
                    //var response = await client.Execute(request);
                    //var statusCode = response.StatusCode;
                    ///Payment result = response.Result<Payment>();

                    //var links = result.links.GetEnumerator();
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        Console.WriteLine("Ok");
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
        [HttpGet]
        public async Task<string> PagingBillAjax(string userName, string keyword, int page, string languageId)
        {
            string html = "";
            var request = new GetOrderPagingRequest()
            {
                Keyword = keyword,
                PageIndex = page,
                PageSize = 5,

            };
            var data = await _saleService.GetOrdersPaging(request, userName);

            foreach (OrderViewModel oder in data.Items)
            {
                html += "<tr>"
                                
                      +         " <td> " + oder.Id+"</ td >"
                      +         " <td> " +oder.ShipName+" </ td >"
                      +         " <td> " + oder.ShipPhoneNumber+" </ td >"
                      +         " <td> "+ oder.ShipEmail+" </ td >"
                      +         " <td> " + oder.OrderDate +" </ td >"
                      +         " <td> " + oder.StatusToString()+" </ td >"
                      +         " <td>"
                      +         " <form action='/" + languageId+ "/Checkout/Details/" + oder.Id + "' >"

                      +         "<input type = 'submit' value = 'Xem' class='btn btn-primary' />"
                      +                      "</form>"
                      +        "</ td >"

                      +     "</tr>";
            }

            return html;
        }


    }
}
