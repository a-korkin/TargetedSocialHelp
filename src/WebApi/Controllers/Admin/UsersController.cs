using Application.Commands.Admin;
using Application.Models.Dtos.Admin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    public async Task<ActionResult<UserOutDto>> CreateUserAsync([FromBody] UserInDto userIn)
    {
        if (userIn is null)
            return BadRequest();

        var userOut = await _mediator.Send(new CreateUserCommand(userIn));
        return Ok(userOut);
    }
}