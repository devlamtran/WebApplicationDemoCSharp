using System;
using System.Collections.Generic;
using System.Text;
using WebApplicationLogic.Dtos;

namespace WebApplicationLogic.Catalog.Products.Dto
{
    public class GetProductPagingRequest : PagingRequestBase
    {
        public String KeyWord { get; set; }

        public  String LanguageId { get; set; }

        public int? CategoryId { get; set; }
    }
}
