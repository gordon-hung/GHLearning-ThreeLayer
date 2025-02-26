using GHLearning.ThreeLayer.Core.Enums;

namespace GHLearning.ThreeLayer.Services.User;

public record UserGetResponse(
string Account,
UserStatus Status,
string NickName);
