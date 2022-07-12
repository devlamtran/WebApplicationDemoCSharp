using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplicationData.EF;
using WebApplicationData.Enties;
using WebApplicationData.Enums;
using WebApplicationLogic.Catalog.Sales.Dto;
using WebApplicationLogic.Dtos;

namespace WebApplicationLogic.Catalog.Sales
{
    public class SaleService : ISaleService
    {
        private readonly WebApplicationContext _context;
        private readonly UserManager<User> _userManager;

        public SaleService(WebApplicationContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<int> Create(CheckoutRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
       
            var orderdetails = new List<OrderDetail>();
            foreach (var item in request.OrderDetails)
            {
                //OrderDetail orderDetail = new OrderDetail() {
                //    ProductId = item.ProductId,
                //    Quantity = item.Quantity,
                //};
                //_context.OrderDetails.Add(orderDetail);

                orderdetails.Add(new OrderDetail()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
                }
            

            var order = new Order()
            {
                
                OrderDate = DateTime.Now,
                UserId = user.Id,
                ShipName = request.Name,
                ShipAddress = request.Address,
                ShipEmail = request.Email,               
                ShipPhoneNumber = request.PhoneNumber,
                Status = OrderStatus.InProgress,
                OrderDetails = orderdetails,             

            };
            

            _context.Orders.Add(order);
            
            await _context.SaveChangesAsync();
            return order.Id;

        }

        public async Task<int> Delete(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) { return -1; };

            _context.Orders.Remove(order);

            return await _context.SaveChangesAsync();
        }

        public async Task<OrderViewModel> GetById(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
           
            var  orderViewModel = new OrderViewModel()
            {
                Id = order.Id,
                ShipEmail = order.ShipName,
                ShipAddress = order.ShipAddress,
                ShipName = order.ShipName,
                ShipPhoneNumber = order.ShipPhoneNumber,
                Status = order.Status,
                OrderDate = order.OrderDate,
                UserId = order.UserId


            };
            return orderViewModel;
        }

        public async Task<PageResult<OrderDetailViewModel>> GetOrderDetailPaging(GetOrderDetailPagingRequest request)
        {
            var query = from o in _context.Orders
                        join od in _context.OrderDetails on o.Id equals od.OrderId
                        join p in _context.Products on od.ProductId equals p.Id
                        join pi in _context.ProductImages on od.ProductId equals pi.ProductId
                        join pt in _context.ProductTranslations on od.ProductId equals pt.ProductId                       
                        where o.Id == request.OrderId
                        select new { o,od,pt,p,pi };

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));

            if ( request.OrderId > 0)
            {
                query = query.Where(x => x.od.OrderId == request.OrderId);

            }
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).Select(x => new OrderDetailViewModel()
            {
                ProductId = x.od.ProductId,
                Name = x.pt.Name,
                Image =x.pi.ImagePath,
                Price = x.od.Price,
                Quantity = x.od.Quantity
                
            }).ToListAsync();

            var pageResult = new PageResult<OrderDetailViewModel>()
            {
                TotalRecord = totalRow,
                Items = data,
            };
            return pageResult;
        }

        public async Task<PageResult<OrderViewModel>> GetOrdersPaging(GetOrderPagingRequest request)
        {
           
            var query = from o in _context.Orders
                        
                        select new 
                        {
                            o
                        };

            
            //var queryGroupBy = query.GroupBy(x => new { x.o.UserId, x.o.Id, x.o.ShipName, x.o.ShipEmail, x.o.ShipAddress, x.o.OrderDate, x.o.ShipPhoneNumber, x.o.Status });
            
            if (!string.IsNullOrEmpty(request.Keyword))

            {
                DateTime? mydate = DateTime.Parse(request.Keyword);
                query = query.Where(x => x.o.OrderDate >= mydate
                 || x.o.ShipPhoneNumber.Contains(request.Keyword));


            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new OrderViewModel()
                {
                    Id = x.o.Id,
                    ShipEmail = x.o.ShipName,
                    ShipAddress = x.o.ShipAddress,
                    ShipName = x.o.ShipName,
                    ShipPhoneNumber = x.o.ShipPhoneNumber,                   
                    Status = x.o.Status,
                    OrderDate = x.o.OrderDate,
                    UserId = x.o.UserId


                }).ToListAsync();

            //4. Select and projection
            var pageResult = new PageResult<OrderViewModel>()
            {
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            
            return pageResult;
        }

        public async Task<PageResult<OrderViewModel>> GetOrdersPaging(GetOrderPagingRequest request, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var query = from o in _context.Orders
                        where o.UserId == user.Id
                        select new
                        {
                            o
                        };


            //var queryGroupBy = query.GroupBy(x => new { x.o.UserId, x.o.Id, x.o.ShipName, x.o.ShipEmail, x.o.ShipAddress, x.o.OrderDate, x.o.ShipPhoneNumber, x.o.Status });

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.o.ShipName.Contains(request.Keyword)
                 || x.o.ShipPhoneNumber.Contains(request.Keyword));


            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new OrderViewModel()
                {
                    Id = x.o.Id,
                    ShipEmail = x.o.ShipName,
                    ShipAddress = x.o.ShipAddress,
                    ShipName = x.o.ShipName,
                    ShipPhoneNumber = x.o.ShipPhoneNumber,
                    Status = x.o.Status,
                    OrderDate = x.o.OrderDate,
                    UserId = x.o.UserId


                }).ToListAsync();

            //4. Select and projection
            var pageResult = new PageResult<OrderViewModel>()
            {
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pageResult;
        }

        public async Task<int> Update(OrderUpdateRequest request)
        {
            var order = await _context.Orders.FindAsync(request.Id);
          

            if (order == null)
            {
                return -1;
            }

            order.Status = request.Status;
            

            return await _context.SaveChangesAsync();
        }
    }

   
    }

    

