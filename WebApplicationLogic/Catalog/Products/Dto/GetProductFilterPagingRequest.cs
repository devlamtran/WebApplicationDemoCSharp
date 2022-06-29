using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplicationLogic.Dtos;

namespace WebApplicationLogic.Catalog.Products.Dto
{
    public class GetProductFilterPagingRequest : PagingRequestBase
    {
        public decimal PriceRange { get; set; }
        public string LanguageId { get; set; }
        public int? CategoryId { get; set; }
        public string BrandRange { get; set; }
    }
}
