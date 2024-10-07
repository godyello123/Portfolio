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

namespace Global
{
    public class OperAccountRecord : STableRecord<string>
    {
        public string PW = string.Empty;
        public eOperRole Role = eOperRole.Max;
    }

    public class OperAccountTable : STable<OperAccountTable, string, OperAccountRecord>
    {
        public override void Prepare() { }
        
        public override bool Load(string text)
        {
            Clear();

            SCSVReader reader = new SCSVReader();
            if (!reader.LoadFromString(text)) return false;

            for (int i = 0; i < reader.GetRowCount(); i++)
            {
                OperAccountRecord record = new OperAccountRecord();
                record.ID = reader.GetValue(i, "ID", string.Empty);
                record.PW = reader.GetValue(i, "PW", string.Empty);
                record.Role = reader.GetEnum<eOperRole>(i, "Role", eOperRole.Max);

                if (string.IsNullOrEmpty(record.ID) || string.IsNullOrEmpty(record.PW) || record.Role == eOperRole.Max)
                    continue;

                Add(record.ID, record);
            }

            return true;
        }

        public bool IsVaildAccount(string id, string pw)
        {
            var record = Find(id);
            if (record == null)
                return false;

            return record.PW == pw;
        }


        public bool IsEditor(string id)
        {
            var record = Find(id);
            if (record == null)
                return false;

            switch (record.Role) 
            {
                case eOperRole.Owner:
                case eOperRole.Editor:
                    return true;
                case eOperRole.Viewer:
                    return false;
            }

            return false;
        }

        public bool IsOwner(string id)
        {
            var record = Find(id);
            if (record == null)
                return false;

            switch (record.Role)
            {
                case eOperRole.Owner:
                    return true;
                case eOperRole.Editor:
                case eOperRole.Viewer:
                    return false;
            }

            return false;
        }
    }
}
