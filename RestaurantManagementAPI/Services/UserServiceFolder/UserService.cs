using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantManagementAPI.Entities;
using RestaurantManagementAPI.Exceptions;
using RestaurantManagementAPI.Models;
using RestaurantManagementAPI.Services.DishServiceFolder;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace RestaurantManagementAPI.Services.UserServiceFolder
{
    public class UserService : IUserService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<DishService> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public UserService(RestaurantDbContext context, IMapper mapper, ILogger<DishService> logger, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }
        public void RegisterUser(RegisterUserDto userDto)
        {
            var user = new User()
            {
                Email = userDto.Email,
                Nationality = userDto.Nationality,
                DateOfBirth = userDto.DateOfBirth,
                RoleId = userDto.RoleId
            };
            var hashedPassword = _passwordHasher.HashPassword(user, userDto.Password);

            user.PasswordHash = hashedPassword;

            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public string GenerateJwt(UserLoginDto loginDto)
        {
            var user = _context
                .Users
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Email == loginDto.Email);
            if (user is null)
                throw new BadRequestException("Invalid username or password");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new BadRequestException("Invalid username or password");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
                new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd"))
            };
            if (!string.IsNullOrEmpty(user.Nationality))
            {
                claims.Add(new Claim("Nationality", user.Nationality));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);
            var tokenhandler = new JwtSecurityTokenHandler();
            return tokenhandler.WriteToken(token);
        }
    }
}
