using Application.Auth.Commands;
using Application.Auth.Queries;
using Application.Common.Interfaces;
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
    CurrentUserAccessor currentUserAccessor,
    IAppDbContext context) : Controller {
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
        
        await using var transaction = context.BeginTransaction();

        try
        {
            if (accountDto.DisplayName != patchedAccountDto.DisplayName)
                await mediator.Send(new UpdateUserDisplayNameCommand()
                {
                    UserId = user.Id,
                    DisplayName = patchedAccountDto.DisplayName,
                });

            if (accountDto.Password != patchedAccountDto.Password)
            {
                var result = await mediator.Send(new UpdateUserPasswordCommand()
                {
                    UserId = user.Id,
                    Password = patchedAccountDto.Password,
                });
                if (!result)
                    throw new("Failed to change password.");
            }

            if (accountDto.Email != patchedAccountDto.Email)
            {
                var result = await mediator.Send(new UpdateUserEmailCommand()
                {
                    UserId = user.Id,
                    Email = patchedAccountDto.Email!,
                    Transaction = transaction,
                });
                if (!result)
                    throw new("Failed to change email.");
            }
            
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return BadRequest(e.Message);
        }

        return Ok();
    }
}