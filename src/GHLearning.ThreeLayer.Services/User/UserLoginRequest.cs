using MediatR;

namespace GHLearning.ThreeLayer.Services.User;

public record UserLoginRequest(
	string Account,
	string Password) : IRequest<UserLoginResponse>;
