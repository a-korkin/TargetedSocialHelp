using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
}