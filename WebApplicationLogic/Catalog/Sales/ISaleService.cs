using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplicationLogic.Catalog.Products.Dto;
using WebApplicationLogic.Catalog.Sales.Dto;
using WebApplicationLogic.Dtos;

namespace WebApplicationLogic.Catalog.Sales
{
    public interface ISaleService
    {
        Task<int> Create(CheckoutRequest request);

        Task<PageResult<OrderViewModel>> GetOrdersPaging(GetOrderPagingRequest request);
        Task<PageResult<OrderViewModel>> GetOrdersPaging(GetOrderPagingRequest request,string userName);
        Task<int> Delete(int orderId);

        Task<PageResult<OrderDetailViewModel>> GetOrderDetailPaging(GetOrderDetailPagingRequest request);
        Task<OrderViewModel> GetById(int id);
        Task<int> Update(OrderUpdateRequest request);
    }
}
