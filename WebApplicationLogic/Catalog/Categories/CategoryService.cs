using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApplicationData.EF;
using WebApplicationLogic.Catalog.Categories.Dto;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplicationLogic.Dtos;
using Microsoft.EntityFrameworkCore.Internal;
using WebApplicationData.Enties;

namespace WebApplicationLogic.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly WebApplicationContext _context;

        public CategoryService(WebApplicationContext context)
        {
            _context = context;
        }

        public async Task<int> Create(CategoryCreateRequest request)
        {
            var languages = _context.Languages;
            var translations = new List<CategoryTranslation>();
            foreach (var language in languages)
            {
                if (language.Id == request.LanguageId)
                {
                    translations.Add(new CategoryTranslation()
                    {
                        Name = request.Name,
                        SeoDescription = request.SeoDescription,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId,
                        SeoAlias = request.SeoAlias

                    });
                }
                else
                {
                    translations.Add(new CategoryTranslation()
                    {
                        Name = "N/A",
                        SeoAlias = "N/A",
                        LanguageId = language.Id
                       

                    });
                }
            }
            var category = new Category()
            {
                SortOrder = 5,
                IsShowOnHome = true,
                ParentId = 0,
                Status = 0,
                CategoryTranslations = translations,

            };
            
          
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.Id;
        }

        public async Task<int> Delete(CategoryDeleteRequest request)
        {
            var category = await _context.Categories.FindAsync(request.Id);
            if (category == null) { return -1; };

            var categoryTranslations = _context.CategoryTranslations.Where(i => i.CategoryId == request.Id);
            var productCategories = _context.ProductInCategories.Where(i => i.CategoryId == request.Id);
            foreach (var translateCategory in categoryTranslations)
            {
                _context.CategoryTranslations.Remove(translateCategory);
            }
            foreach (var productCategory in productCategories)
            {
                _context.ProductInCategories.Remove(productCategory);
            }

            _context.Categories.Remove(category);

            return await _context.SaveChangesAsync();
        }

        public async Task<List<CategoryViewModel>> GetAll(string languageId)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageId
                        select new { c, ct };
            return await query.Select(x => new CategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                ParentId = x.c.ParentId
            }).ToListAsync();
        }

        public async Task<CategoryViewModel> GetById(string languageId, int id)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageId && c.Id == id
                        select new { c, ct };
            return await query.Select(x => new CategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                ParentId = x.c.ParentId,
                SeoAlias = x.ct.SeoAlias,
                SeoTitle = x.ct.SeoTitle,
                SeoDescription = x.ct.SeoDescription,
                LanguageId = x.ct.LanguageId
            }).FirstOrDefaultAsync();
        }

        public async Task<PageResult<CategoryViewModel>> GetCategoriesPaging(GetCategoryPagingRequest request)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId

                        where ct.LanguageId == request.LanguageId
                        select  new { c,ct };
            
            if (!string.IsNullOrEmpty(request.Keyword))
            {

                query = query.Where( x => x.ct.Name.Contains(request.Keyword));
            }
            

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CategoryViewModel()
                {
                    Id = x.c.Id,
                    Name = x.ct.Name
                    
                  

                }).ToListAsync();

            //4. Select and projection
            var pageResult = new PageResult<CategoryViewModel>()
            {
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pageResult;
        }

        public async Task<int> Update(CategoryUpdateRequest request)
        {
            var category = await _context.Categories.FindAsync(request.Id);
            var categoryTranslations = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == request.Id
            && x.LanguageId == request.LanguageId);

            if (category == null || categoryTranslations == null)
            {
                return -1;
            }

            categoryTranslations.Name = request.Name;
            categoryTranslations.SeoAlias = request.SeoAlias;
            categoryTranslations.SeoDescription = request.SeoDescription;
            categoryTranslations.SeoTitle = request.SeoTitle;
            return await _context.SaveChangesAsync();
        }
    }
}
