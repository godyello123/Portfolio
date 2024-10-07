using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using Google.Apis.AndroidPublisher.v3.Data;
using SCommon;

namespace PlayServer
{
    public class CSkillAgent
    {
        private CUser m_Owner;

        private Dictionary<int, _SkillData> m_Skills = new Dictionary<int, _SkillData>();

        private Dictionary<int, _SkillEquipPresetData> m_SkillEquipPresets = new Dictionary<int, _SkillEquipPresetData>();
        private Dictionary<CDefine.eStageType, _StageSkillData> m_Skill4Stage = new Dictionary<CDefine.eStageType, _StageSkillData>();

        public CUser Owner { get => m_Owner; }

        public CSkillAgent(CUser owner) { m_Owner = owner; }

        private void Prepare()
        {
            //knight slot preset
            int equip_max = DefineTable.Instance.Value<int>("Equip_Max_Preset");
            int skill_slot_max = DefineTable.Instance.Value<int>("Skill_Max_Slot");
            for (int i = 0; i < equip_max; ++i)
            {
                _SkillEquipPresetData main_preset = new _SkillEquipPresetData();
                main_preset.m_Index = (int)i;

                for (int j = 0; j < skill_slot_max; ++j)
                {
                    _SkillEquipSlotData slot = new _SkillEquipSlotData();
                    slot.No = j;
                    slot.ID = -1;
                    main_preset.m_EquipedSlots.Add(slot);
                }

                m_SkillEquipPresets.Add(i, main_preset);
            }

            
            for (CDefine.eStageType type = CDefine.eStageType.Main_Stage; type < CDefine.eStageType.Max; ++type)
            {
                if (type == CDefine.eStageType.Max || type == CDefine.eStageType.Main_Stage)
                    continue;

                _StageSkillData skillData = new _StageSkillData();
                skillData.m_Type = type;

                var record = StageSkillTable.Instance.Find(skillData.m_Type);
                if (record == null)
                    continue;

                for (int j = 0; j < record.MaxEquipSlot; ++j)
                {
                    _SkillEquipSlotData slot = new _SkillEquipSlotData();
                    slot.No = j;
                    slot.ID = -1;
                    skillData.m_EquipedSlots.Add(slot);
                }

                m_Skill4Stage[type] = skillData;
            }
            
            var copyDefault = SkillTable.Instance.CopyDefalut();
            foreach(var iter in copyDefault)
                m_Skills[iter.GroupID] = iter;
        }

        public void Init(List<_SkillEquipPresetData> skillpresets, List<_SkillData> loadskills, List<_StageSkillData> stageSkills)
        {
            Prepare();

            foreach (var iter in loadskills)
                m_Skills[iter.GroupID] = iter;

            //load preset
            foreach (var iter in skillpresets)
                m_SkillEquipPresets[iter.m_Index] = iter;

            foreach (var iter in stageSkills)
                m_Skill4Stage[iter.m_Type] = iter;

            RefreshSkillPreset();
        }

        private void RefreshSkillPreset()
        {
            bool is_check = false;
            foreach (var iter in m_SkillEquipPresets)
            {
                var preset = iter.Value;
                if (preset.m_IsEnable)
                {
                    is_check = true;
                }
            }

            if (!is_check)
            {
                var first = m_SkillEquipPresets.FirstOrDefault();
                var preset = first.Value;
                preset.m_IsEnable = true;
            }
        }

        public List<_SkillData> GetList()
        {
            var retlist = new List<_SkillData>();
            foreach(var iter in m_Skills)
            {
                var data = iter.Value;
                retlist.Add(data);
            }

            return retlist;
        }

        public List<_SkillEquipPresetData> GetSkillPresetList()
        {
            return new List<_SkillEquipPresetData>(m_SkillEquipPresets.Values);
        }

        public List<_StageSkillData> GetStageSkillList()
        {
            return new List<_StageSkillData>(m_Skill4Stage.Values);
        }

        public List<_SkillData> GetEnableSkillPresetList()
        {
            var preset = SelectSkillPreset();
            var retList = new List<_SkillData>();
            foreach(var iter in preset.m_EquipedSlots)
            {
                _SkillData skill = FindSkill(iter.ID);
                if (skill == null)
                    continue;

                retList.Add(skill);
            }

            return retList;
        }

