using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCommon;
using Global;


namespace Global
{
    public class SkinRecord : STableRecord<int>
    {

    }

    public class SkinData : STable<SkinData, int, SkinRecord>
    {
        private SkinRecord m_DefaultSkin = new SkinRecord();

        public override bool Load(string text)
        {
            Clear();

            try
            {
                bool isSetDefault = false;

                SCSVReader reader = new SCSVReader();
                if (!reader.LoadFromString(text)) return false;

                for (int i = 0; i < reader.GetRowCount(); i++)
                {
                    SkinRecord record = new SkinRecord();
                    record.ID = reader.GetValue<int>(i, "ID");

                    Add(record.ID, record);

                    if (!isSetDefault)
                    {
                        m_DefaultSkin.ID = record.ID;
                        isSetDefault = true;
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
    }
}
