using Global;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class LevelUpRecord : STableRecord<int>
    {
        public int Level;
        public int exp;
        public int point;
    }

    public class LevelUpTable : STable<LevelUpTable, int , LevelUpRecord>
    {
        private int m_MaxLevel = 0;

        public override void Prepare() 
        {
            m_MaxLevel = m_Table.Max(x => x.Value.Level);
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
                    LevelUpRecord record = new LevelUpRecord();
                    record.ID = reader.GetValue<int>(i, "ID");
                    record.Level = record.ID;
                    record.exp = reader.GetValue(i, "exp", 0);
                    record.point = reader.GetValue(i, "point", 1);

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

        public LevelUpRecord FindbyExp(int level ,long exp, ref int levelPoint)
        {
            if (level >= m_MaxLevel)
                return null;

            var curRecord = Find(level);
            var nextRecord = Find(level + 1);

            if (curRecord == null)
                return null;

            if (nextRecord == null)
                return curRecord;

            while (true)
            {
                if (curRecord.exp <= exp && exp < nextRecord.exp)
                    return curRecord;

                curRecord = nextRecord;
                levelPoint += curRecord.point;
                
                nextRecord = Find(nextRecord.Level + 1);
                if (nextRecord == null)
                    return curRecord;
            }
        }
    }
}
