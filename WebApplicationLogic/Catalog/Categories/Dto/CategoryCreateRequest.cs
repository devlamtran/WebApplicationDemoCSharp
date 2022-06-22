using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplicationLogic.Catalog.Categories.Dto
{
    public class CategoryCreateRequest
    {
        public string Name { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }
        public string LanguageId { set; get; }
        public string SeoAlias { set; get; }
    }
}
