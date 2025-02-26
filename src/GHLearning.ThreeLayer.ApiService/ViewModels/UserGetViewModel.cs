using GHLearning.ThreeLayer.Core.Enums;

namespace GHLearning.ThreeLayer.ApiService.ViewModels;

public record UserGetViewModel(
	string Account,
	UserStatus Status,
	string NickName);
