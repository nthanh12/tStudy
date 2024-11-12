using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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
        private readonly IConfiguration _configuration;

        public AuthService(SignInManager<SystemUser> signInManager, UserManager<SystemUser> userManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
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
        //public async Task<Response<LoginResponseDTO>> LoginSystemUser(SystemSignInUserDTO credentials)
        //{
        //    // Tìm người theo email
        //    var user = await _userManager.FindByEmailAsync(credentials.Email);
        //    if (user == null)
        //    {
        //        return new Response<LoginResponseDTO>
        //        {
        //            Success = false,
        //            Message = "Email or password is incorrect",
        //            Data = new LoginResponseDTO()
        //        };
        //    }
        //    // Kiểm tra đăng nhập
        //    var result = await _signInManager.PasswordSignInAsync(user.UserName, credentials.Password, false, true);
        //    if (!result.Succeeded)
        //    {
        //        return new Response<LoginResponseDTO> { Success = false, Message = "Email or password is incorrect", Data = new LoginResponseDTO() };

        //    }
        //    // Đăng nhập thành công trả về thông tin
        //    return new Response<LoginResponseDTO>
        //    {
        //        Success = true,
        //        Message = "Login Successful",
        //        Data = new LoginResponseDTO
        //        {
        //            Id = user.Id,
        //            Username = user.UserName,
        //            Name = user.Name,
        //            Email = user.Email,
        //            Phone = user.Phone,
        //            Address = user.Address
        //        }
        //    };
        //}
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
                return new Response<LoginResponseDTO>
                {
                    Success = false,
                    Message = "Email or password is incorrect",
                    Data = new LoginResponseDTO()
                };
            }
            // Tạo các claim
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            // Tạo jwt token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new Response<LoginResponseDTO>
            {
                Success = true,
                Message = "Login successful",
                Data = new LoginResponseDTO
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    Address = user.Address,
                    Token = tokenString // Trả về JWT token
                }
            };
        }
    }
}
