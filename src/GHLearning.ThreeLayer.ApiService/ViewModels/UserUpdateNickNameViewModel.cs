using System.ComponentModel.DataAnnotations;

namespace GHLearning.ThreeLayer.ApiService.ViewModels;

public record UserUpdateNickNameViewModel
{
	[Required]
	public string NickName { get; init; } = default!;
}
