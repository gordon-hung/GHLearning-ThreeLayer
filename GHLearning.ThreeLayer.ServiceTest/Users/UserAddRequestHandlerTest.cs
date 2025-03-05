using GHLearning.ThreeLayer.Core;
using GHLearning.ThreeLayer.Core.Enums;
using GHLearning.ThreeLayer.Core.Models;
using GHLearning.ThreeLayer.Service.Users;
using NSubstitute;

namespace GHLearning.ThreeLayer.ServiceTest.Users;

public class UserAddRequestHandlerTest
{
	[Fact]
	public async Task User_Added()
	{
		var fakeTimeProvider = Substitute.For<TimeProvider>();
		var fakePasswordHasher = Substitute.For<IPasswordHasher>();
		var fakeUserRepository = Substitute.For<IUserRepository>();
		var fakeUserLogRepository = Substitute.For<IUserLogRepository>();

		var request = new UserAddRequest(
			Account: "Gordon",
			Password: "1qaz2wsx",
			NickName: "Gordon Hung");

		var currentTime = DateTimeOffset.UtcNow;
		_ = fakeTimeProvider.GetUtcNow().Returns(currentTime);

		var hashedPassword = "$2a$10$6y8BGUE0MI9caEF5xH0i6un0G7Gb2lzFFRNfGzSoY50sxjIws./NO";
		_ = fakePasswordHasher.HashPassword(
			plainPassword: Arg.Is(request.Password))
			.Returns(hashedPassword);

		var userId = "4upkdyvqyg2g";
		_ = fakeUserRepository.AddAync(
			source: Arg.Is<UserAdd>(value =>
				value.Account == request.Account &&
				value.Password == hashedPassword &&
				value.Status == UserStatus.Activity &&
				value.NickName == request.NickName &&
				value.CreatedAt == currentTime),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(userId);

		var sut = new UserAddRequestHandler(
			fakeTimeProvider,
			fakePasswordHasher,
			fakeUserRepository,
			fakeUserLogRepository);

		var cancellationTokenSource = new CancellationTokenSource();
		var token = cancellationTokenSource.Token;

		await sut.Handle(request, token);

		_ = fakeUserLogRepository
			.Received()
			.AddAsync(
			source: Arg.Is<UserLogAdd>(value =>
				value.UserId == userId &&
				value.Event == UserLogEvent.Created &&
				value.Description == "創建使用者" &&
				value.CreatedAt == currentTime),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
