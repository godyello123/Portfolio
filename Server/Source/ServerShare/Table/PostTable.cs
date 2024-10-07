using Global;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.Adapters;

namespace Global
{
    public class PostRecord : STableRecord<int>
    {
        public CDefine.PostType PostType = CDefine.PostType.Max;
        public string Title_String = string.Empty;
        public string Msg_String = string.Empty;
        public long ExpTime = -1;
        public int RewardID { get; set; }

        public bool IsPermanent()
        {
            return ExpTime == -1;
        }
    }

    public class PostTable : STable<PostTable, int , PostRecord>
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
                    PostRecord record = new PostRecord();
                    record.ID = reader.GetValue(i, "Id", 0);
                    record.PostType = reader.GetEnum<CDefine.PostType>(i, "Post_Type", CDefine.PostType.Max);
                    record.Title_String = reader.GetValue(i, "Title_String", "");
                    record.Msg_String = reader.GetValue(i, "Msg_String", "");
                    record.RewardID = reader.GetValue(i, "RewardID", 0);
                    record.ExpTime = reader.GetValue(i, "ExpTime", -1);

                    if (record.PostType != CDefine.PostType.Max)
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

        public _PostData MakePost(int postID)
        {
            _PostData retVal = new _PostData();
            var record = Find(postID);
            if (record == null)
                return retVal;

            retVal.Type = record.PostType;
            retVal.ID = CServerDefine.GeneraterGUID();
            retVal.Title = record.Title_String;
            retVal.Msg = record.Msg_String;
            retVal.beginTime = SDateManager.Instance.CurrTime();

            if (record.IsPermanent())
                retVal.expireTime = SDateManager.Instance.DateTimeToTimeStamp(CDefine.MaxTime);
            else
                retVal.expireTime = SDateManager.Instance.CurrTime() + record.ExpTime;

            return retVal;
        }
    }
}
