using OperTool;
using OperTool.Panel;
using Packet_O2M;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OperTool.Form;

namespace OperTool
{
    public partial class CNetManager
    {
        private void SetupMsgHandler()
        {
            //X2X
            m_Handler.Add((ushort)Packet_X2X.Protocol.X2X_HeartBeat, new MsgHandlerDelegate(X2X_HeartBeat));
            m_Handler.Add((ushort)Packet_X2X.Protocol.X2X_HeartBeatAck, new MsgHandlerDelegate(X2X_HeartBeatAck));

            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultSearchUser, new MsgHandlerDelegate(M2O_ResultSearchUser));
            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultSystemPostLoad, new MsgHandlerDelegate(M2O_ResultSystemPostLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultSystemPostSend, new MsgHandlerDelegate(M2O_ResultSystemPostSend));
            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultNoticeLoad, new MsgHandlerDelegate(M2O_ResultNoticeLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultNoticeUpdate, new MsgHandlerDelegate(M2O_ResultNoticeUpdate));
            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultNoticeErase, new MsgHandlerDelegate(M2O_ResultNoticeErase));
            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultUserPostLoad, new MsgHandlerDelegate(M2O_ResultUserPostLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultUserPostSend, new MsgHandlerDelegate(M2O_ResultUserPostSend));
            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultBlockUserLoad, new MsgHandlerDelegate(M2O_ResultBlockUserLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultCouponLoad, new MsgHandlerDelegate(M2O_ResultCouponLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultCouponCreate, new MsgHandlerDelegate(M2O_ResultCouponCreate));
            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultUserGrowthLevelLoad, new MsgHandlerDelegate(M2O_ResultUserGrowthLevelLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultUserGrowthGoldLoad, new MsgHandlerDelegate(M2O_ResultUserGrowthGoldLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultUserGachaLoad, new MsgHandlerDelegate(M2O_ResultUserGachaLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultUserQuestLoad, new MsgHandlerDelegate(M2O_ResultUserQuestLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultUserRelicLoad, new MsgHandlerDelegate(M2O_ResultUserRelicLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultUserSkillLoad, new MsgHandlerDelegate(M2O_ResultUserSkillLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultUserShopLoad, new MsgHandlerDelegate(M2O_ResultUserShopLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultUserPassLoad, new MsgHandlerDelegate(M2O_ResultUserPassLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.M2O_ResultServerStateLoad, new MsgHandlerDelegate(M2O_ResultServerStateLoad));

            
        }
    }

    //X2X
    public partial class CNetManager
    {
        public void X2X_HeartBeat(long sessionKey, byte[] packet)
        {
            Write(sessionKey, new Packet_X2X.X2X_HeartBeatAck());
        }

        public void X2X_HeartBeatAck(long sessionKey, byte[] packet)
        {
        }


        //=================== O2M ========================
        public void M2O_ResultSearchUser(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.M2O_ResultSearchUser();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, eMessageBoxMessage.NotFoundData);
                return;
            }

            FormManager.Instance.InsertMainFormWork(new WorkUserInfoPanel_SetData(recvMsg.m_UserData, recvMsg.m_GachaData, recvMsg.m_Coins, recvMsg.m_Stages, recvMsg.m_Receipts,
                recvMsg.m_IsBlock, recvMsg.m_IsWhite));
        }

        public void M2O_ResultSystemPostLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.M2O_ResultSystemPostLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, eMessageBoxMessage.NotFoundData);
                return;
            }

            FormManager.Instance.InsertMainFormWork(new WorkSytemPostPanel_SetPostData(recvMsg.m_SystemPosts));
        }

        public void M2O_ResultSystemPostSend(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.M2O_ResultSystemPostSend();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);
                return;
            }

            CMessageBoxManager.Instance.Info(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);

            FormManager.Instance.InsertMainFormWork(new WorkSystemPostPanel_PostSend(recvMsg.m_PostData));
        }

        public void M2O_ResultNoticeLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.M2O_ResultNoticeLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);
                return;
            }

            FormManager.Instance.InsertMainFormWork(new WorkSystemNotice_Load(recvMsg.m_Notices));
        }

        public void M2O_ResultNoticeUpdate(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.M2O_ResultNoticeUpdate();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);
                return;
            }

            FormManager.Instance.InsertMainFormWork(new WorkSystemNotice_Update(recvMsg.m_NoticeData));
        }

        public void M2O_ResultNoticeErase(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.M2O_ResultNoticeErase();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;
            
            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);
                return;
            }

            CMessageBoxManager.Instance.Info(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);
        }

        public void M2O_ResultUserPostLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.M2O_ResultUserPostLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);
                return;
            }

            FormManager.Instance.InsertMainFormWork(new WorkUserPost_Load(recvMsg.m_Posts));
        }

        public void M2O_ResultUserPostSend(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.M2O_ResultUserPostSend();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);
                return;
            }

            FormManager.Instance.InsertMainFormWork(new WorkUserPost_Insert(recvMsg.m_PostData));
        }

        public void M2O_ResultBlockUserLoad(long sessionKey, byte[] packet) 
        {
            var recvMsg = new Packet_O2M.M2O_ResultBlockUserLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);
                return;
            }

            FormManager.Instance.InsertMainFormWork(new WorkBlockUser_Load(recvMsg.m_BlockUsers));
        }

        public void M2O_ResultCouponLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.M2O_ResultCouponLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);
                return;
            }

            FormManager.Instance.InsertMainFormWork(new WorkCoupon_Load(recvMsg.m_Coupons));
        }

        public void M2O_ResultCouponCreate(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.M2O_ResultCouponCreate();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);
                return;
            }

            FormManager.Instance.InsertMainFormWork(new WorkCoupon_Create(recvMsg.m_Coupon));
        }

        public void M2O_ResultUserGrowthLevelLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.M2O_ResultUserGrowthLevelLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);
                return;
            }

            FormManager.Instance.InsertMainFormWork(new WorkUserContent_GrowthLevelLoad(recvMsg.m_Datas));
        }

        public void M2O_ResultUserGrowthGoldLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.M2O_ResultUserGrowthGoldLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);
                return;
            }

            FormManager.Instance.InsertMainFormWork(new WorkUserContent_GrowttGoldLoad(recvMsg.m_Datas));
        }

        public void M2O_ResultUserGachaLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.M2O_ResultUserGachaLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);
                return;
            }
        }

        public void M2O_ResultUserQuestLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.M2O_ResultUserQuestLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);
                return;
            }
        }

        public void M2O_ResultUserRelicLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.M2O_ResultUserRelicLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);
                return;
            }
        }

        public void M2O_ResultUserSkillLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.M2O_ResultUserSkillLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);
                return;
            }
        }

        public void M2O_ResultUserShopLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.M2O_ResultUserShopLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);
                return;
            }
        }

        public void M2O_ResultUserPassLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.M2O_ResultUserPassLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);
                return;
            }
        }

        public void M2O_ResultServerStateLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.M2O_ResultServerStateLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
            {
                CMessageBoxManager.Instance.Warning(FormManager.Instance.MainForm, (Packet_Result.Result)recvMsg.m_Result);
                return;
            }

            FormManager.Instance.InsertMainFormWork(new WorkServerState_ServerStateLoad(recvMsg.m_Datas, recvMsg.m_Open));
        }
    }
}
