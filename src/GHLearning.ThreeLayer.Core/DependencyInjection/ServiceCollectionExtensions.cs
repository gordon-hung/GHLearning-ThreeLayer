using GHLearning.ThreeLayer.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddCore(
		this IServiceCollection services)
		=> services.AddSingleton<ISequentialGuidGenerator, SequentialGuidGenerator>()
		.AddSingleton<IPasswordHasher, PasswordHasher>()
		.AddSingleton<IUserIdGenerator, UserIdGenerator>()
		.AddSingleton<IAesCryptography, AesCBCCryptography>();
}
