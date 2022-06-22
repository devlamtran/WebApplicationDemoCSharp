using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplicationLogic.Catalog.Roles.Dto;
using WebApplicationLogic.Dtos;

namespace WebApplicationLogic.Catalog.Roles
{
    public interface IRoleService
    {
       Task<List<RoleViewModel>> GetAll();
       Task<PageResult<RoleViewModel>> GetRolesPaging(GetRolePagingRequest request);
        Task<bool> Create(RoleCreateRequest request);
        Task<bool> Delete(RoleDeleteRequest request);
        Task<RoleViewModel> GetById(string id);
        Task<bool> Update(RoleUpdateRequest request);
    }
}
