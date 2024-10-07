using Amazon.Runtime.Internal.Util;
using Global;
using OperTool.Panel;
using Packet_O2M;
using SDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperTool.Form
{
    //======================PANEL_USER_INFOMATION======================
    public class WorkUserInfoPanel_SetData : IWorker
    {
        public _UserData m_UserData = new _UserData();
        public List<_GachaData> m_Gachas = new List<_GachaData>();
        public List<_AssetData> m_AssetData = new List<_AssetData>();
        public List<_StageData> m_Stages = new List<_StageData>();
        public List<_ReceiptData> m_Receipts = new List<_ReceiptData>();
        public bool m_IsBlock = false;
        public bool m_IsWhite = false;

        public WorkUserInfoPanel_SetData(_UserData userData, List<_GachaData> gachas, List<_AssetData> coins, List<_StageData> stages, List<_ReceiptData> receipts, bool isBlock, bool isWhite)
        {
            m_UserData = userData;
            m_Gachas = gachas;
            m_AssetData = coins;
            m_Stages = stages;
            m_Receipts = receipts;
            m_IsBlock = isBlock;
            m_IsWhite = isWhite;
        }

        public override void Excute()
        {
            if (PanelManager.Instance.Find(ePanelType.USER_INFOMATION) is Panel_User_Infomation panel)
            {
                if (panel == null)
                {
                    CMessageBoxManager.Instance.Warning(PanelManager.Instance.Owner, eMessageBoxMessage.SystemError);
                    return;
                }

                panel.SetUserInfoData(m_UserData, m_Gachas, m_AssetData, m_Stages, m_Receipts, m_IsBlock, m_IsWhite);
            }
        }
    }

    //======================PANEL_SYTEM_POST======================
    public class WorkSytemPostPanel_SetPostData : IWorker
    {
        public List<_PostData> m_SystemPosts = new List<_PostData>();

        public WorkSytemPostPanel_SetPostData(List<_PostData> systemPosts)
        {
            m_SystemPosts = systemPosts;
        }

        public override void Excute()
        {
            if (PanelManager.Instance.Find(ePanelType.SYSTEM_POST_VIEW) is Panel_SystemPost panel)
            {
                if (panel == null)
                {
                    CMessageBoxManager.Instance.Warning(PanelManager.Instance.Owner, eMessageBoxMessage.SystemError);
                    return;
                }

                panel.SetSystemPostData(m_SystemPosts);
            }
        }
    }

    public class WorkSystemPostPanel_PostSend : IWorker
    {
        private _PostData m_PostData = new _PostData();

        public WorkSystemPostPanel_PostSend(_PostData postData)
        {
            m_PostData = postData;
        }

        public override void Excute()
        {
            if (PanelManager.Instance.Find(ePanelType.SYSTEM_POST_SEND) is Panel_SystemPost_Send panel)
            {
                if (panel == null)
                {
                    CMessageBoxManager.Instance.Warning(PanelManager.Instance.Owner, eMessageBoxMessage.SystemError);
                    return;
                }

                panel.AfterExcutePostSend(m_PostData);
            }
        }
    }

    //====================== SYSTEM_NOTICE ======================
    public class WorkSystemNotice_Load : IWorker
    {
        private List<_NoticeData> m_Notices = new List<_NoticeData>();

        public WorkSystemNotice_Load(List<_NoticeData> notices)
        {
            m_Notices = notices;
        }

        public override void Excute()
        {
            if (PanelManager.Instance.Find(ePanelType.SYSTEM_NOTICE) is Panel_System_Notice panel)
            {
                if (panel == null)
                {
                    CMessageBoxManager.Instance.Warning(PanelManager.Instance.Owner, eMessageBoxMessage.SystemError);
                    return;
                }

                panel.AfterExcuteNoticeLoad(m_Notices);
            }
        }
    }

    public class WorkSystemNotice_Update : IWorker
    {
        private _NoticeData m_Notice = new _NoticeData();

        public WorkSystemNotice_Update(_NoticeData notice)
        {
            m_Notice = notice;
        }

        public override void Excute()
        {
            if (PanelManager.Instance.Find(ePanelType.SYSTEM_NOTICE) is Panel_System_Notice panel)
            {
                if (panel == null)
                {
                    CMessageBoxManager.Instance.Warning(PanelManager.Instance.Owner, eMessageBoxMessage.SystemError);
                    return;
                }

                panel.AfterExcuteNoticeUpdate(m_Notice);
            }
        }
    }

    //======================= USER_POST ==============================
    public class WorkUserPost_Load : IWorker
    {
        private List<_PostData> m_Posts = new List<_PostData>();

        public WorkUserPost_Load(List<_PostData> posts)
        {
            m_Posts = posts;
        }

        public override void Excute()
        {
            if (PanelManager.Instance.Find(ePanelType.USER_POST) is Panel_User_Post panel)
            {
                if (panel == null)
                {
                    CMessageBoxManager.Instance.Warning(PanelManager.Instance.Owner, eMessageBoxMessage.SystemError);
                    return;
                }

                panel.AfterExcutePostLoad(m_Posts);
            }
        }
    }

    public class WorkUserPost_Insert : IWorker
    {
        private _PostData m_Post = new _PostData();

        public WorkUserPost_Insert(_PostData post)
        {
            m_Post = post;
        }

        public override void Excute()
        {
            if (PanelManager.Instance.Find(ePanelType.USER_POST) is Panel_User_Post panel)
            {
                if (panel == null)
                {
                    CMessageBoxManager.Instance.Warning(PanelManager.Instance.Owner, eMessageBoxMessage.SystemError);
                    return;
                }

                panel.AfterExcutePostSend(m_Post);
            }
        }
    }

    //==================== BLOCK_USER ===================
    public class WorkBlockUser_Load : IWorker
    {
        private List<_BlockUser> m_Blocks = new List<_BlockUser>();
        public WorkBlockUser_Load(List<_BlockUser> blocks)
        {
            m_Blocks = blocks;
        }

        public override void Excute()
        {
            if (PanelManager.Instance.Find(ePanelType.SYSTEM_BLOCKUSER) is Panel_BlockList panel)
            {
                if (panel == null)
                {
                    CMessageBoxManager.Instance.Warning(PanelManager.Instance.Owner, eMessageBoxMessage.SystemError);
                    return;
                }

                panel.AfterExcuteBlockUserLoad(m_Blocks);
            }
        }
    }

    // ================== COUPON ===================
    public class WorkCoupon_Load : IWorker
    {
        private List<_CouponData> m_Coupons = new List<_CouponData>();

        public WorkCoupon_Load(List<_CouponData> coupons)
        {
            m_Coupons = coupons;
        }

        public override void Excute()
        {
            if (PanelManager.Instance.Find(ePanelType.SYSTEM_COUPON) is Panel_System_Coupon panel)
            {
                if (panel == null)
                {
                    CMessageBoxManager.Instance.Warning(PanelManager.Instance.Owner, eMessageBoxMessage.SystemError);
                    return;
                }

                panel.AfterExcuteCouponLoad(m_Coupons);
            }
        }
    }

    public class WorkCoupon_Create : IWorker
    {
        private _CouponData m_Coupon = new _CouponData();

        public WorkCoupon_Create(_CouponData coupon)
        {
            m_Coupon = coupon;
        }

        public override void Excute() 
        {
            if (PanelManager.Instance.Find(ePanelType.SYSTEM_COUPON) is Panel_System_Coupon panel)
            {
                if (panel == null)
                {
                    CMessageBoxManager.Instance.Warning(PanelManager.Instance.Owner, eMessageBoxMessage.SystemError);
                    return;
                }

                panel.AfterExcuteCouponCreate(m_Coupon);
            }
        }
    }

    public class WorkDisConnectServer : IWorker
    {

        public WorkDisConnectServer()
        {

        }

        public override void Excute()
        {
            FormManager.Instance.MainForm.DisConnectServer();
        }
    }

    public class WorkUserContent_GrowthLevelLoad : IWorker
    {
        public List<_LevelData> m_Datas = new List<_LevelData>();

        public WorkUserContent_GrowthLevelLoad(List<_LevelData> datas)
        {
            m_Datas = datas;
        }

        public override void Excute()
        {
            if (PanelManager.Instance.Find(ePanelType.USER_CONTENTS) is Panel_User_Contents panel)
            {
                if (panel == null)
                {
                    CMessageBoxManager.Instance.Warning(PanelManager.Instance.Owner, eMessageBoxMessage.SystemError);
                    return;
                }

                panel.AfterExcuteGrowthLevelLoad(m_Datas);
            }
        }
    }

    public class WorkUserContent_GrowttGoldLoad : IWorker
    {
        public List<_LevelData> m_Datas = new List<_LevelData>();

        public WorkUserContent_GrowttGoldLoad(List<_LevelData> datas)
        {
            m_Datas = datas;
        }

        public override void Excute()
        {
            if (PanelManager.Instance.Find(ePanelType.USER_CONTENTS) is Panel_User_Contents panel)
            {
                if (panel == null)
                {
                    CMessageBoxManager.Instance.Warning(PanelManager.Instance.Owner, eMessageBoxMessage.SystemError);
                    return;
                }

                panel.AfterExcuteGrowthGoldLoad(m_Datas);
            }
        }
    }

    public class WorkServerState_ServerStateLoad : IWorker
    {
        private bool m_Open = false;
        private List<CServerInfo> m_Datas = new List<CServerInfo>();

        public WorkServerState_ServerStateLoad(List<CServerInfo> datas, bool bopen)
        {
            m_Datas = datas;
            m_Open = bopen;
        }

        public override void Excute()
        {
            if (PanelManager.Instance.Find(ePanelType.SERVER) is Panel_Server panel)
            {
                if (panel == null)
                {
                    CMessageBoxManager.Instance.Warning(PanelManager.Instance.Owner, eMessageBoxMessage.SystemError);
                    return;
                }

                panel.AfterExcuteServerStateLoad(m_Datas, m_Open);
            }
        }
    }


    public class WorkGameLog_GameLogLoad : IWorker
    {
        private List<LogBson> m_Datas = new List<LogBson>();

        public WorkGameLog_GameLogLoad(List<LogBson> datas)
        {
            m_Datas = datas;
        }

        public override void Excute()
        {
            if (PanelManager.Instance.Find(ePanelType.GAME_LOG) is Panel_GameLog panel)
            {
                if (panel == null)
                {
                    CMessageBoxManager.Instance.Warning(PanelManager.Instance.Owner, eMessageBoxMessage.SystemError);
                    return;
                }

                panel.SetGameLogData(m_Datas);
            }
        }
    }
}
