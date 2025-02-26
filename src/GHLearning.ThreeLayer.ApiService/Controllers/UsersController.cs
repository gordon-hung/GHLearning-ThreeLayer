using GHLearning.ThreeLayer.ApiService.ViewModels;
using GHLearning.ThreeLayer.Services.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GHLearning.ThreeLayer.ApiService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
	/// <summary>
	/// Users the add asncy.
	/// </summary>
	/// <param name="mediator">The mediator.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost]
	public Task UserAddAsncy(
		[FromServices] IMediator mediator,
		[FromBody] UserAddViewModel source)
		=> mediator.Send(
			request: new UserAddRequest(source.Account, source.Password, source.NickName),
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Users the add asncy.
	/// </summary>
	/// <param name="mediator">The mediator.</param>
	/// <param name="account">The account.</param>
	/// <returns></returns>
	[HttpGet("{account}")]
	[Authorize]
	public async Task<UserGetViewModel?> UserGetAsncy(
		[FromServices] IMediator mediator,
		string account)
	{
		var response = await mediator.Send(
			request: new UserGetRequest(account),
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return response is null
			? null
			: new UserGetViewModel(response.Account, response.Status, response.NickName);
	}
}
