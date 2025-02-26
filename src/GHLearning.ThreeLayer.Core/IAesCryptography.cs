using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHLearning.ThreeLayer.Core;

public interface IAesCryptography
{
	string Encrypt(string plainText, string key, string iv);
	string Decrypt(string cipherText, string key, string iv);
}
