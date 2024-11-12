using Microsoft.AspNetCore.Identity;
using tStudy.Application.Interfaces;
using tStudy.Application.Response;
using tStudy.Models.DTOs;
using tStudy.Models.Entities;

namespace tStudy.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<SystemUser> _signInManager;
        private readonly UserManager<SystemUser> _userManager;

        public AuthService(SignInManager<SystemUser> signInManager, UserManager<SystemUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<Response<IdentityResult>> RegisterSystemUser(SystemRegisterUserDTO user)
        {
            var systemUser = new SystemUser
            {
                UserName = user.Username,
                Email = user.Email,
                Name = user.Name,
                Phone = user.Phone,
                Address = user.Address
            };

            // Đăng ký người dùng với mật khẩu
            var result = await _userManager.CreateAsync(systemUser, user.Password);

            return new Response<IdentityResult>
            {
                Success = result.Succeeded,
                Message = result.Succeeded ? "User Registration Successful!" : "User Registration Failed!",
                Data = result
            };
        }
        public async Task<Response<LoginResponseDTO>> LoginSystemUser(SystemSignInUserDTO credentials)
        {
            // Tìm người theo email
            var user = await _userManager.FindByEmailAsync(credentials.Email);
            if (user == null)
            {
                return new Response<LoginResponseDTO>
                {
                    Success = false,
                    Message = "Email or password is incorrect",
                    Data = new LoginResponseDTO()
                };
            }
            // Kiểm tra đăng nhập
            var result = await _signInManager.PasswordSignInAsync(user.UserName, credentials.Password, false, true);
            if (!result.Succeeded)
            {
                return new Response<LoginResponseDTO> { Success = false, Message = "Email or password is incorrect", Data = new LoginResponseDTO() };

            }
            // Đăng nhập thành công trả về thông tin
            return new Response<LoginResponseDTO>
            {
                Success = true,
                Message = "Login Successful",
                Data = new LoginResponseDTO
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    Address = user.Address
                }
            };
        }
    }
}
