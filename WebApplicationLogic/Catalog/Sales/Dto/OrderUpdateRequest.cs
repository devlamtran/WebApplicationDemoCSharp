using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplicationData.Enums;

namespace WebApplicationLogic.Catalog.Sales.Dto
{
    public class OrderUpdateRequest
    {
        public int Id { set; get; }
       
        public OrderStatus Status { set; get; }
    }
}
