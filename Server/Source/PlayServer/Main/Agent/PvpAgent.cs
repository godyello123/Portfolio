using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCommon;
using Global;

namespace PlayServer
{
    public  class CPvpAgent
    {
        private CUser m_Owner;
        public bool IsPlaying { get; set; } = false;

        public CPvpAgent(CUser user)
        {
            m_Owner = user;
        }

        public Packet_Result.Result ReqPvpStart(long sessionKey)
        {
            //todo : pvp stage check
            
            //todo : pvp condition check

            //todo : matching

            m_Owner.RefreshUserOverView();

            CNetManager.Instance.P2M_RequestPvpMatchStart(sessionKey, m_Owner.DeviceID);
            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqPvpEnd(long sessionKey, bool isWin)
        {
            //todo 보상 처리 임시임
            //todo : point
            CRewardInfo forClient = new CRewardInfo();
            CDBMerge dbtran = new CDBMerge();
            var pvp_point = m_Owner.AssetAgent.Find(CDefine.CoinType.Pvp_Point);
            if (pvp_point == null)
                return Packet_Result.Result.PacketError;

            long before = pvp_point.Count;
            if (isWin)
            {
                //todo : pvp win process
                forClient.Insert(CDefine.AssetType.Coin, "BlueDia", 1000);
                pvp_point.Count++;
            }
            else
            {
                //todo : pvp lose process
                pvp_point.Count = Math.Max(0, pvp_point.Count - 1);
            }

            forClient.UpdateRewardData(m_Owner, ref dbtran, true);
            dbtran.Merge();
            //todo : reward
            //todo : pvp rank? pvp rank report


            CNetManager.Instance.P2C_ReportAssetData(sessionKey, m_Owner.AssetAgent.GetList());
            CDBManager.Instance.QueryCharacterUpdateCoinList(m_Owner.DBGUID, sessionKey, m_Owner.UID, dbtran.m_UpdateCoinList);
            CNetManager.Instance.P2C_ResultPvpEnd(sessionKey, before, pvp_point.Count, isWin, forClient.GetList(), Packet_Result.Result.Success);

            return Packet_Result.Result.Success;
        }
    }
}
