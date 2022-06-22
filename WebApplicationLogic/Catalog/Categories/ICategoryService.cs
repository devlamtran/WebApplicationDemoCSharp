using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApplicationLogic.Catalog.Categories.Dto;
using WebApplicationLogic.Dtos;

namespace WebApplicationLogic.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<List<CategoryViewModel>> GetAll(string languageId);

        Task<CategoryViewModel> GetById(string languageId, int id);
        Task<PageResult<CategoryViewModel>> GetCategoriesPaging(GetCategoryPagingRequest request);
        Task<int> Create(CategoryCreateRequest request);
        Task<int> Delete(CategoryDeleteRequest request);
        Task<int> Update(CategoryUpdateRequest request);
    }
}
