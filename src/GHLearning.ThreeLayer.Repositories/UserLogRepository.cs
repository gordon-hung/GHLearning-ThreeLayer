using GHLearning.ThreeLayer.Core;
using GHLearning.ThreeLayer.Core.Models;
using GHLearning.ThreeLayer.Repositories.Entities;

namespace GHLearning.ThreeLayer.Repositories;

internal class UserLogRepository(
	SampleContext sampleContext,
	ISequentialGuidGenerator sequentialGuidGenerator) : IUserLogRepository
{
	public async Task AddAsync(UserLogAdd source, CancellationToken cancellationToken = default)
	{
		var id = await sequentialGuidGenerator.NewIdAsync(cancellationToken).ConfigureAwait(false);
		var userLog = new UserLog
		{
			Id = id,
			UserId = source.UserId,
			Event = source.Event.ToString(),
			Description = source.Description,
			CreatedAt = source.CreatedAt.UtcDateTime
		};

		await sampleContext.UserLogs.AddAsync(userLog, cancellationToken).ConfigureAwait(false);
		await sampleContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}
}
