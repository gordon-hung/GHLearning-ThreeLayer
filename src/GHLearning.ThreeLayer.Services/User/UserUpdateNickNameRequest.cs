using MediatR;

namespace GHLearning.ThreeLayer.Services.User;

public record UserUpdateNickNameRequest(
	string Account,
	string NickName) : IRequest;
