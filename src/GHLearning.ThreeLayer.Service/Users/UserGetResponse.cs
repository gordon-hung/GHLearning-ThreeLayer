using GHLearning.ThreeLayer.Core.Enums;

namespace GHLearning.ThreeLayer.Service.Users;

public record UserGetResponse(
string Account,
UserStatus Status,
string NickName);
