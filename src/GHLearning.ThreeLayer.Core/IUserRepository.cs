using GHLearning.ThreeLayer.Core.Models;

namespace GHLearning.ThreeLayer.Core;

public interface IUserRepository
{
	Task<UserGetInfo?> GetUserAsync(string account, CancellationToken cancellationToken = default);
	Task<string> AddAync(UserAdd source, CancellationToken cancellationToken = default);

}
