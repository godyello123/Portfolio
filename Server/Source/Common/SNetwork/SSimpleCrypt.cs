using System;
using System.Text;
using System.Security.Cryptography;

namespace SNetwork
{
	public class SXOR
	{
		public static string DefaultKey = "This is default xor crypt key!!!";
		private static byte[] s_Key = Encoding.Unicode.GetBytes(DefaultKey);
		private static int s_KeyLen = s_Key.Length;

		public static void Init(string key)
		{
			s_Key = Encoding.Unicode.GetBytes(key);
			s_KeyLen = s_Key.Length;
		}
		public static void Crypt(byte[] src, int offset, int count)
		{
			for(int i = offset; i < offset + count; i++)
			{
				src[i] ^= s_Key[i % s_KeyLen];
			}
		}
		public static string Crypt(string src)
		{
			byte[] temp = Encoding.Unicode.GetBytes(src);
			SXOR.Crypt(temp, 0, temp.Length);
			return Encoding.Unicode.GetString(temp, 0, temp.Length);
		}
	}

	public class SAES : IDisposable
	{
		private static volatile SAES s_Instance;
		private static Object s_Lock = new Object();
		public static SAES Instance
		{
			get
			{
				if(s_Instance == null)
				{
					lock(s_Lock)
					{
						if(s_Instance == null) s_Instance = new SAES();
					}
				}
				return s_Instance;
			}
		}

		public static string DefaultKey = "This is default aes crypt key!!!";

		private bool m_Disposed;

		private RijndaelManaged m_AES;
		private ICryptoTransform m_Encryptor;
		private ICryptoTransform m_Decryptor;

		public SAES()
		{
			Init(DefaultKey);
		}
		~SAES()
		{
			Dispose(false);
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if(m_Disposed) return;
			if(disposing)
			{
				Release();
			}
			m_Disposed = true;
		}

		private void Release()
		{
			if(m_AES != null)
			{
				m_AES.Clear();
				m_AES = null;
			}
			if(m_Encryptor != null)
			{
				m_Encryptor.Dispose();
				m_Encryptor = null;
			}
			if(m_Decryptor != null)
			{
				m_Decryptor.Dispose();
				m_Decryptor = null;
			}
		}
		public void Init(string key)
		{
			Release();

			m_AES = new RijndaelManaged();
			byte[] keyArray = UTF8Encoding.ASCII.GetBytes(key);
			if(keyArray.Length != 32)
			{
				byte[] temp = new byte[32];
				Array.Copy(keyArray, temp, Math.Min(keyArray.Length, 32));
				keyArray = temp;
			}
			m_AES.Key = keyArray;
			m_AES.Mode = CipherMode.ECB;
			m_AES.Padding = PaddingMode.PKCS7;
			m_Encryptor = m_AES.CreateEncryptor();
			m_Decryptor = m_AES.CreateDecryptor();
		}

		public byte[] Encrypt(byte[] src, int offset, int count)
		{
			return m_Encryptor.TransformFinalBlock(src, offset, count);
		}
		public byte[] Decrypt(byte[] src, int offset, int count)
		{
			return m_Decryptor.TransformFinalBlock(src, offset, count);
		}
		public string Encrypt(string src)
		{
			byte[] temp = Encoding.Unicode.GetBytes(src);
			temp = m_Encryptor.TransformFinalBlock(temp, 0, temp.Length);
			return Convert.ToBase64String(temp);
		}
		public string Decrypt(string src)
		{
			byte[] temp = Convert.FromBase64String(src);
			temp = m_Decryptor.TransformFinalBlock(temp, 0, temp.Length);
			return Encoding.Unicode.GetString(temp, 0, temp.Length);
		}
	}
}
