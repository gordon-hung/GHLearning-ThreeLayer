using GHLearning.ThreeLayer.Core.Enums;

namespace GHLearning.ThreeLayer.Core.Models;

public record UserLogAdd(
	string UserId,
	UserLogEvent Event,
	string Description,
	DateTimeOffset CreatedAt);
