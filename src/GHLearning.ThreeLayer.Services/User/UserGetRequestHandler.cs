using GHLearning.ThreeLayer.Core;
using MediatR;

namespace GHLearning.ThreeLayer.Services.User;

internal class UserGetRequestHandler(
	IUserRepository userRepository) : IRequestHandler<UserGetRequest, UserGetResponse?>
{
	public async Task<UserGetResponse?> Handle(UserGetRequest request, CancellationToken cancellationToken)
	{
		var info = await userRepository.GetUserAsync(request.Account, cancellationToken).ConfigureAwait(false);

		return info is null ? null : new UserGetResponse(info.Account, info.Status, info.NickName);
	}
}
