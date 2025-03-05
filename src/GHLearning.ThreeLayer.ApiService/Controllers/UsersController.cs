using GHLearning.ThreeLayer.ApiService.ViewModels;
using GHLearning.ThreeLayer.Service.Users;
using MediatR;
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

	/// <summary>
	/// Users the update nick name asncy.
	/// </summary>
	/// <param name="mediator">The mediator.</param>
	/// <param name="account">The account.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPatch("{account}/NickName")]
	public Task UserUpdateNickNameAsncy(
		[FromServices] IMediator mediator,
		string account,
		[FromBody] UserUpdateNickNameViewModel source)
		=> mediator.Send(
			request: new UserUpdateNickNameRequest(
				account,
				source.NickName),
			cancellationToken: HttpContext.RequestAborted);
}
