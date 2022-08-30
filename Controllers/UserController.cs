using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManager.Core.DTOs;
using WarehouseManager.Services.Interfaces;

namespace WarehouseManager.Controllers;

[Route("API/[Controller]")]
[ApiController]
public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto) =>
        Ok(await _userService.Login(dto));
    
    [AllowAnonymous]
    [HttpPost("RegisterUser")]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto dto) =>
        Ok(await _userService.RegisterUser(dto));

    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetUsers(int pageIndex, int pageSize) =>
        Ok(await _userService.GetUsers(pageIndex, pageSize));

    [HttpGet("GetUser")]
    public async Task<IActionResult> GetUser(string id) =>
        Ok(await _userService.GetUser(id));

    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] CreateUserDto dto) =>
        Ok(await _userService.UpdateUser(id, dto));

    [HttpDelete("DeleteUser")]
    public async Task<IActionResult> DeleteUser(string id) =>
        Ok(await _userService.DeleteUser(id));
}