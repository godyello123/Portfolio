using System;
using System.IO;
using System.Text;

namespace SCommon
{
	public delegate void TailDelegate(string text);

	public class STail
	{
		private enum FileType { None, UTF8, Unicode, ASCII, Big };
		private string m_FilePath;
		private FileType m_FileType = FileType.UTF8;
		private long m_FileSize;
		private System.Timers.Timer m_Timer = new System.Timers.Timer();
		private event TailDelegate m_TailDelegate;

		public void Start(string FilePath, TailDelegate Delegate)
		{
			m_FilePath = FilePath;
			m_TailDelegate += Delegate;

			m_Timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);
			m_Timer.Interval = 500;
			m_Timer.Start();
		}
		public void Stop()
		{
			m_Timer.Stop();
		}

		private void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
		{
			long Size = GetFileSize();
			if(Size > m_FileSize)
			{
				Read(Size - m_FileSize);
				m_FileSize = Size;
			}
		}
		private void Read(long Length)
		{
			try
			{
				if(m_FileType == FileType.None) m_FileType = GetFileType();

				using(FileStream Stream = new FileStream(m_FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					Length = Length < Stream.Length ? Length : Stream.Length;

					byte[] Buff = new byte[Length];
					Stream.Seek(-Length, SeekOrigin.End);
					Stream.Read(Buff, 0, (int)Length);

					switch(m_FileType)
					{
					case FileType.Unicode:
						if(m_TailDelegate != null) m_TailDelegate(Encoding.UTF8.GetString(Encoding.Convert(Encoding.Unicode, Encoding.UTF8, Buff)));
						break;
					case FileType.UTF8:
						if(m_TailDelegate != null) m_TailDelegate(Encoding.UTF8.GetString(Buff));
						break;
					case FileType.Big:
						if(m_TailDelegate != null) m_TailDelegate(Encoding.UTF8.GetString(Encoding.Convert(Encoding.BigEndianUnicode, Encoding.UTF8, Buff)));
						break;
					default:
						if(m_TailDelegate != null) m_TailDelegate(Encoding.UTF8.GetString(Encoding.Convert(Encoding.ASCII, Encoding.UTF8, Buff)));
						break;
					}

					m_FileSize = Stream.Length;
				}
			}
			catch
			{
				Stop();
			}
		}
		private FileType GetFileType()
		{
			try
			{
				using(FileStream Reader = new FileStream(m_FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					byte[] ReadBuffer = new byte[2];
					Reader.Read(ReadBuffer, 0, 2);
					if(ReadBuffer[0] == 255 && ReadBuffer[1] == 254) return FileType.Unicode;
					if(ReadBuffer[0] == 239 && ReadBuffer[1] == 187) return FileType.UTF8;
					if(ReadBuffer[0] == 254 && ReadBuffer[1] == 255) return FileType.Big;
					return FileType.ASCII;
				}
			}
			catch
			{
				return FileType.ASCII;
			}
		}
		private long GetFileSize()
		{
			try
			{
				using(FileStream Stream = new FileStream(m_FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					return Stream.Length;
				}
			}
			catch
			{
				return 0;
			}
		}
	}
}
