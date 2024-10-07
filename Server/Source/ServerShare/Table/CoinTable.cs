using PlayServer;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class CoinRecord : STableRecord<string>
    {
        public CDefine.CoinType CoinType = CDefine.CoinType.Max;
        public long Limit = -1;
    }

    public class CoinTable : STable<CoinTable, string, CoinRecord>
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
                    CoinRecord record = new CoinRecord();
                    record.ID = reader.GetValue(i, "ID");
                    record.CoinType = reader.GetEnum(i, "ID", CDefine.CoinType.Max);
                    record.Limit = reader.GetValue(i, "Limit", long.MaxValue);

                    if (record.CoinType == CDefine.CoinType.Max)
                        continue;

                    Add(record.ID, record);
                }

                return true;
            }
            catch (Exception e)
            {
                SCrashManager.AddTableErrorString($"{this.GetType()} : {e.ToString()}");
                return false;
            }

        }

        public CDefine.CoinType CoinType(string tableID)
        {
            var record = Find(tableID);
            if (record == null)
                return CDefine.CoinType.Max;

            return record.CoinType;
        }
    }
}
