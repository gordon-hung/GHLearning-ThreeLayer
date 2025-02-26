using System.Transactions;
using GHLearning.ThreeLayer.Core;
using GHLearning.ThreeLayer.Core.Enums;
using GHLearning.ThreeLayer.Core.Models;
using MediatR;

namespace GHLearning.ThreeLayer.Services.User;

class UserAddRequestHandler(
	TimeProvider timeProvider,
	IPasswordHasher passwordHasher,
	IUserRepository userRepository,
	IUserLogRepository userLogRepository) : IRequestHandler<UserAddRequest>
{
	public async Task Handle(UserAddRequest request, CancellationToken cancellationToken)
	{
		var currentTime = timeProvider.GetUtcNow();
		using var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled);

		var passwordHash = passwordHasher.HashPassword(request.Password);

		var userId = await userRepository.AddAync(
			source: new UserAdd(
				Account: request.Account,
				Password: passwordHash,
				Status: UserStatus.Activity,
				NickName: request.NickName,
				CreatedAt: currentTime),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		await userLogRepository.AddAsync(
			source: new UserLogAdd(
				UserId: userId,
				Event: UserLogEvent.Created,
				Description: "創建使用者",
				CreatedAt: currentTime),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		scope.Complete();
	}
}
