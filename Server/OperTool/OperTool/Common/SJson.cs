using System;
using System.Text;
using Newtonsoft.Json;

namespace SCommon
{
	public class SJson
	{
		public static string RemoveBOM(string json)
		{
			byte[] buff = Encoding.UTF8.GetBytes(json);
			if(buff.Length >= 3)
			{
				if(buff[0] == (byte)0xEF && buff[1] == (byte)0xBB && buff[2] == (byte)0xBF)
				{
					return Encoding.UTF8.GetString(buff, 3, buff.Length - 3);
				}
			}
			return json;
		}
		public static string ObjectToJson(object obj)
		{
            string str = JsonConvert.SerializeObject(obj);
            if (true == string.IsNullOrEmpty(str))
                str = "";

            return str;
        }
        public static T JsonToObject<T>(string json)
		{
            return JsonConvert.DeserializeObject<T>(RemoveBOM(json));
        }

        public static bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = Newtonsoft.Json.Linq.JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
