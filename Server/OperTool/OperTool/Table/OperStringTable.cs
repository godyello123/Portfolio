using OperTool;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class OperStringRecord : STableRecord<string>
    {
        public string KOR = string.Empty;
        public string ENG = string.Empty;

        public string String(eLanguageType type)
        {
            switch (type) 
            {
                case eLanguageType.Kor: return KOR;
                case eLanguageType.Eng: return ENG;
            }

            return $"!{ID}";
        }
    }

    public class OperStringTalbe : STable<OperStringTalbe, string, OperStringRecord>
    {
        public override void Prepare() { }
        
        public override bool Load(string text)
        {
            Clear();

            SCSVReader reader = new SCSVReader();
            if (!reader.LoadFromString(text)) return false;

            for (int i = 0; i < reader.GetRowCount(); i++)
            {
                OperStringRecord record = new OperStringRecord();
                record.ID = reader.GetValue(i , "ID");
                record.KOR = reader.GetValue(i, "KOR", $"!{record.ID}");
                record.ENG = reader.GetValue(i, "ENG", $"!{record.ID}");

                Add(record.ID, record);
            }

            return true;
        }

        public string String(string key, eLanguageType type = eLanguageType.Eng)
        {
            var record = Find(key);
            if (record == null)
                return $"!{key}";

            return record.String(type);
        }
    }
}
