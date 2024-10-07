using Global;
using PlayServer;
using SCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Global
{
    public class GachaProb
    {
        public int m_Prob = 0;
        public int m_DropTID = 0;
    }
    
    public class GachaProbRecord : STableRecord<int>
    {
        public int GroupID = 0;
        public CDefine.EItemMainType GachaType = CDefine.EItemMainType.Max;
        public int GachaLv = 0;
        public List<GachaProb> Probs = new List<GachaProb>();
        public int MaxProb = 0;
    }

    public class GachaProbTable : SPairKeyTable<GachaProbTable, int, int, GachaProbRecord>
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
                    GachaProbRecord record = new GachaProbRecord();
                    record.ID = reader.GetValue<int>(i, "ID");
                    record.GroupID = reader.GetValue<int>(i, "Group_ID", -1);
                    record.GachaType = reader.GetEnum<CDefine.EItemMainType>(i, "ItemMainType", CDefine.EItemMainType.Max);
                    record.GachaLv = reader.GetValue<int>(i, "GachaLv", 0);

                    int idx = 1;
                    int total_prob = 0;
                    while (true)
                    {
                        string strProb = "Drop_Prob_" + idx.ToString();
                        if (!reader.DoesColumnExist(strProb))
                            break;

                        string strProbItem = "DropID_" + idx.ToString();
                        if (!reader.DoesColumnExist(strProbItem))
                            break;

                        string strProb_val = reader.GetValue(i, strProb);
                        if (string.IsNullOrEmpty(strProb_val))
                            break;

                        string strProbItem_val = reader.GetValue(i, strProbItem);
                        if (string.IsNullOrEmpty(strProbItem_val))
                            break;

                        int prob = reader.GetValue(i, strProb, 0);
                        if (prob <= 0)
                        {
                            idx++;
                            continue;
                        }

                        total_prob += prob;

                        GachaProb probData = new GachaProb();
                        probData.m_DropTID = reader.GetValue(i, strProbItem, 0);
                        probData.m_Prob = total_prob;
                        record.Probs.Add(probData);

                        ++idx;
                    }

                    record.MaxProb = total_prob;

                    Add(record.GroupID, record.GachaLv, record);
                }

                return true;
            }
            catch (Exception e)
            {
                SCrashManager.AddTableErrorString($"{this.GetType()} : {e.ToString()}");
                return false;

            }
        }

        public List<_AssetData> Roulette(int groupid , int lv, int count)
        {
            var gachaProbRecord = Find(groupid, lv);
            if (gachaProbRecord == null)
                return null;

            List<_AssetData> ret = new List<_AssetData>();
            for (int i = 0; i < count; i++)
            {
                int randVal = SRandom.Instance.Next(0, gachaProbRecord.MaxProb);

                foreach(var iter in gachaProbRecord.Probs)
                {
                    if (randVal > iter.m_Prob)
                        continue;

                    var rewards = DropTable.Instance.Roulette(iter.m_DropTID, 1);
                    ret.AddRange(rewards);
                    break;
                }
            }

            return ret;
        }

        public List<_AssetData> ChivalryRoullet(int groupid, int lv, int cnt, List<int> exceptList)
        {
            var gachaProbRecord = Find(groupid, lv);
            if (gachaProbRecord == null)
                return null;

            List<_AssetData> ret = new List<_AssetData>();
            int randVal = SRandom.Instance.Next(0, gachaProbRecord.MaxProb);

            foreach (var iter in gachaProbRecord.Probs)
            {
                if (randVal > iter.m_Prob)
                    continue;

                var rewards = DropTable.Instance.ChivalryRoulette(iter.m_DropTID, cnt, exceptList);
                ret.AddRange(rewards);
                break;
            }

            return ret;
        }
    }
}
