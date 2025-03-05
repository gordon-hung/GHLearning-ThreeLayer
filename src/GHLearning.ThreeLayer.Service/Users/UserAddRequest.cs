using MediatR;

namespace GHLearning.ThreeLayer.Service.Users;

public record UserAddRequest(
	string Account,
	string Password,
	string NickName) : IRequest;
