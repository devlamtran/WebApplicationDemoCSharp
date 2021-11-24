using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationLogic.Catalog.Categories.Dto;
using WebApplicationLogic.Catalog.Products.Dto;
using WebApplicationLogic.Dtos;

namespace WebApplication.Models
{
    public class ProductCategoryViewModel
    {
        public CategoryViewModel Category { get; set; }

        public PageResult<ProductViewModel> Products { get; set; }
    }
}
