using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet]
    public string HelloWorld()
    {
        return "hello world";
    }
}