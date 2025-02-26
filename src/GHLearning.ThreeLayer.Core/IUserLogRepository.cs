using GHLearning.ThreeLayer.Core.Models;

namespace GHLearning.ThreeLayer.Core;

public interface IUserLogRepository
{
	Task AddAsync(UserLogAdd source, CancellationToken cancellationToken = default);
}
