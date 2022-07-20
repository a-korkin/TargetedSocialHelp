using Application.Commands.Admin;
using Application.Models.Dtos.Admin;
using Application.Queries.Admin;
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
        if (userIn is null) return BadRequest();

        var userOut = await _mediator.Send(new CreateUserCommand { User = userIn });

        return CreatedAtRoute("GetUser", new { id = userOut.Id }, userOut);
    }

    [HttpGet("{id:guid}", Name = "GetUser")]
    public async Task<ActionResult<UserOutDto>> GetUserAsync([FromRoute] Guid id)
    {
        var userOut = await _mediator.Send(new GetUserQuery { Id = id });

        if (userOut is null) return BadRequest();

        return Ok(userOut);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserOutDto>>> GetUsersAsync()
    {
        var users = await _mediator.Send(new GetUsersQuery());
        return Ok(users);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserOutDto>> UpdateUserAsync(
        [FromRoute] Guid id,
        [FromBody] UserInDto userIn)
    {
        var user = await _mediator.Send(new UpdateUserCommand { Id = id, UserIn = userIn });
        return Ok(user);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new DeleteUserCommand { UserId = id });
        
        if (!result) return NotFound();

        return NoContent();
    }
}