#define USE_JSON

using System;
using System.IO;
#if !USE_JSON
using System.Runtime.Serialization.Formatters.Binary;
#else
using Newtonsoft.Json;
#endif

namespace SCommon
{
	public static class SCopy<T>
	{
#if !USE_JSON
		public static T DeepCopy(object srcObject)
		{
			using(MemoryStream memoryStream = new MemoryStream())
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(memoryStream, srcObject);
				memoryStream.Seek(0, SeekOrigin.Begin);
				return (T)binaryFormatter.Deserialize(memoryStream);
			}
		}
#else
		public static T DeepCopy(object srcObject)
		{
			using(MemoryStream memoryStream = new MemoryStream())
			{
				return (T)JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(srcObject));
			}
		}
#endif
	}
}
