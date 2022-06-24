using System;
using System.Threading.Tasks;
using WebApplicationData.Enties;
using WebApplicationLogic.Catalog.Roles.Dto;
using WebApplicationLogic.Catalog.Users.Dto;
using WebApplicationLogic.Dtos;

namespace WebApplicationLogic.Catalog.Users
{
    public interface IUserService
    {
        Task<string> Authencate(LoginRequest request);

        Task<bool> Register(RegisterRequest request);
        void Logout();

        Task<bool> Update(UserUpdateRequest request);

        //Task<User> FindByEmail(string email);

        Task<bool> ResetPassword(string email, ResetPasswordViewModel request);

        Task<string> GeneratePasswordResetToken(string email);
        Task<PageResult<UserViewModel>> GetUsersPaging(GetUserPagingRequest request);

        Task<UserViewModel> GetById(string id);

        Task<bool> Delete(string id);
        
        Task<bool> RoleAssign(String id, RoleAssignRequest request);
       
    }
}