        public _SkillData FindSkill(int _skillid)
        {
            if (m_Skills.ContainsKey(_skillid))
                return m_Skills[_skillid];

            return null;
        }

        public _SkillEquipPresetData FindSkillPreset(int _presetNo)
        {
            if (m_SkillEquipPresets.ContainsKey(_presetNo))
                return m_SkillEquipPresets[_presetNo];

            return null;
        }

        public _StageSkillData FindStageSkill(CDefine.eStageType type)
        {
            if (m_Skill4Stage.ContainsKey(type))
                return m_Skill4Stage[type];

            return null;
        }

   
        public Packet_Result.Result ReqSkillEquip(long _sessionKey, int _presetNo, int _slotNo, int _groupID, bool _equip)
        {
            var preset = FindSkillPreset(_presetNo);
            if (preset == null)
                return Packet_Result.Result.NotFoundData;

            var slot = preset.m_EquipedSlots.Find((x) => { return x.No == _slotNo; });
            if (slot == null)
                return Packet_Result.Result.InValidData;

            var skillData = FindSkill(_groupID);
            if (skillData == null)
                return Packet_Result.Result.NotFoundData;

            var skillRecord = SkillTable.Instance.Find(skillData.TID);
            if (skillRecord == null)
                return Packet_Result.Result.InValidRecord;

            if (!skillData.IsLearend)
                return Packet_Result.Result.ConditionCheckError;

            if (!m_Owner.IsCondition(DefineTable.Instance.GetListValue<int>("ConditionSkillLockID", _slotNo)))
                return Packet_Result.Result.PacketError;

            if (_equip)
            {
                var duplicateSlot = preset.m_EquipedSlots.Find(x => x.ID == _groupID);
                if (duplicateSlot != null)
                    return Packet_Result.Result.InValidData;

                slot.ID = _groupID;

                m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Equip_Skill, "", "", 1);
            }
            else
            {
                slot.ID = -1;
            }

            CNetManager.Instance.P2C_ResultSkillEquip(_sessionKey, preset, Packet_Result.Result.Success);
            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqSkillLevelUp(long _sessionKey, int _groupID)
        {
            var skillData = FindSkill(_groupID);
            if (skillData == null)
                return Packet_Result.Result.InValidData;

            var curRecord = SkillTable.Instance.Find(_groupID, skillData.Lv);
            if (curRecord == null)
                return Packet_Result.Result.InValidRecord;

            var goalRecord = SkillTable.Instance.Find(_groupID, skillData.Lv + 1);
            if (goalRecord == null)
                return Packet_Result.Result.AlreadyLast;

            List<_AssetData> costAsset = new List<_AssetData>();
            foreach (var it in goalRecord.ConsumeAssets)
            {
                if (!m_Owner.HasEnoughAsset(it.Type, it.TableID, it.Count))
                    return Packet_Result.Result.LackAssetError;

                costAsset.Add(new _AssetData(it.Type, it.TableID, -it.Count));
            }

            if (costAsset.Count < 1)
                return Packet_Result.Result.PacketError;

            CDBMerge dbtran = new CDBMerge();

            bool bSend = true;
            m_Owner.UpdateAssetData(costAsset, ref dbtran, bSend);
            dbtran.Merge();

            skillData.Lv = goalRecord.Level;
            skillData.TID = goalRecord.ID;

            //todo : quest
            //

            CDBManager.Instance.QueryCharacterUpdateSkillLevel(m_Owner.DBGUID, _sessionKey, m_Owner.UID, skillData, dbtran);
            return Packet_Result.Result.Success;
        }

        private _SkillEquipPresetData SelectSkillPreset()
        {
            foreach (var iter in m_SkillEquipPresets)
            {
                var itPreset = iter.Value;
                if (itPreset.m_IsEnable)
                    return itPreset;
            }

            return null;
        }

