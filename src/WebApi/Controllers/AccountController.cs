using Application.Auth.Commands;
using Application.Auth.Queries;
using Domain.Auth.Entities;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApi.Auth.Services;
using WebApi.Controllers.Models;
using WebApi.Dto;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class AccountController(
    IMediator mediator,
    CurrentUserAccessor currentUserAccessor) : Controller {
    [HttpGet]
    public async ValueTask<ActionResult> GetMyAccount()
    {
        var user = await mediator.Send(new GetUserQuery()
        {
            UserId = currentUserAccessor.User.Id,
        });
        return Ok(user!.Adapt<MyAccountDto>());
    }

    [HttpPatch]
    public async ValueTask<ActionResult> Patch([FromBody] JsonPatchDocument<AccountPatchRequestDto> patch)
    {
        var user = await mediator.Send(new GetUserQuery()
        {
            UserId = currentUserAccessor.User.Id,
        });
        
        var accountDto = user.Adapt<AccountPatchRequestDto>();
        var patchedAccountDto = user.Adapt<AccountPatchRequestDto>();
        
        patch.ApplyTo(patchedAccountDto);

        if (accountDto.DisplayName != patchedAccountDto.DisplayName)
            await mediator.Send(new UpdateUserDisplayNameCommand()
            {
                UserId = user.Id,
                DisplayName = patchedAccountDto.DisplayName,
            });
        
        // TODO: Update password
        // TODO: Update email

        return Ok();
    }
}