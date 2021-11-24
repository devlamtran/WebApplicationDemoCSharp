﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplicationLogic.Dtos;

namespace WebApplicationLogic.Catalog.Sales.Dto
{
    public class GetOrderDetailPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
        public int OrderId { get; set; }
    }
}
