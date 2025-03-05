using MediatR;

namespace GHLearning.ThreeLayer.Service.Users;

public record UserUpdateNickNameRequest(
	string Account,
	string NickName) : IRequest;
