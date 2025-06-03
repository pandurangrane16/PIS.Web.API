using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace PIS.Framework.Repositories
{
	public static class EncDecService
	{
		public static void LoadPrivateKeyFromPem(RSA rsa, string pem)
		{
			string keyBase64 = pem
				.Replace("-----BEGIN RSA PRIVATE KEY-----", "")
				.Replace("-----END RSA PRIVATE KEY-----", "")
				.Replace("\r", "")
				.Replace("\n", "")
				.Trim();

			byte[] keyBytes = Convert.FromBase64String(keyBase64);
			rsa.ImportRSAPrivateKey(keyBytes, out _);
		}
		public static string Decrypt(string base64EncryptedData)
		{
			
			string privateKeyPem = Keys.privateKeyPem;
			RSA privateKey = RSA.Create();
			LoadPrivateKeyFromPem(privateKey, privateKeyPem);


			byte[] encryptedBytes = Convert.FromBase64String(base64EncryptedData);
			byte[] decryptedBytes = privateKey.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
			return Encoding.UTF8.GetString(decryptedBytes);


		}
		public static string CleanBase64String(string base64)
		{
			if (string.IsNullOrWhiteSpace(base64))
				return string.Empty;


			base64 = base64.Trim();
			base64 = base64.Replace(" ", "")
						.Replace("\n", "")
						.Replace("\r", "");

			base64 = base64.Replace('-', '+').Replace('_', '/');
			int padding = base64.Length % 4;
			if (padding > 0)
			{
				base64 = base64.PadRight(base64.Length + (4 - padding), '=');
			}

			return base64;
		}
	}
}
