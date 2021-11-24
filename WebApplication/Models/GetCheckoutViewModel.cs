using System.Collections.Generic;
using WebApplicationLogic.Catalog.Sales.Dto;

namespace WebApplication.Models
{
    public class GetCheckoutViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; }

        public CheckoutRequest CheckoutModel { get; set; }
    }
}
