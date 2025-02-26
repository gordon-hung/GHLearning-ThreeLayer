using System.Security.Claims;
using GHLearning.ThreeLayer.ApiService.ViewModels;
using GHLearning.ThreeLayer.Repositories.Entities;
using GHLearning.ThreeLayer.Services.User;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace GHLearning.ThreeLayer.ApiService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> LoginAsncy(
		[FromServices] IMediator mediator,
		[FromBody] UserLoginViewModel source)
	{
		var response = await mediator.Send(
			request: new UserLoginRequest(source.Account, source.Password),
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, response.Account),
			new Claim("FullName", response.NickName)
		};

		var claimsIdentity = new ClaimsIdentity(
			claims, CookieAuthenticationDefaults.AuthenticationScheme);

		var authProperties = new AuthenticationProperties
		{
			IsPersistent = true,
			ExpiresUtc = DateTime.UtcNow.AddMinutes(10)
		};

		await HttpContext.SignInAsync(
			CookieAuthenticationDefaults.AuthenticationScheme,
			new ClaimsPrincipal(claimsIdentity),
			authProperties)
			.ConfigureAwait(false);

		return Ok();
	}
}
