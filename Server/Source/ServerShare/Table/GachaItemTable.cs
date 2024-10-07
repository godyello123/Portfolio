using Global;
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
    public class GachaItemProb
    {
        public int m_Prob = 0;
        public int m_GachaItemID = 0;
    }


    public class CGachaItemRecord : STableRecord<int>
    {
        public List<GachaItemProb> Probs = new List<GachaItemProb>();
        public int MaxProb = 0;

        public List<CAssetData> Gacha()
        {
            int randValue = SRandom.Instance.Next(0, MaxProb);

            List<CAssetData> ret = new List<CAssetData>();

            foreach (var prob in Probs)
            {
                if (prob.m_Prob > randValue)
                {
                    var rewards = CRewardTable.Instance.Find(prob.m_GachaItemID);
                    if (rewards == null)
                        return ret;

                    foreach(var reward in rewards)
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

        public List<CAssetData> ChivalryGacha(List<int> exceptList, int count)
        {
            List<CAssetData> ret = new List<CAssetData>();
            List<GachaItemProb> copyProbs = new List<GachaItemProb>();
            int prob = 0;
            foreach(var iter in Probs)
            {
                GachaItemProb gachaProb = new GachaItemProb();
                gachaProb.m_Prob = iter.m_Prob - prob;
                prob = gachaProb.m_Prob;
                gachaProb.m_GachaItemID = iter.m_GachaItemID;
                copyProbs.Add(gachaProb);
            }

            int total_prob = 0;
            for(int i = 0; i < copyProbs.Count; i++)
            {
                var entry = copyProbs[i];
                if(exceptList.Contains(entry.m_GachaItemID))
                {
                    copyProbs.RemoveAt(i);
                }
                else
                {
                    entry.m_Prob += total_prob;
                    total_prob = entry.m_Prob;
                }
            }

            for (int i = 0; i < count; i++)
            {
                int randValue = SRandom.Instance.Next(0, total_prob);

                foreach(var it in copyProbs)
                {
                    if (it.m_Prob > randValue)
                    {
                        var rewards = CRewardTable.Instance.Find(it.m_GachaItemID);
                        if (rewards == null)
                            break;

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
    }

    public class CGachaItemTable : STable<CGachaItemTable, int, CGachaItemRecord>
    {
        public override void Prepare() { }

        public override bool Load(string text)
        {
            Clear();

            SCSVReader reader = new SCSVReader();
            if (!reader.LoadFromString(text)) return false;

            for (int i = 0; i < reader.GetRowCount(); i++)
            {
                CGachaItemRecord record = new CGachaItemRecord();
                record.ID = reader.GetValue<int>(i, "ID", 0);
                
                int idx = 1;
                int total_prob = 0;
                while (true)
                {
                    string strProb = "Gacha_Prob_" + idx.ToString();
                    if (!reader.DoesColumnExist(strProb))
                        break;

                    string strProbItem = "Gacha_RewardID_" + idx.ToString();
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

                    GachaItemProb probData = new GachaItemProb();
                    probData.m_GachaItemID = reader.GetValue(i, strProbItem, 0);
                    probData.m_Prob = total_prob;

                    record.Probs.Add(probData);

                    ++idx;
                }

                record.MaxProb = total_prob;

                Add(record.ID, record);
            }

            return true;
        }
    }
}
