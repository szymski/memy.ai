using Application.Credits.Queries;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Auth.Services;
using WebApi.Dto;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class CreditsController(
    IMediator mediator,
    CurrentUserAccessor currentUserAccessor) : Controller {

    [HttpGet("balance")]
    public async ValueTask<IActionResult> GetBalance()
    {
        var result = await mediator.Send(new GetUserCreditBalanceQuery()
        {
            UserId = currentUserAccessor.User.Id,
        });
        return Ok(result);
    }

    [HttpGet("events")]
    public async ValueTask<IActionResult> GetEvents()
    {
        var result = await mediator.Send(new GetUserCreditEventsQuery()
        {
            UserId = currentUserAccessor.User.Id,
        });
        return Ok(result.Adapt<IEnumerable<CreditEventDto>>());
    }
}