using System;
using WebApplicationLogic.Dtos;

namespace WebApplicationLogic.Catalog.Products.Dto
{
    public class GetManageProductPagingRequest : PagingRequestBase
    {
        public String KeyWord { get; set; }
        public String LanguageId { get; set; }
        public int? CategoryId { get; set; }

        public decimal PriceRange { get; set; }
        public string BrandRange { get; set; }
    }
}
