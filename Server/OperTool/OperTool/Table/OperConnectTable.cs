using OperTool;
using SCommon;
using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class OperConnectRecord : STableRecord<eServerType>
    {
        public string DN = string.Empty;
        public ushort Port = 0;
    }

    public class OperConnectTable : STable<OperConnectTable, eServerType, OperConnectRecord>
    {
        public override void Prepare() { }
        
        public override bool Load(string text)
        {
            Clear();

            SCSVReader reader = new SCSVReader();
            if (!reader.LoadFromString(text)) return false;

            for (int i = 0; i < reader.GetRowCount(); i++)
            {
                OperConnectRecord record = new OperConnectRecord();
                record.ID = reader.GetEnum<eServerType>(i , "ServerType", eServerType.Max);
                record.DN = reader.GetValue(i, "DN");
                record.Port = reader.GetValue<ushort>(i, "Port", 0);

                if (record.ID == eServerType.Local || string.IsNullOrEmpty(record.DN))
                    record.DN = SNetSystem.GetLocalIP();
                
                Add(record.ID, record);
            }

            return true;
        }
    }
}
