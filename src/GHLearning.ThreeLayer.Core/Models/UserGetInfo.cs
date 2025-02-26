using GHLearning.ThreeLayer.Core.Enums;

namespace GHLearning.ThreeLayer.Core.Models;

public record UserGetInfo(
	string Id,
	string Account,
	string Password,
	UserStatus Status,
	string NickName);
