using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Global;
using SCommon;

namespace PlayServer
{
    class CTableLoader
    {
        private static string m_CurrentPath = string.Empty;

        public static bool Init()
        {
            m_CurrentPath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "Table");

            if (!LoadTable())
            {
                CLogger.Instance.System($"TableLoad Fail!");
                SCrashManager.TableLoadFailCheck();
                return false;
            }

            Prepare();

            return true;
        }

        private static bool LoadTable()
        {
            if (!ConditionTable.Instance.Load(LoadFile("ConditionTable.csv"))) return false;
            if (!StageTable.Instance.Load(LoadFile("StageTable.csv"))) return false;
            if (!ActorTable.Instance.Load(LoadFile("ActorTable.csv"))) return false;
            if (!SkillTable.Instance.Load(LoadFile("SkillTable.csv"))) return false;
            if (!RewardTable.Instance.Load(LoadFile("RewardTable.csv"))) return false;
            if (!GrowthGoldTable.Instance.Load(LoadFile("GrowthGoldTable.csv"))) return false;
            if (!GrowthLevelTable.Instance.Load(LoadFile("GrowthLevelTable.csv"))) return false;
            if (!DefaultPlayerTable.Instance.Load(LoadFile("DefaultPlayerTable.csv"))) return false;
            if (!LevelUpTable.Instance.Load(LoadFile("LevelUpTable.csv"))) return false;
            if (!DefineTable.Instance.Load(LoadFile("DefineTable.csv"))) return false;
            if (!MissionTable.Instance.Load(LoadFile("MissionTable.csv"))) return false;
            if (!QuestTable.Instance.Load(LoadFile("QuestTable.csv"))) return false;
            if (!ItemEnchantTable.Instance.Load(LoadFile("ItemEnchantTable.csv"))) return false;
            if (!ItemTable.Instance.Load(LoadFile("ItemTable.csv"))) return false;
            if (!CoinTable.Instance.Load(LoadFile("CoinTable.csv"))) return false;
            if (!AbilityTable.Instance.Load(LoadFile("AbilityTable.csv"))) return false;
            if (!ItemRandomOptionTable.Instance.Load(LoadFile("ItemRandomOptionTable.csv"))) return false;
            if (!ItemBreakthroughTable.Instance.Load(LoadFile("ItemBreakthroughTable.csv"))) return false;
            if (!ItemRandomSlotTable.Instance.Load(LoadFile("ItemRandomSlotTable.csv"))) return false;
            if (!ItemChivalryEnchantTable.Instance.Load(LoadFile("ItemChivalryEnchantTable.csv"))) return false;
            if (!GachaTable.Instance.Load(LoadFile("GachaTable.csv"))) return false;
            if (!GachaLvTable.Instance.Load(LoadFile("GachaLvTable.csv"))) return false;
            if (!GachaProbTable.Instance.Load(LoadFile("GachaProbTable.csv"))) return false;
            if (!DropTable.Instance.Load(LoadFile("DropTable.csv"))) return false;
            if (!RelicTable.Instance.Load(LoadFile("RelicTable.csv"))) return false;
            if (!ShopTable.Instance.Load(LoadFile("ShopTable.csv"))) return false;
            if (!PostTable.Instance.Load(LoadFile("PostTable.csv"))) return false;
            if (!AdsBuffTable.Instance.Load(LoadFile("ADBuffTable.csv"))) return false;
            if (!StageSkillTable.Instance.Load(LoadFile("StageSkillTable.csv"))) return false;
            if (!ProfileTable.Instance.Load(LoadFile("ProfileTable.csv"))) return false;
            if (!EventTable.Instance.Load(LoadFile("EventTable.csv"))) return false;
            if (!EventShopTable.Instance.Load(LoadFile("EventShopTable.csv"))) return false;
            if (!EventRouletteTable.Instance.Load(LoadFile("EventRouletteTable.csv"))) return false;
            if (!KnightUpgradeTable.Instance.Load(LoadFile("KnightsUpgradeTable.csv"))) return false;

            return true;
        }

        private static void Prepare()
        {
            ConditionTable.Instance.Prepare();
            StageTable.Instance.Prepare();
            ActorTable.Instance.Prepare();
            SkillTable.Instance.Prepare();
            RewardTable.Instance.Prepare();
            GrowthGoldTable.Instance.Prepare();
            GrowthLevelTable.Instance.Prepare();
            DefaultPlayerTable.Instance.Prepare();
            LevelUpTable.Instance.Prepare();
            DefineTable.Instance.Prepare();
            MissionTable.Instance.Prepare();
            QuestTable.Instance.Prepare();
            ItemEnchantTable.Instance.Prepare();
            ItemTable.Instance.Prepare();
            CoinTable.Instance.Prepare();
            AbilityTable.Instance.Prepare();
            ItemRandomOptionTable.Instance.Prepare();
            ItemBreakthroughTable.Instance.Prepare();
            ItemRandomSlotTable.Instance.Prepare();
            ItemChivalryEnchantTable.Instance.Prepare();
            GachaTable.Instance.Prepare();
            GachaLvTable.Instance.Prepare();
            GachaProbTable.Instance.Prepare();
            DropTable.Instance.Prepare();
            RelicTable.Instance.Prepare();
            ShopTable.Instance.Prepare();
            PostTable.Instance.Prepare();
            AdsBuffTable.Instance.Prepare();
            StageSkillTable.Instance.Prepare();
            ProfileTable.Instance.Prepare();
            KnightUpgradeTable.Instance.Prepare();
        }

        private static string LoadFile(string tableName)
        {
            string path = System.IO.Path.Combine(m_CurrentPath, tableName);
            using (var reader = new StreamReader(path))
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                var text = reader.ReadToEnd();
                return text;
            }
        }
    }
}
