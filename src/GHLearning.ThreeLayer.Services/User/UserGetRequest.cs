using MediatR;

namespace GHLearning.ThreeLayer.Services.User;

public record UserGetRequest(
	string Account) : IRequest<UserGetResponse?>;
