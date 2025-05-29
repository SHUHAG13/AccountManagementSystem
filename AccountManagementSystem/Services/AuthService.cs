using AccountManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniAccountApi.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AccountManagementSystem.Services
{
    public interface IAuthService
    {
        Task<ResponseModel> LoginAsync(LoginModel model);
        Task<ResponseModel> RegisterUser(UserModel model);
        Task<ResponseModel> GetUsers();
    }

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ResponseModel> GetUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "User list fetched successfully!",
                    Data = users
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel> LoginAsync(LoginModel model)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.EmailAddress == model.Email);
                if (user == null)
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "User not found!"
                    };
                }

                if (!string.Equals(user.Password, model.Password))
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Invalid credentials!"
                    };
                }

                var token = GenerateJsonWebToken(user);
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Login successful",
                    Data = token
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel> RegisterUser(UserModel model)
        {
            try
            {
                // Optional: Check if user already exists
                if (await _context.Users.AnyAsync(x => x.EmailAddress == model.EmailAddress))
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Email already registered!"
                    };
                }

                await _context.Users.AddAsync(model);
                await _context.SaveChangesAsync();

                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "User registered successfully!"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        private string GenerateJsonWebToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("DateofJoining", user.DateofJoining.ToString("yyyy-MM-dd")),
                new Claim("EmailAddress", user.EmailAddress)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
