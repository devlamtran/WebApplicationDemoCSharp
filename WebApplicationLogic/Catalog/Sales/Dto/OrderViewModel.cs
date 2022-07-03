using System;
using System.Collections.Generic;
using WebApplicationData.Enties;
using WebApplicationData.Enums;

namespace WebApplicationLogic.Catalog.Sales.Dto
{
    public class OrderViewModel
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

        public string StatusToString()
        {
            if (Status == 0) { return "Đang xử lí"; }
            if (Status == (OrderStatus)1) { return "Đã xác nhận"; }
            if (Status == (OrderStatus)2) { return "Đang gửi đi"; }
            if (Status == (OrderStatus)3) { return "Đơn hàng đã chuyển"; }
            return "Đã hủy";
        }
    }
}
