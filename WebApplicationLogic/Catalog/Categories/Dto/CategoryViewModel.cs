using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplicationLogic.Catalog.Categories.Dto
{
    public  class CategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }
    }
}
