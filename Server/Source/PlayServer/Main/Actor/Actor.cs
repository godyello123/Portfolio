using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Global;

namespace PlayServer
{
    public class CActor
    {
        protected _ActorData m_ActorData = new _ActorData();

        public CActor()
        {
            SetActorSkinID(DefaultPlayerTable.Instance.SkinID());
            SetActorTID(DefaultPlayerTable.Instance.ActorID());
            SetActorDefaultSkill(DefaultPlayerTable.Instance.CopyDefaultSkills());
        }

        public void SetActorTID(int _tid)
        {
            m_ActorData.m_TableID = _tid.ToString();
        }

        public void InitActorData(long guid, string name)
        {
            SetActorGUID(guid);
            SetActorName(name);
        }

        public void SetActorGUID(long guid)
        {
            m_ActorData.m_GUID = guid;
        }

        public void SetActorName(string name)
        {
            m_ActorData.m_name = name;
        }

        public _ActorData GetActorData()
        {
            return m_ActorData;
        }

        public void SetActorData(_ActorData actorData)
        {
            m_ActorData = actorData;
        }

        public void SetActorSkinID(string skinID)
        {
            m_ActorData.m_SkinID = skinID;
        }

        public void SetActorDefaultSkill(List<int> skills)
        {
            //m_ActorData.m_DefaultSkill.Clear();
            //foreach (var iter in skills)
            //    m_ActorData.m_DefaultSkill.Add(iter);
        }

        public void SetActorAbils(List<_AbilData> abils)
        {
            m_ActorData.m_AbiliList.Clear();
            foreach (var iter in abils)
                m_ActorData.m_AbiliList.Add(new _AbilData(iter));
        }

    }
}
