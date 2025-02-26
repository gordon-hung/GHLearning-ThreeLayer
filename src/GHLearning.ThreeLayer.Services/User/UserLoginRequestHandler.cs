using GHLearning.ThreeLayer.Core;
using MediatR;

namespace GHLearning.ThreeLayer.Services.User;

internal class UserLoginRequestHandler(
	IUserRepository userRepository,
	IPasswordHasher passwordHasher) : IRequestHandler<UserLoginRequest, UserLoginResponse>
{

	public async Task<UserLoginResponse> Handle(UserLoginRequest request, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetUserAsync(
			account: request.Account,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false)
			?? throw new Exception("User not found");

		return !passwordHasher.VerifyPassword(request.Password, user.Password)
			? throw new Exception("Password error")
			: new UserLoginResponse(
				Id: user.Id,
				Account: user.Account,
				NickName: user.NickName);
	}
}
