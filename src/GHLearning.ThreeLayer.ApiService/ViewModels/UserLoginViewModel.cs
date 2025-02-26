using System.ComponentModel.DataAnnotations;

namespace GHLearning.ThreeLayer.ApiService.ViewModels;

public record UserLoginViewModel
{
	[Required]
	public string Account { get; init; } = default!;
	[Required]
	public string Password { get; init; } = default!;
}