        public Packet_Result.Result ReqSelectSkillEquipPreset(long _sessionKey, int _idx)
        {
            var preset = FindSkillPreset(_idx);
            if (preset == null)
                return Packet_Result.Result.InValidData;

            var prevSelectedPreset = SelectSkillPreset();
            if (prevSelectedPreset == null)
                return Packet_Result.Result.PacketError;

            if (prevSelectedPreset.m_Index == preset.m_Index)
                return Packet_Result.Result.IgnoreError;

            prevSelectedPreset.m_IsEnable = false;
            preset.m_IsEnable = true;

            //RefreshSelectPreset();

            m_Owner.StatusAgent.RefreshAll();

            CNetManager.Instance.P2C_ResultSelectSkillEquipPreset(_sessionKey, _idx, Packet_Result.Result.Success);

            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqSkillOpen(long sessionKey, int groupID)
        {
            var skillGroupfirstRecord = SkillTable.Instance.FindGroupFirst(groupID);
            if(skillGroupfirstRecord == null)
            {
                CLogger.Instance.Debug($"skillGroupRecord == null, groupID : {groupID}");
                return Packet_Result.Result.InValidRecord;
            }

            var skillData = FindSkill(skillGroupfirstRecord.GroupID);
            if(skillData == null)
            {
                skillData = new _SkillData();
                skillData.GroupID = groupID;
                skillData.TID = skillGroupfirstRecord.ID;
                skillData.Lv = 1;
                m_Skills.Add(skillData.GroupID, skillData);
            }

            //todo : condition
            if(!m_Owner.IsCondition(skillGroupfirstRecord.ConditionID))
            {
                CLogger.Instance.Debug($"!conditionCheckRet, conditionCheckRet : {skillGroupfirstRecord.ConditionID}");
                return Packet_Result.Result.ConditionCheckError;
            }
            
            skillData.IsLearend = true;

            //mission
            m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.SkillLearn, "", skillData.GroupID.ToString(), 1);
            
            //DB
            CDBManager.Instance.QueryCharacterUpdateSkillOpen(m_Owner.DBGUID, sessionKey, m_Owner.UID, skillData);

            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqStageSkillEquip(long sessionKey, _StageSkillData stageSkill)
        {
            //valid
            if (stageSkill.m_EquipedSlots.Count < 1)
                return Packet_Result.Result.InValidData;

            HashSet<int> temp = new HashSet<int>();

            var findData = FindStageSkill(stageSkill.m_Type);
            if (findData == null)
                return Packet_Result.Result.NotFoundData;

            if (findData.m_EquipedSlots.Count != stageSkill.m_EquipedSlots.Count)
                return Packet_Result.Result.InValidData;

            var skillBanRecord = StageSkillTable.Instance.Find(stageSkill.m_Type);
            if (skillBanRecord == null)
                return Packet_Result.Result.InValidRecord;

            foreach (var iter in stageSkill.m_EquipedSlots)
            {
                if (iter.ID == -1)
                    continue;

                var skillRecord = SkillTable.Instance.Find(iter.ID);
                if (skillRecord == null)
                    return Packet_Result.Result.InValidRecord;

                var skillData = FindSkill(iter.ID);
                if (skillData == null)
                    return Packet_Result.Result.PacketError;

                if (!skillData.IsLearend)
                    return Packet_Result.Result.PacketError;

                if (skillBanRecord.IsBanSkill(iter.ID))
                    return Packet_Result.Result.PacketError;

                int idx = findData.m_EquipedSlots.FindIndex(x => x.No == iter.No);
                if (idx == -1)
                    return Packet_Result.Result.InVaildIndex;

                if (!temp.Add(iter.ID))
                    return Packet_Result.Result.PacketError;
            }
            

            findData.m_EquipedSlots.Clear();
            findData.m_EquipedSlots.AddRange(stageSkill.m_EquipedSlots);
            findData.m_Modify = true;

            CNetManager.Instance.P2C_ResultStageSkillEquip(sessionKey, findData, Packet_Result.Result.Success);
            return Packet_Result.Result.Success;
        }

        public void AfterQuerySkillOpen(long sessionKey, _SkillData data, Packet_Result.Result result)
        {
            CNetManager.Instance.P2C_ResultSkillOpen(sessionKey, data, result);
        }

        public void AfterQuerySkillLevelUp(long sessionKey, _SkillData data, Packet_Result.Result result)
        {
            CNetManager.Instance.P2C_ResultSkillLevelUp(sessionKey, data, result);
        }

        public void SaveAtLogout()
        {
            foreach(var iter in m_SkillEquipPresets)
            {
                var preset = iter.Value;
                CDBManager.Instance.QueryCharacterUpdateEquipSkillPreset(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, preset);
            }

            foreach(var iter in m_Skill4Stage)
            {
                var itVal = iter.Value;
                if (itVal.m_Modify)
                    CDBManager.Instance.QueryCharacterStageSkillUpdate(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, itVal);
            }
        }
    }
}
