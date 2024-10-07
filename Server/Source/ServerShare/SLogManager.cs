//using System;
//using System.Collections.Generic;
//using System.Collections.Concurrent;
//using SCommon;
//using System.Threading.Tasks;
//using System.Threading;
//using System.IO;
//using System.Windows.Forms;
//using System.Diagnostics;
//using Global;

//namespace Global
//{
//    public class ParquetFileBase
//    {
//        public ParquetSharp.ParquetFileWriter m_File = null;
//        public ParquetSharp.RowGroupWriter m_RowGroupWriter = null;

//        public int m_LoggingCnt = 0;
//        public int m_LoggingCapacity = 1000000;
//        public STimer m_Timer = new STimer(10 * 60 * 1000, false);

//        public ParquetFileBase(int loggingCapacity, double tickMs)
//        {
//            m_LoggingCapacity = loggingCapacity;
//            m_Timer.SetTimer(tickMs, false);
//        }

//        ~ParquetFileBase()
//        {
//            Release();
//        }

//        public void Release()
//        {
//            if (m_File != null)
//            {
//                m_File.Close();
//                m_File = null;
//            }
//        }
//        public void Create(string fileName)
//        {
//            var cols = new ParquetSharp.Column[]
//                {
//                    new ParquetSharp.Column<int>("log_type"),
//                    new ParquetSharp.Column<string>("uid"),
//                    new ParquetSharp.Column<long>("cid"),
//                    new ParquetSharp.Column<string>("sub_str"),
//                    new ParquetSharp.Column<string>("obj_type"),
//                    new ParquetSharp.Column<long>("obj_uid"),
//                    new ParquetSharp.Column<string>("obj_tid"),
//                    new ParquetSharp.Column<long>("action_cnt"),
//                    new ParquetSharp.Column<string>("prev_objs"),
//                    new ParquetSharp.Column<string>("update_objs"),
//                    new ParquetSharp.Column<DateTime>("dw_action_time")
//                };

//            m_File = new ParquetSharp.ParquetFileWriter(fileName, cols);
//            m_RowGroupWriter = m_File.AppendBufferedRowGroup();
//        }

//        public bool Write(LogBase log)
//        {
//            int property = 0;
//            if (!Write(property++, log.log_type)) return false;
//            if (!Write(property++, log.uid)) return false;
//            if (!Write(property++, log.cid)) return false;
//            if (!Write(property++, log.sub_str)) return false;
//            if (!Write(property++, log.obj_type)) return false;
//            if (!Write(property++, log.obj_uid)) return false;
//            if (!Write(property++, log.obj_tid)) return false;
//            if (!Write(property++, log.action_cnt)) return false;
//            //if (!Write(property++, Global.LogHelper.PrevObjToJson(log))) return false;
//            //if (!Write(property++, Global.LogHelper.UpdateObjToJson(log))) return false;
//            if (!Write(property++, log.dw_action_time)) return false;

//            ++m_LoggingCnt;
//            return true;
//        }
//        public bool Write<T>(int colIdx, T data)
//        {
//            try
//            {
//                using (var writer = m_RowGroupWriter.Column(colIdx).LogicalWriter<T>())
//                {
//                    writer.WriteBatch(new T[] { data });
//                }
//                return true;
//            }
//            catch (Exception e)
//            {
//                LogServer.CLogger.Instance.Error($"write failed [{e.ToString()}]");
//            }

//            return false;
//        }

//        public bool CheckReplaceWithRelease(bool bForce)
//        {
//            if (m_File == null)
//            {
//                return false;
//            }


//            if (!bForce)
//            {
//                if (m_LoggingCnt < m_LoggingCapacity && !m_Timer.Check())
//                    return false;
//            }

//            Release();

//            return true;
//        }
//    }

//    public class SLogManager : SSingleton<SLogManager>
//    {
//        private Thread m_Thread;
//        private volatile bool m_bRun = false;
//        private ConcurrentQueue<LogBase> m_Queue = new ConcurrentQueue<LogBase>();
//        private ParquetFileBase m_ParquetFile = null;

//        public int QueueCnt { get { return m_Queue.Count; } }

//        private string GetFileName()
//        {
//            //[todo:]
//            Directory.CreateDirectory(@"./AWS");
//            Directory.CreateDirectory(@"./AWS/Log");
//            return @"./AWS/Log/" + DateTime.UtcNow.ToString("yyyy-MM-dd_HHmmssfff");
//        }

//        public void Init()
//        {
//            m_Thread = new Thread(new ThreadStart(ThreadFunc));
//            m_Thread.Start();
//        }
//        public void Stop()
//        {
//            m_bRun = false;

//            if (m_Thread != null)
//                m_Thread.Join();

//            CheckUpload(true);
//        }

//        private void ThreadFunc()
//        {
//            m_bRun = true;
//            while (m_bRun)
//            {
//                while (m_Queue.TryDequeue(out LogBase log))
//                {
//                    Logging(log);
//                }

//                Thread.Sleep(1);
//            }

//            while (m_Queue.TryDequeue(out LogBase log))
//            {
//                Logging(log);
//            }
//        }
//        public void Push(LogBase log)
//        {
//            m_Queue.Enqueue(log);
//        }

//        private void Logging(LogBase log)
//        {
//            CheckUpload(false);

//            var file = GetOrCreateParquetFile();
//            if (file == null)
//                return;

//            file.Write(log);
//        }

//        private ParquetFileBase GetOrCreateParquetFile()
//        {
//            if (m_ParquetFile == null)
//            {
//                int uploadTimeMs = LogServer.CConfig.Instance.AWS_UploadTimeSec * 1000;
//                int fileRowCapacity = LogServer.CConfig.Instance.AWS_FileRowCapacity;
//                m_ParquetFile = new ParquetFileBase(fileRowCapacity, uploadTimeMs);
//                m_ParquetFile.Create(GetFileName());
//            }

//            return m_ParquetFile;
//        }

//        private void CheckUpload(bool bForce)
//        {
//            if (m_ParquetFile == null)
//                return;

//            if (!m_ParquetFile.CheckReplaceWithRelease(bForce))
//                return;

//            m_ParquetFile = null;

//            SAWSS3Manager.Instance.CheckUpload();
//        }
//    }
//}
