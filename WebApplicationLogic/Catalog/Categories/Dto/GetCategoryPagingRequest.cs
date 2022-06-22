using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplicationLogic.Dtos;

namespace WebApplicationLogic.Catalog.Categories.Dto
{
    public class GetCategoryPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
        public String LanguageId { get; set; }
    }
}
