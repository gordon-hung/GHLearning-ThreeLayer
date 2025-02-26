namespace GHLearning.ThreeLayer.Core;

public interface ISequentialGuidGenerator
{
	Task<Guid> NewIdAsync(CancellationToken cancellationToken = default);
}
