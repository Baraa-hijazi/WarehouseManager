using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WarehouseManager.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
public class BaseController : ControllerBase
{
    [NonAction]
    protected string GetCurrentUserName()
    {
        return User.Claims.First(i => i.Type == "Username").Value;
    }
}