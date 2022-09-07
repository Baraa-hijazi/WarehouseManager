using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using WarehouseManager.Core.DTOs;
using WarehouseManager.Core.Entities;
using WarehouseManager.Persistence.Interfaces;

namespace WarehouseManager.Controllers;

[Route("API/[Controller]")]
[ApiController]
public class UserController : BaseController
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _config;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    private const string ActionName = nameof(GetValue);

    public UserController(IUnitOfWork unitOfWork,
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        IConfiguration config,
        IMapper mapper)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _config = config;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var logged = await _signInManager
            .PasswordSignInAsync(dto.UserName, dto.Password, false, false);

        if (!logged.Succeeded) return BadRequest(new { message = "Username or password is incorrect" });

        return Ok(await GenerateJsonWebTokenAsync(dto));
    }

    // [Produces("application/video-xflv")]
    // public IActionResult? GetFile()
    // {
    //     // Response.Body;
    //     // return File(null, "");
    //     return null;
    // }


    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] CreateUserDto dto,
        [FromQuery] DateTime dateFrom,
        [FromQuery] DateTime dateAt)
    {
        var user = new User
        {
            Email = dto.Email,
            UserName = dto.UserName,
            PasswordHash = dto.Password
        };

        _unitOfWork.UserRepository.Add(user);
        await _unitOfWork.CommitAsync();

        string json = @"{
  ""Date"": ""2019-08-01T00:00:00"",
  ""Temperature"": 25,
  ""Summary"": ""Hot"",
  ""DatesAvailable"": [
    ""2019-08-01T00:00:00"",
    ""2019-08-02T00:00:00""
  ],
  ""TemperatureRanges"": {
      ""Cold"": {
          ""High"": 20,
          ""Low"": -10
      },
      ""Hot"": {
          ""High"": 60,
          ""Low"": 20
      }
  }
}
";

        var obj = JObject.Parse(json);

        var createdResource = new { obj, DateTime.Now, Version = "1.0" };
        var routeValues = new { obj, DateTime.Now, version = createdResource.Version };

        return CreatedAtAction(ActionName, routeValues, createdResource);
    }

    [HttpGet("GetValues")]
    public async Task<IActionResult> GetValues(int pageIndex, int pageSize)
    {
        var users = await _unitOfWork.UserRepository.GetAllIncludedPagination(
            pageIndex: pageIndex,
            pageSize: pageSize);

        return Ok(_mapper.Map<PagedResultDto<UserDto>>(users));
    }

    [HttpGet]
    public async Task<IActionResult> GetValue(string id)
    {
        var user = await _unitOfWork.UserRepository.GetById(id);

        if (user == null) return NotFound();

        return Ok(user);
    }

    [HttpPut]
    public async Task<IActionResult> Update(string id, [FromBody] CreateUserDto dto)
    {
        var user = await _unitOfWork.UserRepository.GetById(id);

        if (user == null) return NotFound();

        _mapper.Map(dto, user);

        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.CommitAsync();

        var createdResource = new { user.Id, Version = "1.0" };
        var routeValues = new { id = createdResource.Id, version = createdResource.Version };

        return AcceptedAtAction(ActionName, routeValues, createdResource);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _unitOfWork.UserRepository.GetById(id);

        if (user == null) return NotFound();

        _unitOfWork.UserRepository.Delete(user);
        await _unitOfWork.CommitAsync();

        var createdResource = new { user.Id, Version = "1.0" };
        var routeValues = new { id = createdResource.Id, version = createdResource.Version };

        return AcceptedAtAction(ActionName, routeValues, createdResource);
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