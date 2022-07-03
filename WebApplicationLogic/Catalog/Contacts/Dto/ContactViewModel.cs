using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplicationLogic.Catalog.Contacts.Dto
{
    public class ContactViewModel
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }

        public string Subject { set; get; }

        public string Message { set; get; }

        public int Status { set; get; }

        public string StatusToString()
        {
            if(Status == 1) { return "Đã phản hồi"; }
            return "Chưa phản hồi";
        }
    }
}
