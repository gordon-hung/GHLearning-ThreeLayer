using GHLearning.ThreeLayer.Core.Enums;

namespace GHLearning.ThreeLayer.Core.Models;

public record UserAdd(
	string Account,
	string Password,
	UserStatus Status,
	string NickName,
	DateTimeOffset CreatedAt);
