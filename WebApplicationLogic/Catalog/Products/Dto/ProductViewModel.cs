using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace WebApplicationLogic.Catalog.Products.Dto
{
    public class ProductViewModel
    {
        public int Id { set; get; }
        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public int Stock { set; get; }
        public int ViewCount { set; get; }
        public DateTime DateCreated { set; get; }
        public string Name { set; get; }

        public string Brand { set; get; }

        public string Description { set; get; }
        public string Details { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }

        public string SeoAlias { get; set; }
        public string LanguageId { set; get; }

        public bool? IsFeatured { get; set; }

        public string ThumbnailImage { get; set; }

        public List<string> Categories { get; set; } = new List<string>();

        public string ToStringPrice()
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            string price = Price.ToString("#,###", cul.NumberFormat);

            return price;
        }
        public string ToStringOriginalPrice()
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            string price = OriginalPrice.ToString("#,###", cul.NumberFormat);

            return price;
        }

    }
}
