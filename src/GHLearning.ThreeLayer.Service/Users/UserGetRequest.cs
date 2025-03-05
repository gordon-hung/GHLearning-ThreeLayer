using MediatR;

namespace GHLearning.ThreeLayer.Service.Users;

public record UserGetRequest(
	string Account) : IRequest<UserGetResponse?>;
