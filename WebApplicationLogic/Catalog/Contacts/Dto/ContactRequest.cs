using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplicationLogic.Catalog.Contacts.Dto
{
    public class ContactRequest
    {
        public string Name { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }

        public string Subject { set; get; }
       
        public string Message { set; get; }
    }
}
