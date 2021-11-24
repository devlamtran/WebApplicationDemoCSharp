using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationLogic.Catalog.Users.Dto;
using WebApplicationLogic.Dtos;

namespace WebApplication.Controllers.API
{
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest request);

      

        Task<ApiResult<bool>> RegisterUser(RegisterRequest registerRequest);
    }
}
