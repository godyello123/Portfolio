using Global;
using SCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class ItemEnchantRecord : STableRecord<int>
    {
        public int MaxLevel = -1;
        public string Enchant_CoinType = "";
        public int Enchant_StartCost = 0;
        public int Enchant_IncreaseCost = 0;

        public List<float> m_Ablis = new List<float>();
        public List<float> m_EquipAbils = new List<float>();
    }


    public class ItemEnchantTable : STable<ItemEnchantTable, int , ItemEnchantRecord>
    {
        private Dictionary<CDefine.EItemGrade, List<float>> m_ItemEnchantStepValue = new Dictionary<CDefine.EItemGrade, List<float>>();
        private int m_ItemEnchnatCalcLevel = 0;

        public double GetItemEnchantStepValue(CDefine.EItemGrade _grade, double _abilValue, int _itemLevel, double _enchantAddRatio)
        {
            List<float> listItemEnchantStepValue = new List<float>();
            if (!m_ItemEnchantStepValue.TryGetValue(_grade, out listItemEnchantStepValue))
                return 0.0;

            if (listItemEnchantStepValue == null || listItemEnchantStepValue.Count <= 0)
                return 0f;

            double abilValue = _abilValue;

            int currentLevelStep = _itemLevel / m_ItemEnchnatCalcLevel;
            int currentLevelRemainder = _itemLevel % m_ItemEnchnatCalcLevel;

            for (int i = 0; i < currentLevelStep; ++i)
            {
                if (i >= listItemEnchantStepValue.Count)
                    break;

                double value = (_abilValue * m_ItemEnchnatCalcLevel * _enchantAddRatio * listItemEnchantStepValue[i]);
                abilValue += System.Math.Max(0, value);
            }

            if (currentLevelRemainder > 0 && currentLevelStep < listItemEnchantStepValue.Count)
            {
                double value = (_abilValue * currentLevelRemainder * _enchantAddRatio * listItemEnchantStepValue[currentLevelStep]);
                abilValue += System.Math.Max(0, value);
            }

            return abilValue;
        }


        public override void Prepare() 
        {
            m_ItemEnchnatCalcLevel = DefineTable.Instance.Value<int>("ItemEnchant_Calc_Level");

            foreach (CDefine.EItemGrade grade in Enum.GetValues(typeof(CDefine.EItemGrade)))
            {
                if (grade == CDefine.EItemGrade.None || grade == CDefine.EItemGrade.Max) continue;

                string correct_str = DefineTable.Instance.Value<string>($"ItemEnchant_{grade}");
                if (string.IsNullOrEmpty(correct_str))
                    continue;

                string[] splitstr = correct_str.Split(',');
                foreach(var it in splitstr)
                {
                    float val;
                    if (!float.TryParse(it, out val))
                        continue;

                    if (m_ItemEnchantStepValue.ContainsKey(grade))
                        m_ItemEnchantStepValue[grade].Add(val);
                    else
                        m_ItemEnchantStepValue.Add(grade, new List<float> { val });
                }
            }
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
                    ItemEnchantRecord record = new ItemEnchantRecord();
                    record.ID = reader.GetValue<int>(i, "ID", 0);
                    record.MaxLevel = reader.GetValue<int>(i, "MaxLevel", -1);
                    record.Enchant_CoinType = reader.GetValue(i, "Enchant_CoinType");
                    record.Enchant_StartCost = reader.GetValue<int>(i, "Enchant_StartCost", 0);
                    record.Enchant_IncreaseCost = reader.GetValue<int>(i, "Enchant_IncreaseCost", 0);

                    int idx = 1;
                    while (true)
                    {
                        //Abil_Value
                        string strAbilValue = "Abil_Value" + idx.ToString();
                        if (!reader.DoesColumnExist(strAbilValue))
                            break;

                        string strAbilValue_val = reader.GetValue(i, strAbilValue);
                        if (string.IsNullOrEmpty(strAbilValue_val))
                            break;

                        float abilVal = reader.GetValue<float>(i, strAbilValue, 0.0f);
                        record.m_Ablis.Add(abilVal);

                        ++idx;
                    }

                    idx = 1;
                    while (true)
                    {
                        //Equip_Abil
                        string strEquipAbilType = "Equip_Abil" + idx.ToString();
                        if (!reader.DoesColumnExist(strEquipAbilType))
                            break;

                        string strEquipAbilType_val = reader.GetValue(i, strEquipAbilType);
                        if (string.IsNullOrEmpty(strEquipAbilType_val))
                            break;

                        float equipabil = reader.GetValue<float>(i, strEquipAbilType, 0.0f);
                        record.m_EquipAbils.Add(equipabil);

                        ++idx;
                    }


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
    }
}
