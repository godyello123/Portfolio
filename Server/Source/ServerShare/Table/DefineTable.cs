using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class DefineRecord : STableRecord<string>
    {
        public string Text;
        public long Long;
        public float Float;
        public double Double;
        public List<string> ListVal = new List<string>();
    }

    public class DefineTable : STable<DefineTable, string, DefineRecord>
    {
        public override void Prepare() { }
        
        public override bool Load(string text)
        {
            Clear();

            try
            {
                SCSVReader reader = new SCSVReader();
                if (!reader.LoadFromString(text)) return false;

                for (int i = 0; i < reader.GetRowCount(); i++)
                {
                    DefineRecord record = new DefineRecord();
                    record.ID = reader.GetValue(i, "ID");
                    try
                    {
                        record.Text = reader.GetValue(i, "Value");
                        if (long.TryParse(record.Text, out var longValue)) record.Long = longValue;
                        if (float.TryParse(record.Text, out var floatValue)) record.Float = floatValue;
                        if (double.TryParse(record.Text, out var doubleValue)) record.Double = doubleValue;

                        record.ListVal = reader.GetList<string>(i, "Value");

                        Add(record.ID, record);
                    }
                    catch (Exception e)
                    {
                        CommonLogger.Error($"{GetType().Name} - ID : {record.ID}, {e}");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                SCrashManager.AddTableErrorString($"{this.GetType()} : {e.ToString()}");
                return false;

            }
        }

        
        public T Value<T>(string strDefine)
        {
            var record = Find(strDefine);
            if (record == null)
                return default(T);

            return (T)Convert.ChangeType(record.Text, typeof(T));
        }

        public T GetListValue<T>(string strDefine, int idx)
        {
            var record = Find(strDefine);
            if (record == null)
                return default(T);

            if (record.ListVal == null || record.ListVal.Count <= idx || record.ListVal.Count == 0)
                return default(T);

            return (T)Convert.ChangeType(record.ListVal[idx], typeof(T));
        }
    }
}
