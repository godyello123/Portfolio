//using System;
//using System.Collections.Generic;
//using System.IO;
//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.Blob;

//namespace SCommon
//{
//	public class SAzureStorage
//	{
//		private static CloudBlobContainer GetContainer(string accountName, string accountKey, string containerName)
//		{
//			string connectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", accountName, accountKey);
//			CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
//			CloudBlobClient client = account.CreateCloudBlobClient();
//			CloudBlobContainer container = client.GetContainerReference(containerName);
//			container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
//			return container;
//		}

//		private static CloudBlockBlob GetBlob(string accountName, string accountKey, string containerName, string fileName)
//		{
//			var container = GetContainer(accountName, accountKey, containerName);
//			return container.GetBlockBlobReference(fileName);
//		}

//		public static IEnumerable<IListBlobItem> GetBlobList(string accountName, string accountKey, string containerName, string prefix = null)
//		{
//			try
//			{
//				var container = GetContainer(accountName, accountKey, containerName);
//				return container.ListBlobs(prefix);
//			}
//			catch
//			{
//				return null;
//			}
//		}

//		public static string DownloadText(string accountName, string accountKey, string containerName, string file)
//		{
//			try
//			{
//				var blob = GetBlob(accountName, accountKey, containerName, file);
//				return blob.DownloadText();
//			}
//			catch
//			{
//				return null;
//			}
//		}

//		public static bool UploadText(string accountName, string accountKey,
//			string containerName, string file, string text, string contentType = "text/plain;charset=utf-8")
//		{
//			try
//			{
//				var blob = GetBlob(accountName, accountKey, containerName, file);
//				blob.UploadText(text);
//				if(contentType != null)
//				{
//					blob.Properties.ContentType = contentType;
//					blob.SetProperties();
//				}
//				return true;
//			}
//			catch
//			{
//				return false;
//			}
//		}

//		public static bool Upload(string accountName, string accountKey, string containerName, string dstFile, string srcFile, string contentType = null)
//		{
//			try
//			{
//				var blob = GetBlob(accountName, accountKey, containerName, dstFile);
//				using(var stream = File.OpenRead(srcFile))
//				{
//					blob.UploadFromStream(stream);
//				}
//				if(contentType != null)
//				{
//					blob.Properties.ContentType = contentType;
//					blob.SetProperties();
//				}
//				return true;
//			}
//			catch
//			{
//				return false;
//			}
//		}

//		public static bool Upload(string accountName, string accountKey, string containerName, string dstFile, byte[] srcMemory, string contentType = null)
//		{
//			try
//			{
//				var blob = GetBlob(accountName, accountKey, containerName, dstFile);
//				using(MemoryStream stream = new MemoryStream(srcMemory))
//				{
//					blob.UploadFromStream(stream);
//				}
//				if(contentType != null)
//				{
//					blob.Properties.ContentType = contentType;
//					blob.SetProperties();
//				}
//				return true;
//			}
//			catch
//			{
//				return false;
//			}
//		}
//	}
//}
