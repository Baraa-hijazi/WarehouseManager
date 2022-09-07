namespace WarehouseManager.Core.DTOs;

public class CreateUserDto
{
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime JoinedDate { get; set; }
    public CreateProductDto? UserDto { get; set; }
}