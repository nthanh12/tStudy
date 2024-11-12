using Microsoft.AspNetCore.Identity;
using tStudy.Application.Response;
using tStudy.Models.DTOs;

namespace tStudy.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Response<IdentityResult>> RegisterSystemUser(SystemRegisterUserDTO user);
        Task<Response<LoginResponseDTO>> LoginSystemUser(SystemSignInUserDTO credentials);

    }
}
