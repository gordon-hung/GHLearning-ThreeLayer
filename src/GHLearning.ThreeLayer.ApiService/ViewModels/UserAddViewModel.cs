using System.ComponentModel.DataAnnotations;

namespace GHLearning.ThreeLayer.ApiService.ViewModels;

public record UserAddViewModel
{
	[Required]
	public string Account { get; init; } = default!;
	[Required]
	public string Password { get; init; } = default!;
	[Required]
	public string NickName { get; init; } = default!;
}
