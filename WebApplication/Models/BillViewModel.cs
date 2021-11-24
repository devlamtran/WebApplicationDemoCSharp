using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationLogic.Catalog.Sales.Dto;

namespace WebApplication.Models
{
    public class BillViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; }

        public OrderViewModel orderModel { get; set; }
    }
}
