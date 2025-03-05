using GHLearning.ThreeLayer.Core;
using GHLearning.ThreeLayer.Core.Enums;
using GHLearning.ThreeLayer.Core.Models;
using GHLearning.ThreeLayer.Service.Users;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace GHLearning.ThreeLayer.ServiceTest.Users;

public class UserGetRequestHandlerTest
{
	[Fact]
	public async Task User_Information()
	{
		var fakeUserRepository = Substitute.For<IUserRepository>();

		var request = new UserGetRequest(
			Account: "Gordon");

		var userGetInfo = new UserGetInfo(
			Id: "4upkdyvqyg2g",
			Account: "gordon",
			Password: "$2a$10$6y8BGUE0MI9caEF5xH0i6un0G7Gb2lzFFRNfGzSoY50sxjIws./NO",
			Status: UserStatus.Activity,
			NickName: "Gordon Hung");
		_ = fakeUserRepository.GetUserAsync(
			account: Arg.Is(request.Account),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(userGetInfo);

		var sut = new UserGetRequestHandler(
			fakeUserRepository);

		var cancellationTokenSource = new CancellationTokenSource();
		var token = cancellationTokenSource.Token;

		var actual = await sut.Handle(request, token);

		Assert.NotNull(actual);
		Assert.Equal(userGetInfo.Account, actual.Account);
		Assert.Equal(userGetInfo.Status, actual.Status);
		Assert.Equal(userGetInfo.NickName, actual.NickName);
	}

	[Fact]
	public async Task User_Information_Not_Obtained()
	{
		var fakeUserRepository = Substitute.For<IUserRepository>();

		var request = new UserGetRequest(
			Account: "Gordon");

		_ = fakeUserRepository.GetUserAsync(
			account: Arg.Is(request.Account),
			cancellationToken: Arg.Any<CancellationToken>())
			.ReturnsNull();

		var sut = new UserGetRequestHandler(
			fakeUserRepository);

		var cancellationTokenSource = new CancellationTokenSource();
		var token = cancellationTokenSource.Token;

		var actual = await sut.Handle(request, token);

		Assert.Null(actual);
	}
}
