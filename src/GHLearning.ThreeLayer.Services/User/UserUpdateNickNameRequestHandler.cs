using System.Transactions;
using GHLearning.ThreeLayer.Core;
using GHLearning.ThreeLayer.Core.Enums;
using GHLearning.ThreeLayer.Core.Models;
using MediatR;

namespace GHLearning.ThreeLayer.Services.User;

internal class UserUpdateNickNameRequestHandler(
	TimeProvider timeProvider,
	IUserRepository userRepository,
	IUserLogRepository userLogRepository) : IRequestHandler<UserUpdateNickNameRequest>
{
	public async Task Handle(UserUpdateNickNameRequest request, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetUserAsync(request.Account, cancellationToken).ConfigureAwait(false);

		ArgumentNullException.ThrowIfNull(user, nameof(user));

		if (request.NickName == user.NickName)
		{
			return;
		}

		using var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled);
		var currentTime = timeProvider.GetUtcNow();
		await userRepository.UpdateNickNameAync(
			source: new UserUpdateNickName(
				Id: user.Id,
				NickName: request.NickName,
				CreatedAt: currentTime),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		await userLogRepository.AddAsync(
			source: new UserLogAdd(
				UserId: user.Id,
				Event: UserLogEvent.UpdateNickName,
				Description: "更新暱稱",
				CreatedAt: currentTime),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		scope.Complete();
	}
}
