using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplicationData.EF;
using WebApplicationData.Enties;
using WebApplicationData.Enums;
using WebApplicationLogic.Catalog.Contacts.Dto;
using WebApplicationLogic.Dtos;

namespace WebApplicationLogic.Catalog.Contacts
{
    public class ContactService : IContactService
    {
        private readonly WebApplicationContext _context;

        public ContactService(WebApplicationContext context)
        {
            _context = context;
        }

        public async Task<int> Delete(int contactId)
        {
            var contact = await _context.Contacts.FindAsync(contactId);
            if (contact == null) { return -1; };

            _context.Contacts.Remove(contact);

            return await _context.SaveChangesAsync();
        }

        public List<ContactViewModel> GetAllContact()
        {
            var listContactViewModel = new List<ContactViewModel>();
            var contacts = _context.Contacts.OrderBy(x=>x.Id).ToList();
            foreach (var contact in contacts )
            {
                listContactViewModel.Add(new ContactViewModel() { 
                         Id = contact.Id,
                         Name = contact.Name,
                         PhoneNumber = contact.PhoneNumber,
                         Email  = contact.Email,
                         Message = contact.Message,
                         Status = (int)contact.Status
                });
            }
            return listContactViewModel;
        }

        public async Task<PageResult<ContactViewModel>> GetAllPaging(GetManageContactPagingRequest request)
        {
            var query = from c in _context.Contacts
                       
                        select c;
            //2. filter
            if (!string.IsNullOrEmpty(request.KeyWord))
                query = query.Where(x => x.Name.Contains(request.KeyWord));

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ContactViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Message = x.Message,
                    Status = (int)x.Status
                    
                }).ToListAsync();

            //4. Select and projection
            var pageResult = new PageResult<ContactViewModel>()
            {
                TotalRecord = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pageResult;
        }

        public async Task<ContactViewModel> GetById(int contactId)
        {
            var contact = await _context.Contacts.FindAsync(contactId);

            var contactViewModel = new ContactViewModel()
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email,
                Message = contact.Message,
                PhoneNumber = contact.PhoneNumber
            };
            return contactViewModel;
        }

        public async Task<int> Process(ContactProcessRequest request)
        {
            var contact = await _context.Contacts.FindAsync(request.Id);
            if (contact == null )
            {
                return -1;
            }
            contact.Status = Status.Active;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> sendContact(ContactRequest request)
        {
            var contact = new Contact()
            {
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Message = request.Message

            };
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return contact.Id;

        }
    }
}
