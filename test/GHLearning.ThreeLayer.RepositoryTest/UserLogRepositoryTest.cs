using System.Data;
using GHLearning.ThreeLayer.Core;
using GHLearning.ThreeLayer.Core.Models;
using GHLearning.ThreeLayer.Repository;
using GHLearning.ThreeLayer.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NSubstitute;

namespace GHLearning.ThreeLayer.RepositoryTest;

public class UserLogRepositoryTest
{
	[Fact]
	public async Task Add_User_Does_Not_Exist()
	{
		var options = new DbContextOptionsBuilder<SampleContext>()
			.UseInMemoryDatabase(databaseName: $"dbo.{nameof(Add_User_Does_Not_Exist)}")
			.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
			.Options;
		var context = new SampleContext(options);
		_ = context.Database.EnsureDeleted();

		var fakeUserIdGenerator = Substitute.For<IUserIdGenerator>();

		var source = new UserAdd(
			Account: "Account",
			Password: "password",
			Status: Core.Enums.UserStatus.Activity,
			NickName: "nickname",
			CreatedAt: DateTimeOffset.UtcNow);

		var id = "id";
		_ = fakeUserIdGenerator.NewIdAsync()
			.Returns(id);

		var sut = new UserRepository(context, fakeUserIdGenerator);

		var actual = await sut.AddAync(source);

		var user = await context.Users
		.Where(x => x.Id == id)
		.GroupJoin(context.UserInfos, user => user.Id, userInfo => userInfo.UserId, (user, userInfos) => new { user, userInfos })
		.SelectMany(x => x.userInfos.DefaultIfEmpty(), (x, userInfo) => new { x.user, userInfo })
		.GroupJoin(context.UserStatuses, x => x.user.Id, userStatus => userStatus.UserId, (x, userStatuses) => new { x.user, x.userInfo, userStatuses })
		.SelectMany(x => x.userStatuses.DefaultIfEmpty(), (x, userStatus) => new
		{
			x.user.Id,
			x.user.Account,
			x.user.Password,
			x.user.CreatedAt,
			x.user.UpdatedAt,
			UserInfo = x.userInfo!,
			UserStatus = userStatus!
		})
		.SingleAsync();

		Assert.NotNull(actual);
		Assert.Equal(id, user.Id);
		Assert.Equal(source.Account.ToLower(), user.Account);
		Assert.Equal(source.Password, user.Password);
		Assert.Equal(source.CreatedAt, user.CreatedAt);
		Assert.Equal((byte)source.Status, user.UserStatus.Status);
		Assert.Equal(source.NickName, user.UserInfo.NickName);
	}

	[Fact]
	public async Task Add_User_Primary_Invalid_Operation_Exception()
	{
		var options = new DbContextOptionsBuilder<SampleContext>()
			.UseInMemoryDatabase(databaseName: $"dbo.{nameof(Add_User_Primary_Invalid_Operation_Exception)}")
			.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
			.Options;
		var context = new SampleContext(options);
		_ = context.Database.EnsureDeleted();

		var source = new UserAdd(
			Account: "Account2",
			Password: "password",
			Status: Core.Enums.UserStatus.Activity,
			NickName: "nickname",
			CreatedAt: DateTimeOffset.UtcNow);

		var entity = new User
		{
			Id = "id",
			Account = "account",
			Password = source.Password,
			CreatedAt = source.CreatedAt.UtcDateTime,
			UpdatedAt = source.CreatedAt.UtcDateTime,
			UserInfo = new UserInfo
			{
				UserId = "id",
				NickName = source.NickName
			},
			UserStatus = new UserStatus
			{
				UserId = "id",
				Status = (byte)source.Status
			},
			UserVipLevel = new UserVipLevel
			{
				UserId = "id",
				VipLevel = 1
			}
		};

		var cancellationTokenSource = new CancellationTokenSource();
		var token = cancellationTokenSource.Token;
		await context.Users.AddAsync(entity, token);
		await context.SaveChangesAsync(token);

		var fakeUserIdGenerator = Substitute.For<IUserIdGenerator>();

		var id = "id";
		_ = fakeUserIdGenerator.NewIdAsync()
			.Returns(id);

		var sut = new UserRepository(context, fakeUserIdGenerator);

		await Assert.ThrowsAsync<InvalidOperationException>(() => sut.AddAync(source));
	}

	[Fact]
	public async Task Add_User_Unique_Db_Update_Exception()
	{
		var options = new DbContextOptionsBuilder<SampleContext>()
			.UseInMemoryDatabase(databaseName: $"dbo.{nameof(Add_User_Unique_Db_Update_Exception)}")
			.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
			.Options;
		var context = new SampleContext(options);
		_ = context.Database.EnsureDeleted();

		var source = new UserAdd(
			Account: "Account",
			Password: "password",
			Status: Core.Enums.UserStatus.Activity,
			NickName: "nickname",
			CreatedAt: DateTimeOffset.UtcNow);

		var entity = new User
		{
			Id = "id",
			Account = source.Account.ToLower(),
			Password = source.Password,
			CreatedAt = source.CreatedAt.UtcDateTime,
			UpdatedAt = source.CreatedAt.UtcDateTime,
			UserInfo = new UserInfo
			{
				UserId = "id",
				NickName = source.NickName
			},
			UserStatus = new UserStatus
			{
				UserId = "id",
				Status = (byte)source.Status
			},
			UserVipLevel = new UserVipLevel
			{
				UserId = "id",
				VipLevel = 1
			}
		};

		var cancellationTokenSource = new CancellationTokenSource();
		var token = cancellationTokenSource.Token;
		await context.Users.AddAsync(entity, token);
		await context.SaveChangesAsync(token);

		var fakeUserIdGenerator = Substitute.For<IUserIdGenerator>();

		var id = "id2";
		_ = fakeUserIdGenerator.NewIdAsync()
			.Returns(id);

		var sut = new UserRepository(context, fakeUserIdGenerator);

		await Assert.ThrowsAsync<DbUpdateException>(() => sut.AddAync(source));
	}
}
