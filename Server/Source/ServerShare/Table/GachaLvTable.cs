using Global;
using PlayServer;
using SCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class GachaLvInfo
    {
        public int m_MaxLv = 0;
        public int m_BeginRewardLv = 0;
    }
    
    public class GachaLvRecord : STableRecord<int>
    {
        public int GroupID = 0;
        public int GachaLv = 0;
        public long Exp = 0;
        public int RewardID = 0;
        public bool Merge = false;
        public int GachaBonus_CountNum = 0;
        public int GachaBonus_Num = 0;
    }

    public class GachaLvTable : SPairKeyTable<GachaLvTable, int, int, GachaLvRecord>
    {
        private Dictionary<int, GachaLvInfo> m_GachaLvInfo = new Dictionary<int, GachaLvInfo>();
        
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
                    GachaLvRecord record = new GachaLvRecord();
                    record.ID = reader.GetValue<int>(i, "ID");
                    record.GroupID = reader.GetValue<int>(i, "Group_ID", -1);
                    record.GachaLv = reader.GetValue<int>(i, "GachaLv", 0);
                    record.Exp = reader.GetValue<long>(i, "Exp", 0);
                    record.RewardID = reader.GetValue<int>(i, "RewardID", 0);
                    record.Merge = reader.GetValueBool(i, "Merge");
                    record.GachaBonus_CountNum = reader.GetValue<int>(i, "GachaBonus_CountNum", 10);
                    record.GachaBonus_Num = reader.GetValue<int>(i, "GachaBonus_Num", 0);

                    Add(record.GroupID, record.GachaLv, record);

                    if (m_GachaLvInfo.ContainsKey(record.GroupID))
                    {
                        int maxLv = m_GachaLvInfo[record.GroupID].m_MaxLv;
                        m_GachaLvInfo[record.GroupID].m_MaxLv = maxLv < record.GachaLv ? record.GachaLv : maxLv;

                        if (record.RewardID != 0)
                        {
                            int beginRewardLv = m_GachaLvInfo[record.GroupID].m_BeginRewardLv;
                            m_GachaLvInfo[record.GroupID].m_BeginRewardLv = beginRewardLv < record.GachaLv ? beginRewardLv : record.RewardID;
                        }
                    }
                    else
                    {
                        GachaLvInfo info = new GachaLvInfo();
                        info.m_MaxLv = record.GachaLv;
                        info.m_BeginRewardLv = record.GachaLv;
                        m_GachaLvInfo[record.GroupID] = info;
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

        public int GetGachaMaxLv(int _groupID)
        {
            if (m_GachaLvInfo.ContainsKey(_groupID))
                return m_GachaLvInfo[_groupID].m_MaxLv;

            return -1;
        }

        public int GetGachaBeginRewardLv(int _groupID)
        {
            if(m_GachaLvInfo.ContainsKey(_groupID))
                return m_GachaLvInfo[_groupID].m_BeginRewardLv;

            return -1;
        }

        public GachaLvRecord FindbyExp(int _groupId,int _level, long _exp)
        {
            if (_level >= GetGachaMaxLv(_groupId))
                return null;

            var curRecord = Find(_groupId, _level);
            var nextRecord = Find(_groupId, _level + 1);

            if (curRecord == null)
                return null;

            if (nextRecord == null)
                return curRecord;

            while (true)
            {
                if (curRecord.Exp <= _exp && _exp < nextRecord.Exp)
                    return curRecord;

                curRecord = nextRecord;

                nextRecord = Find(_groupId, nextRecord.GachaLv + 1);
                if (nextRecord == null)
                    return curRecord;
            }
        }

        public List<_GachaData> CopyDefault()
        {
            var retList = new List<_GachaData>();
            foreach(var iter in Table)
            {
                _GachaData data = new _GachaData();
                data.m_GroupID = iter.Key;
                data.m_Level = 1;
                data.m_Rewarded = 1;
                data.m_Exp = 0;

                retList.Add(data);
            }

            return retList;
        }
    }
}
