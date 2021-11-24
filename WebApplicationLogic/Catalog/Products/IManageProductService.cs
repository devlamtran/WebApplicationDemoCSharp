using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApplicationLogic.Catalog.Products.Dto;
using WebApplicationLogic.Dtos;

namespace WebApplicationLogic.Catalog.Products
{
    public interface IManageProductService
    {
        Task<int> Create(ProductCreateRequest request);

        Task<int> Update(ProductUpdateRequest request);

        Task<int> Delete(int productId);

        Task<ProductViewModel> GetById(int productId, String languageId);


        Task<bool> UpdatePrice(int productId, decimal newPrice);
        Task<bool> UpdateStock(int productId, int addedQuantity);


        Task<List<ProductViewModel>> GetAll(int productId);
        Task<PageResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);



    }
}
