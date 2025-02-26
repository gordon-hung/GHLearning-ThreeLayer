namespace GHLearning.ThreeLayer.Core;

public interface IUserIdGenerator
{
	Task<string> NewIdAsync(CancellationToken cancellationToken = default);
}
