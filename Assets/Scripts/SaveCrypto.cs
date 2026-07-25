using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class SaveCrypto
{
	private const string AppSalt = "CoMSquad;save^v1;K9#zL2mQ";

	private static byte[] DeriveKey()
	{
		string deviceId = SystemInfo.deviceUniqueIdentifier;
		using (SHA256 sha256 = SHA256.Create())
		{
			return sha256.ComputeHash(Encoding.UTF8.GetBytes(deviceId + AppSalt));
		}
	}

	public static byte[] Encrypt(string plaintext)
	{
		byte[] key = DeriveKey();
		using (Aes aes = Aes.Create())
		{
			aes.Key = key;
			aes.GenerateIV();
			using (ICryptoTransform encryptor = aes.CreateEncryptor())
			using (MemoryStream output = new MemoryStream())
			{
				output.Write(aes.IV, 0, aes.IV.Length);
				using (CryptoStream cryptoStream = new CryptoStream(output, encryptor, CryptoStreamMode.Write))
				{
					byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);
					cryptoStream.Write(plainBytes, 0, plainBytes.Length);
					cryptoStream.FlushFinalBlock();
					return output.ToArray();
				}
			}
		}
	}

	public static string Decrypt(byte[] data)
	{
		byte[] key = DeriveKey();
		using (Aes aes = Aes.Create())
		{
			aes.Key = key;
			int ivLength = aes.BlockSize / 8;
			if (data == null || data.Length <= ivLength)
			{
				throw new CryptographicException("Encrypted save data too short.");
			}
			byte[] iv = new byte[ivLength];
			Array.Copy(data, 0, iv, 0, ivLength);
			aes.IV = iv;
			using (ICryptoTransform decryptor = aes.CreateDecryptor())
			using (MemoryStream input = new MemoryStream(data, ivLength, data.Length - ivLength))
			using (CryptoStream cryptoStream = new CryptoStream(input, decryptor, CryptoStreamMode.Read))
			using (StreamReader reader = new StreamReader(cryptoStream, Encoding.UTF8))
			{
				return reader.ReadToEnd();
			}
		}
	}
}
