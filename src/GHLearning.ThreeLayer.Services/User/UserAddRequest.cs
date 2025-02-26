using MediatR;

namespace GHLearning.ThreeLayer.Services.User;

public record UserAddRequest(
	string Account,
	string Password,
	string NickName) : IRequest;
