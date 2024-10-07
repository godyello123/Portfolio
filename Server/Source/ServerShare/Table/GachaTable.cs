using Global;
using PlayServer;
using SCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class GachaRecord : STableRecord<int>
    {
        public int LevelGroupID = 0;
        public CDefine.EItemMainType MainType = CDefine.EItemMainType.Max;
        public string AssetType = "";
        public int Cost_Once = 0;
        public int Cost_Set = 0;
        public int GachaMaxCount = 0;
        public int ConditionID = 0;
    }

    public class GachaTable : STable<GachaTable, int, GachaRecord>
    {
        public override void Prepare() 
        {
            
        }

        public override bool Load(string text)
        {
            Clear();

            try
            {
                SCSVReader reader = new SCSVReader();
                if (!reader.LoadFromString(text)) return false;

                for (int i = 0; i < reader.GetRowCount(); i++)
                {
                    GachaRecord record = new GachaRecord();
                    record.ID = reader.GetValue(i, "ID", 0);
                    record.LevelGroupID = reader.GetValue(i, "LevelGroup", 0);
                    record.MainType = reader.GetEnum<CDefine.EItemMainType>(i, "MainType", CDefine.EItemMainType.Max);
                    record.AssetType = reader.GetValue(i, "AssetType", "");
                    record.Cost_Once = reader.GetValue(i, "Cost_Once", 0);
                    record.Cost_Set = reader.GetValue(i, "Cost_Set", 0);
                    record.GachaMaxCount = reader.GetValue<int>(i, "GachaMaxCount", 0);
                    record.ConditionID = reader.GetValue<int>(i, "ConditionID", -1);

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
    }
}
