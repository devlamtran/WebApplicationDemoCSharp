using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplicationData.EF;
using WebApplicationData.Enties;
using WebApplicationLogic.Catalog.Roles.Dto;
using WebApplicationLogic.Dtos;

namespace WebApplicationLogic.Catalog.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly WebApplicationContext _context;
        

        public RoleService(RoleManager<Role> roleManager, WebApplicationContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<bool> Create(RoleCreateRequest request)
        {
            var roleExist = await _roleManager.RoleExistsAsync(request.Name);
           
            if (!roleExist) {
                var role = new Role()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = request.Name,
                    Description = request.Description

                };
                await _roleManager.CreateAsync(role);
            }
            return true;
            
            
        }

        public async Task<bool> Delete(RoleDeleteRequest request)
        {
            var role = await _roleManager.FindByIdAsync(request.Id);
            if (role == null)
            {
                return false;
            }
            var reult = await _roleManager.DeleteAsync(role);
            if (reult.Succeeded)
                return true;

            return false;
        }

        public async Task<List<RoleViewModel>> GetAll()
        {
            var roles = await _roleManager.Roles
                .Select(x => new RoleViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToListAsync();

            return roles;
        }

        public async Task<RoleViewModel> GetById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return  new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
        }

        public async Task<PageResult<RoleViewModel>> GetRolesPaging(GetRolePagingRequest request)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Name.Contains(request.Keyword)
                 || x.NormalizedName.Contains(request.Keyword));
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new RoleViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                   
                }).ToListAsync();

            //4. Select and projection
            var pageResult = new PageResult<RoleViewModel>()
            {
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pageResult;
        }

        public async Task<bool> Update(RoleUpdateRequest request)
        {
            var role = await _roleManager.FindByIdAsync(request.Id);
            

            if (role == null)
            {
                return false;
            }

            role.Name = request.Name;
            role.Description = request.Description;
            role.NormalizedName = request.Name;

            var reult = await _roleManager.UpdateAsync(role);
            if (reult.Succeeded)
                return true;

            return false;
        }
    }
}
