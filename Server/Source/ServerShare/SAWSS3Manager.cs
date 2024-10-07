//using System;
//using System.Collections.Generic;
//using System.Collections.Concurrent;
//using SCommon;
//using System.Threading.Tasks;
//using System.Threading;
//using System.IO;
//using System.Diagnostics;
//using Global;

//namespace Global
//{
//    public class AWSLoggingFile
//    {
//        public string m_Dir;
//        public string m_FileName;

//        public string FileFullPath { get { return m_Dir + "/" + m_FileName; } }
//    }

//    public class SAWSS3Manager : SSingleton<SAWSS3Manager>
//    {
//        private Thread m_Thread;
//        private volatile bool m_bRun = false;
//        private ConcurrentQueue<AWSLoggingFile> m_UploadList = new ConcurrentQueue<AWSLoggingFile>();

//        private string m_AccessKeyID = "AKIAVMMVENMGLXFAMVP2";
//        private string m_AccessSecretKey = "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";
//        private Amazon.RegionEndpoint m_BucketRegion = Amazon.RegionEndpoint.APNortheast2; //todo
//        private string m_BucketName = "";

//        private string m_DirPath = @"./AWS/Log";

//        public bool Init(string bucketName, string bucketRegion)
//        {
//            m_BucketName = bucketName;
//            m_BucketRegion = Amazon.RegionEndpoint.GetBySystemName(bucketRegion);

//            CheckUpload();

//            m_Thread = new Thread(new ThreadStart(ThreadFunc));
//            m_Thread.Start();

//            return true;
//        }

//        public void Stop()
//        {
//            m_bRun = false;

//            if (m_Thread != null)
//                m_Thread.Join();
//        }

//        private void ThreadFunc()
//        {
//            m_bRun = true;
//            while (m_bRun)
//            {
//                while (m_UploadList.TryDequeue(out AWSLoggingFile file))
//                {
//                    Upload(file);
//                }

//                Thread.Sleep(1);
//            }

//            while (m_UploadList.TryDequeue(out AWSLoggingFile file))
//            {
//                Upload(file);
//            }
//        }

//        private void Push(AWSLoggingFile data)
//        {
//            m_UploadList.Enqueue(data);
//        }

//        private void Upload(AWSLoggingFile fileInfo)
//        {
//            string filePath = fileInfo.FileFullPath;
//            if (!System.IO.File.Exists(filePath))
//            {
//                return;
//            }

//            string bucketName = m_BucketName;
//            string parServer = LogServer.CConfig.Instance.AWS_AthenaPar1;
//            string parDt = "dt=" + fileInfo.m_FileName.Substring(0, 10);
//            string key = parServer + "/" + parDt + "/" + fileInfo.m_FileName;

//            if (UploadFile(filePath, bucketName, key))
//            {
//                RemoveFile(fileInfo);
//            }
//        }

//        private void RemoveFile(AWSLoggingFile fileInfo)
//        {
//            if (!System.IO.File.Exists(fileInfo.FileFullPath))
//            {
//                return;
//            }

//            System.IO.File.Delete(fileInfo.FileFullPath);
//        }
        
//        public bool UploadFile(string filePath, string bucketName, string key)
//        {
//            try
//            {
//                var transferUtil = CreateTransferUtil();
//                var task = transferUtil.UploadAsync(filePath, bucketName, key);
//                task.Wait();

//                return task.IsCompleted;                
//            }
//            catch (Exception e)
//            {
//                LogServer.CLogger.Instance.Error($"upload failed [{e.ToString()}]");
//                return false;
//            }
//        }

//        private Amazon.S3.Transfer.TransferUtility CreateTransferUtil()
//        {
//            return new Amazon.S3.Transfer.TransferUtility(m_AccessKeyID, m_AccessSecretKey, m_BucketRegion);
//        }
        
//        public void CheckUpload()
//        {
//            Directory.CreateDirectory(m_DirPath);
//            var files = System.IO.Directory.GetFiles(m_DirPath);
//            if (files == null)
//                return;

//            string strDate = DateTime.UtcNow.ToString("yyyy-MM-dd_HHmmssfff");
//            foreach (var fileName in files)
//            {
//                AWSLoggingFile loggingFile = new AWSLoggingFile();
//                loggingFile.m_Dir = m_DirPath;
//                loggingFile.m_FileName = System.IO.Path.GetFileName(fileName);

//                int cmp = loggingFile.m_FileName.CompareTo(strDate);
//                if (cmp <= 0)
//                {
//                    Push(loggingFile);
//                }
//            }
//        }
//    }
//}
