using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplicationLogic.Dtos;

namespace WebApplicationLogic.Catalog.Contacts.Dto
{
    public class GetManageContactPagingRequest : PagingRequestBase
    {
        public String KeyWord { get; set; }
    }
}
