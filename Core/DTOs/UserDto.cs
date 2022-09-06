namespace WarehouseManager.Core.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string CreatedOn { get; set; } = null!;
}