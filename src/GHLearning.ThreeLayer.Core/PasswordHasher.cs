namespace GHLearning.ThreeLayer.Core;

internal class PasswordHasher : IPasswordHasher
{
	private const int SaltRounds = 10;

	public string HashPassword(string plainPassword)
	{
		// 生成隨機的鹽並哈希密碼
		string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword, SaltRounds);
		return hashedPassword;
	}

	public bool VerifyPassword(string plainPassword, string hashedPassword)
	{
		// 驗證密碼是否正確
		return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
	}
}
