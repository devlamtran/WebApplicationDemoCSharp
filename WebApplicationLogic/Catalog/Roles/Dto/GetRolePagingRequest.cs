using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplicationLogic.Dtos;

namespace WebApplicationLogic.Catalog.Roles.Dto
{
    public class GetRolePagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
