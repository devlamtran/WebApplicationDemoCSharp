using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplicationLogic.Catalog.Contacts.Dto;
using WebApplicationLogic.Dtos;

namespace WebApplicationLogic.Catalog.Contacts
{
    public interface IContactService
    {
        Task<int> sendContact(ContactRequest request);
        Task<PageResult<ContactViewModel>> GetAllPaging(GetManageContactPagingRequest request);
        Task<int> Delete(int contactId);
        Task<ContactViewModel> GetById(int contactId);
        Task<int> Process(ContactProcessRequest request);
    }
}
