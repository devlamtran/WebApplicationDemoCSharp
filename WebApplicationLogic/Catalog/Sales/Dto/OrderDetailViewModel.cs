using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplicationLogic.Catalog.Sales.Dto
{
   public class OrderDetailViewModel
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
