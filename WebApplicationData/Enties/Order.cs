using System;
using System.Collections.Generic;
using System.Text;
using WebApplicationData.Enums;

namespace WebApplicationData.Enties
{
    public class Order
    {
        public int Id { set; get; }
        public DateTime OrderDate { set; get; }
        public string UserId { set; get; }
        public string ShipName { set; get; }
        public string ShipAddress { set; get; }
        public string ShipEmail { set; get; }
        public string ShipPhoneNumber { set; get; }
        public OrderStatus Status { set; get; }

        public List<OrderDetail> OrderDetails { get; set; }

        public User User { get; set; }

    }
}
