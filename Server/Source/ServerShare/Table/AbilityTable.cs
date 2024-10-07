using Global;
using PlayServer;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class AbilityRecord : STableRecord<CDefine.EAbility>
    {
        public double Max = 0;
    }

    public class AbilityTable : STable<AbilityTable, CDefine.EAbility, AbilityRecord>
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
                    AbilityRecord record = new AbilityRecord();
                    record.ID = reader.GetEnum<CDefine.EAbility>(i, "Abil_Type", CDefine.EAbility.Max);
                    record.Max = reader.GetValue(i, "Max", -1);

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

        public Dictionary<CDefine.EAbility, _AbilData> CopyDefault()
        {
            Dictionary<CDefine.EAbility, _AbilData> ret = new Dictionary<CDefine.EAbility, _AbilData>();

            foreach(var it in Table)
            {
                _AbilData abil = new _AbilData();
                abil.type = it.Key;
                abil.val = 0;
                ret.Add(abil.type, abil);
            }

            return ret;


        }
    }
}
