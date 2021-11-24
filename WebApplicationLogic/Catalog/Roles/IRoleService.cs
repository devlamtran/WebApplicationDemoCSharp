using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplicationLogic.Catalog.Roles.Dto;

namespace WebApplicationLogic.Catalog.Roles
{
    public interface IRoleService
    {
       Task<List<RoleViewModel>> GetAll();
    }
}
