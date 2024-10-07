using Global;
using PlayServer;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class DropProb
    {
        public int m_Prob = 0;
        public int m_DropRewardID = 0;
    }

    public class DropRecord : STableRecord<int>
    {
        public List<DropProb> Probs = new List<DropProb>();
        public int MaxProb = 0;
    }

    public class DropTable : STable<DropTable, int, DropRecord>
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
                    DropRecord record = new DropRecord();
                    record.ID = reader.GetValue<int>(i, "ID", 0);

                    int idx = 1;
                    int total_prob = 0;
                    while (true)
                    {
                        string strProb = "Drop_Prob_" + idx.ToString();
                        if (!reader.DoesColumnExist(strProb))
                            break;

                        string strProbItem = "Drop_RewardID_" + idx.ToString();
                        if (!reader.DoesColumnExist(strProbItem))
                            break;

                        string strProb_val = reader.GetValue(i, strProb);
                        if (string.IsNullOrEmpty(strProb_val))
                            break;

                        string strProbItem_val = reader.GetValue(i, strProbItem);
                        if (string.IsNullOrEmpty(strProbItem_val))
                            break;


                        DropProb probData = new DropProb();
                        probData.m_DropRewardID = reader.GetValue(i, strProbItem, 0);

                        int prob = reader.GetValue(i, strProb, 0);
                        total_prob += prob;
                        probData.m_Prob = total_prob;

                        record.Probs.Add(probData);

                        ++idx;
                    }

                    record.MaxProb = total_prob;

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

        public List<_AssetData> Roulette(int dropid, int cnt)
        {
            var dropRecord = Find(dropid);
            if (dropRecord == null)
                return null;

            List<_AssetData> ret = new List<_AssetData>();

            for(int i = 0; i <cnt; i++)
            {
                int randValue = SRandom.Instance.Next(0, dropRecord.MaxProb);

                foreach (var prob in dropRecord.Probs)
                {
                    if (prob.m_Prob > randValue)
                    {
                        var rewards = RewardTable.Instance.Find(prob.m_DropRewardID);
                        if (rewards == null)
                            return ret;

                        foreach (var reward in rewards)
                        {
                            var asset = reward.Value.GetAssetData();
                            ret.Add(asset);
                            break;
                        }

                        break;
                    }
                }
            }
            
            return ret;
        }

        public List<_AssetData> ChivalryRoulette(int dropID, int cnt, List<int> exceptList)
        {
            var dropRecord = Find(dropID);
            if (dropRecord == null)
                return null;

            List<_AssetData> ret = new List<_AssetData>();
            var copyEntry = SCommon.SCopy<List<DropProb>>.DeepCopy(dropRecord.Probs);
            
            int calc_prob = 0;
            for(int i = 0; i < copyEntry.Count; i++)
            {
                copyEntry[i].m_Prob -= calc_prob;
                calc_prob += copyEntry[i].m_Prob;
            }

            int total_prob = 0;
            for (int i = copyEntry.Count-1; i >= 0; --i)
            {
                var entries = copyEntry[i];
                if (exceptList.Contains(entries.m_DropRewardID))
                {
                    copyEntry.RemoveAt(i);
                }
                else
                {
                    total_prob += entries.m_Prob;
                    entries.m_Prob = total_prob;
                }
            }

            for (int i = 0; i < cnt; i++)
            {
                int randValue = SRandom.Instance.Next(0, total_prob);

                for(int j = copyEntry.Count -1; j >=0; --j)
                {
                    var entry = copyEntry[j];
                    if (randValue > entry.m_Prob)
                        continue;

                    var rewards = RewardTable.Instance.Find(entry.m_DropRewardID);
                    if (rewards == null)
                        return ret;

                    foreach (var reward in rewards)
                    {
                        var asset = reward.Value.GetAssetData();
                        ret.Add(asset);
                        break;
                    }

                    break;
                }
            }

            return ret;
        }
    }
}
