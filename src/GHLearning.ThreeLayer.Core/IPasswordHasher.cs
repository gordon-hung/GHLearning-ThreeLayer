namespace GHLearning.ThreeLayer.Core;

public interface IPasswordHasher
{
	string HashPassword(string plainPassword);

	bool VerifyPassword(string plainPassword, string hashedPassword);
}
