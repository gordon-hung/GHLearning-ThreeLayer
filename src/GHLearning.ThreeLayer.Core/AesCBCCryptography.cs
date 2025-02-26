using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GHLearning.ThreeLayer.Core;

internal class AesCBCCryptography : IAesCryptography
{
	public string Decrypt(string cipherText, string key, string iv)
	{
		using Aes aesAlg = Aes.Create();
		aesAlg.Key = Encoding.UTF8.GetBytes(key);  // 256-bit 密鑰 (32 字節)
		aesAlg.IV = Encoding.UTF8.GetBytes(iv);    // 128-bit IV (16 字節)
		aesAlg.Mode = CipherMode.CBC;              // 設定為 CBC 模式
		aesAlg.Padding = PaddingMode.PKCS7;        // 設定填充模式

		using MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText));
		using CryptoStream cs = new CryptoStream(ms, aesAlg.CreateDecryptor(), CryptoStreamMode.Read);
		using StreamReader sr = new StreamReader(cs);
		return sr.ReadToEnd();  // 返回明文
	}
	public string Encrypt(string plainText, string key, string iv)
	{
		using Aes aesAlg = Aes.Create();
		aesAlg.Key = Encoding.UTF8.GetBytes(key);  // 256-bit 密鑰 (32 字節)
		aesAlg.IV = Encoding.UTF8.GetBytes(iv);    // 128-bit IV (16 字節)
		aesAlg.Mode = CipherMode.CBC;              // 設定為 CBC 模式
		aesAlg.Padding = PaddingMode.PKCS7;        // 設定填充模式

		using MemoryStream ms = new MemoryStream();
		using (CryptoStream cs = new CryptoStream(ms, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
		{
			using StreamWriter sw = new StreamWriter(cs);
			sw.Write(plainText);
		}

		return Convert.ToBase64String(ms.ToArray()); // 返回 Base64 字串
	}
}
