using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplicationLogic.Catalog.Categories.Dto;
using WebApplicationLogic.Catalog.Images.Dto;
using WebApplicationLogic.Catalog.Products.Dto;
using WebApplicationLogic.Dtos;

namespace WebApplicationLogic.Catalog.Products
{
    public interface IProductService
    {
        Task<int> Create(ProductCreateRequest request);

        Task<int> Update(ProductUpdateRequest request);

        Task<int> Delete(int productId);

        Task<ProductViewModel> GetById(int productId, string languageId);

        Task<bool> UpdatePrice(int productId, decimal newPrice);

        Task<bool> UpdateStock(int productId, int addedQuantity);

        Task<List<ProductViewModel>> GetAll(String languageId);


        Task AddViewcount(int productId);

        Task<PageResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);

        Task<PageResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request);

        Task<int> AddImage(int productId, ProductImageCreateRequest request);

        Task<int> RemoveImage(int imageId);

        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request);

        Task<ProductImageViewModel> GetImageById(int imageId);

        Task<List<ProductImageViewModel>> GetListImages(int productId);

        Task<PageResult<ProductViewModel>> GetAllByCategoryId( GetProductPagingRequest request);
        

        Task<bool> CategoryAssign(int id, CategoryAssignRequest request);

         Task<List<ProductViewModel>> GetFeaturedProducts(string languageId, int take);

        Task<List<ProductViewModel>> GetLatestProducts(string languageId, int take);
    }
}
