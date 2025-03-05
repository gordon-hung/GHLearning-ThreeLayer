namespace GHLearning.ThreeLayer.Core;

public interface IAesCryptography
{
	string Encrypt(string plainText, string key, string iv);

	string Decrypt(string cipherText, string key, string iv);
}
