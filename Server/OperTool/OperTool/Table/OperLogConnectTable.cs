using Amazon.Runtime.Internal.Util;
using MongoDB.Driver.Core.Operations;
using OperTool;
using SCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace Global
{
    public class OperLogConnectRecord : STableRecord<string>
    {
        public string LogConnection = string.Empty;
        public string DBName = string.Empty;
    }

    public class OperLogConnectTable : STable<OperLogConnectTable, string, OperLogConnectRecord>
    {
        public override void Prepare() { }
        
        public override bool Load(string text)
        {
            Clear();

            SCSVReader reader = new SCSVReader();
            if (!reader.LoadFromString(text)) return false;

            for (int i = 0; i < reader.GetRowCount(); i++)
            {
                OperLogConnectRecord record = new OperLogConnectRecord();
                record.ID = reader.GetValue(i, "ID", string.Empty);
                record.LogConnection = reader.GetValue(i, "LogConnection", string.Empty);
                record.DBName = reader.GetValue(i, "DBName");

                if (string.IsNullOrEmpty(record.ID) || string.IsNullOrEmpty(record.LogConnection) || string.IsNullOrEmpty(record.DBName))
                    continue;

                Add(record.ID, record);
            }

            return true;
        }
    }
}
