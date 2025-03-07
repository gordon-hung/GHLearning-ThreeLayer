﻿using GHLearning.ThreeLayer.Core;
using GHLearning.ThreeLayer.Core.Models;
using GHLearning.ThreeLayer.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace GHLearning.ThreeLayer.Repository;

internal class UserRepository(
	SampleContext sampleContext,
	IUserIdGenerator userIdGenerator) : IUserRepository
{
	public async Task<string> AddAync(UserAdd source, CancellationToken cancellationToken = default)
	{
		var userId = await userIdGenerator.NewIdAsync(cancellationToken).ConfigureAwait(false);
		var account = source.Account.ToLower();

		if (await sampleContext.Users.AnyAsync(user => user.Account == account).ConfigureAwait(false))
		{
			throw new DbUpdateException();
		}

		var user = new User
		{
			Id = userId,
			Account = account,
			Password = source.Password,
			CreatedAt = source.CreatedAt.UtcDateTime,
			UpdatedAt = source.CreatedAt.UtcDateTime,
			UserInfo = new UserInfo
			{
				UserId = userId,
				NickName = source.NickName
			},
			UserStatus = new UserStatus
			{
				UserId = userId,
				Status = (byte)source.Status
			},
			UserVipLevel = new UserVipLevel
			{
				UserId = userId,
				VipLevel = 1
			}
		};
		await sampleContext.Users.AddAsync(user, cancellationToken).ConfigureAwait(false);
		await sampleContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

		return userId;
	}

	public async Task<UserGetInfo?> GetUserAsync(string account, CancellationToken cancellationToken = default)
	{
		account = account.ToLower();

		var user = await sampleContext.Users
		.Where(x => x.Account == account)
		.GroupJoin(sampleContext.UserInfos, user => user.Id, userInfo => userInfo.UserId, (user, userInfos) => new { user, userInfos })
		.SelectMany(x => x.userInfos.DefaultIfEmpty(), (x, userInfo) => new { x.user, userInfo })
		.GroupJoin(sampleContext.UserStatuses, x => x.user.Id, userStatus => userStatus.UserId, (x, userStatuses) => new { x.user, x.userInfo, userStatuses })
		.SelectMany(x => x.userStatuses.DefaultIfEmpty(), (x, userStatus) => new
		{
			x.user.Id,
			x.user.Account,
			x.user.Password,
			x.user.CreatedAt,
			x.user.UpdatedAt,
			UserInfo = x.userInfo,
			UserStatus = userStatus
		})
		.FirstOrDefaultAsync(cancellationToken)
		.ConfigureAwait(false);

		return user is null
			? null
			: new UserGetInfo(
				user.Id,
				user.Account,
				user.Password,
				(Core.Enums.UserStatus?)(user.UserStatus?.Status ?? (byte)Core.Enums.UserStatus.Disable) ?? Core.Enums.UserStatus.Disable,
				user.UserInfo?.NickName ?? user.Account);
	}

	public async Task UpdateNickNameAync(UserUpdateNickName source, CancellationToken cancellationToken = default)
	{
		var user = await sampleContext.Users.SingleAsync(
			x => x.Id == source.Id,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		user.UpdatedAt = source.CreatedAt.UtcDateTime;

		sampleContext.Users.Update(user);

		var userInfo = await sampleContext.UserInfos.SingleAsync(
			x => x.UserId == source.Id,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		userInfo.NickName = source.NickName;

		sampleContext.UserInfos.Update(userInfo);
		await sampleContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}
}
