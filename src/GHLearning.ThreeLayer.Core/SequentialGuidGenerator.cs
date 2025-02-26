namespace GHLearning.ThreeLayer.Core;

internal class SequentialGuidGenerator : ISequentialGuidGenerator
{
	public Task<Guid> NewIdAsync(CancellationToken cancellationToken = default) => Task.FromResult(SequentialGuid.SequentialGuidGenerator.Instance.NewGuid());
}
