namespace GHLearning.ThreeLayer.Core.Models;

public record UserUpdateNickName(
	string Id,
	string NickName,
	DateTimeOffset CreatedAt);
