using System.Collections.Generic;
using WebApplicationLogic.Catalog.Sales.Dto;

namespace WebApplication.Models
{
    public class CheckoutViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; }

        public CheckoutRequest CheckoutModel { get; set; }
    }
}
