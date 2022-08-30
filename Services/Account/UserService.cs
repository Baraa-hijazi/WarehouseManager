using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WarehouseManager.Core.DTOs;
using WarehouseManager.Core.Entities;
using WarehouseManager.Persistence.Interfaces;
using WarehouseManager.Services.Exception;
using WarehouseManager.Services.Interfaces;

namespace WarehouseManager.Services.Account;

public class UserService : IUserService
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public UserService(SignInManager<User> signInManager,
        UserManager<User> userManager,
        IUnitOfWork unitOfWork,
        IConfiguration config,
        IMapper mapper)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _config = config;
        _mapper = mapper;
    }

    public async Task<object> Login(LoginDto loginDto)
    {
        var logged = await _signInManager
            .PasswordSignInAsync(loginDto.UserName, loginDto.Password, false, false);

        if (!logged.Succeeded) throw new System.Exception(ExceptionState.CheckUrFiled);

        return new { Token = await GenerateJsonWebTokenAsync(loginDto) };
    }

    public async Task<UserDto> RegisterUser(CreateUserDto dto)
    {
        var user = new User { UserName = dto.UserName, Email = dto.Email };
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded) throw new System.Exception(ExceptionState.CheckUrFiled);

        return _mapper.Map<UserDto>(user);
    }

    public async Task<PagedResultDto<UserDto>> GetUsers(int pageIndex, int pageSize)
    {
        return _mapper.Map<PagedResultDto<UserDto>>(await _unitOfWork.UserRepository
            .GetAllIncludedPagination(u => u.UserName != null, pageIndex, pageSize));
    }

    public async Task<UserDto?> GetUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public Task<UserDto> UpdateUser(string id, CreateUserDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> DeleteUser(string id)
    {
        throw new NotImplementedException();
    }

    private async Task<string> GenerateJsonWebTokenAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.UserName);

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:SecurityKey"])),
            SecurityAlgorithms.HmacSha256);

        var userClaims = await _userManager.GetClaimsAsync(user);
        var userRoles = await _userManager.GetRolesAsync(user);

        foreach (var role in userRoles)
        {
            userClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(type: "Username", user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.Ticks.ToString(), ClaimValueTypes.Integer64)
        }.Union(userClaims);

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}