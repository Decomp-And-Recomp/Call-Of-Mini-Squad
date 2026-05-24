using System;
using System.IO;
using System.Text;
using UnityEngine;

public class FileUtil
{
	private static string m_savePath;

	private static string m_configPath;

	public static readonly string dataPath;

	private static bool bNeedEncrypt;

	private static string CHECK;

	private static byte[] key;

	public static string SavePath
	{
		get
		{
			return m_savePath;
		}
	}

	public static string ConfigPath
	{
		get
		{
			return m_configPath;
		}
	}

	static FileUtil()
	{
		bNeedEncrypt = true;
		CHECK = "COMA";
		key = new byte[16]
		{
			161, 233, 184, 245, 193, 162, 208, 178, 184, 245,
			193, 162, 162, 233, 184, 245
		};
		string text = Application.dataPath;
		m_configPath = text + "/../Configs";
		m_savePath = Application.persistentDataPath;
		m_configPath = text + "/Configs";
		if (!Directory.Exists(m_savePath))
		{
			CreateDirectory(m_savePath);
		}
	}

	public static string LoadResourcesFile(string filename)
	{
		TextAsset textAsset = Resources.Load(filename, typeof(TextAsset)) as TextAsset;
		if (textAsset == null)
		{
			return null;
		}
		return textAsset.text;
	}

	private static byte[] Encrypt(string content)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(content);
		if (!bNeedEncrypt)
		{
			return bytes;
		}
		for (int i = 0; i < bytes.Length; i++)
		{
			bytes[i] ^= key[i % 8];
		}
		return bytes;
	}

	private static string Decrypt(byte[] bytes)
	{
		if (!bNeedEncrypt)
		{
			return Encoding.UTF8.GetString(bytes);
		}
		for (int i = 0; i < bytes.Length; i++)
		{
			bytes[i] ^= key[i % 8];
		}
		return Encoding.UTF8.GetString(bytes);
	}

	public static void WriteFile(string fileName, string content)
	{
		string path = m_savePath + "/" + fileName + ".dat";
		byte[] bytes = Encrypt(content);
		File.WriteAllBytes(path, bytes);
	}

	public static string LoadFile(string fileName)
	{
		string text = ReadFile(fileName);
		string[] array = text.Split('&');
		if (array[0] == CHECK)
		{
			return text.Substring(array[0].Length + 1);
		}
		return string.Empty;
	}

	private static string ReadFile(string fileName)
	{
		string path = m_savePath + "/" + fileName + ".dat";
		if (File.Exists(path))
		{
			byte[] bytes = File.ReadAllBytes(path);
			return Decrypt(bytes);
		}
		return string.Empty;
	}

	public static void WriteSave(string filename, string content)
	{
		try
		{
			if (!Directory.Exists(m_savePath))
			{
				CreateDirectory(m_savePath);
			}
			string path = m_savePath + "/" + filename;
			FileStream fileStream = new FileStream(path, FileMode.Create);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.Write(content);
			streamWriter.Close();
			fileStream.Close();
		}
		catch
		{
		}
	}

	public static void WriteSave(string path, string fileName, string content)
	{
		string path2 = m_savePath + path + "/" + fileName;
		try
		{
			if (!Directory.Exists(m_savePath + path))
			{
				CreateDirectory(m_savePath + path);
			}
			FileStream fileStream = new FileStream(path2, FileMode.Create);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.Write(content);
			streamWriter.Close();
			fileStream.Close();
		}
		catch
		{
		}
	}

	public static string ReadSave(string filename)
	{
		string path = m_savePath + "/" + filename;
		if (!File.Exists(path))
		{
			return null;
		}
		try
		{
			FileStream fileStream = new FileStream(path, FileMode.Open);
			StreamReader streamReader = new StreamReader(fileStream);
			string result = streamReader.ReadToEnd();
			streamReader.Close();
			fileStream.Close();
			return result;
		}
		catch
		{
			return null;
		}
	}

	public static string ReadSave(string path, string filename)
	{
		string path2 = m_savePath + path + "/" + filename;
		if (!File.Exists(path2))
		{
			return null;
		}
		try
		{
			FileStream fileStream = new FileStream(path2, FileMode.Open);
			StreamReader streamReader = new StreamReader(fileStream);
			string result = streamReader.ReadToEnd();
			streamReader.Close();
			fileStream.Close();
			return result;
		}
		catch
		{
			return null;
		}
	}

	public static string ReadConfig(string fileName)
	{
		if (!File.Exists(m_configPath + "/" + fileName))
		{
			return null;
		}
		try
		{
			FileStream fileStream = new FileStream(m_configPath + "/" + fileName, FileMode.Open);
			StreamReader streamReader = new StreamReader(fileStream);
			string result = streamReader.ReadToEnd();
			streamReader.Close();
			fileStream.Close();
			return result;
		}
		catch
		{
			return null;
		}
	}

	public static void WriteConfig(string fileName, string content)
	{
		try
		{
			if (!Directory.Exists(m_configPath))
			{
				CreateDirectory(m_configPath);
			}
			FileStream fileStream = new FileStream(m_configPath + fileName, FileMode.Create);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.Write(content);
			streamWriter.Close();
			fileStream.Close();
		}
		catch
		{
		}
	}

	public static void WriteConfig(string path, string fileName, string content)
	{
		try
		{
			if (!Directory.Exists(path))
			{
				CreateDirectory(path);
			}
			FileStream fileStream = new FileStream(path + fileName, FileMode.Create);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.Write(content);
			streamWriter.Close();
			fileStream.Close();
		}
		catch
		{
		}
	}

	public static void CreateDirectory(string path)
	{
		string[] array = path.Split('/');
		string text = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			text += array[i];
			try
			{
				if (!Directory.Exists(text) && text != string.Empty)
				{
					Directory.CreateDirectory(text);
				}
				else
				{
					text += "/";
				}
			}
			catch (Exception)
			{
			}
		}
	}

	public static string GetProjectRelativePath(string p)
	{
		return Path.Combine(Application.dataPath, p);
	}
}
