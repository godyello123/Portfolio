using Global;
using MongoDB.Driver.Core.Operations;
using OperTool.Panel;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperTool
{
    public class PanelManager : SSingleton<PanelManager>
    {
        private Form1 m_Owner;
        
        private Dictionary<ePanelType, PannelBase> m_Panels = new Dictionary<ePanelType, PannelBase>();
        
        private ePanelType m_CurPanelType = ePanelType.Max;

        public Form1 Owner { get { return m_Owner; } }

        public void Init(Form1 owner)
        {
            m_Owner = owner;
            InitPanel();
        }

        public PannelBase Find(ePanelType type)
        {
            if (m_Panels.TryGetValue(type, out var retval))
                return retval;

            return null;
        }

        private void InitPanel()
        {
            m_Panels[ePanelType.HOME]               = CreatePanel<Panel_Home>(ePanelType.HOME);
            m_Panels[ePanelType.USER_INFOMATION]    = CreatePanel<Panel_User_Infomation>(ePanelType.USER_INFOMATION);
            m_Panels[ePanelType.USER_POST]          = CreatePanel<Panel_User_Post>(ePanelType.USER_POST);
            m_Panels[ePanelType.USER_RECEIPT]       = CreatePanel<Panel_User_Receipt>(ePanelType.USER_RECEIPT);
            m_Panels[ePanelType.USER_ITEM]          = CreatePanel<Panel_User_Item>(ePanelType.USER_ITEM);
            m_Panels[ePanelType.USER_CONTENTS]      = CreatePanel<Panel_User_Contents>(ePanelType.USER_CONTENTS);
            m_Panels[ePanelType.SYSTEM_POST_VIEW]   = CreatePanel<Panel_SystemPost>(ePanelType.SYSTEM_POST_VIEW);
            m_Panels[ePanelType.SYSTEM_POST_SEND]   = CreatePanel<Panel_SystemPost_Send>(ePanelType.SYSTEM_POST_SEND);
            m_Panels[ePanelType.SYSTEM_NOTICE]      = CreatePanel<Panel_System_Notice>(ePanelType.SYSTEM_NOTICE);
            m_Panels[ePanelType.SYSTEM_BLOCKUSER]   = CreatePanel<Panel_BlockList>(ePanelType.SYSTEM_BLOCKUSER);
            m_Panels[ePanelType.SYSTEM_WHITEUSER]   = CreatePanel<Panel_WhiteList>(ePanelType.SYSTEM_WHITEUSER);
            m_Panels[ePanelType.SYSTEM_COUPON]      = CreatePanel<Panel_System_Coupon>(ePanelType.SYSTEM_COUPON);
            m_Panels[ePanelType.SERVER]             = CreatePanel<Panel_Server>(ePanelType.SERVER);
            m_Panels[ePanelType.GAME_LOG]           = CreatePanel<Panel_GameLog>(ePanelType.GAME_LOG);

            m_CurPanelType = ePanelType.HOME;
        }

        private T CreatePanel<T>(ePanelType type) where T : PannelBase, new()
        {
            T retVal = new T();
            retVal.Init(type);
            m_Owner.Controls.Add(retVal);
            return retVal;
        }

        public PannelBase ChangePanel(ePanelType panelType)
        {
            if (m_Panels.TryGetValue(m_CurPanelType, out var prev))
                prev.Visible = false;

            if (m_Panels.TryGetValue(panelType, out var panel))
            {
                panel.Visible = true;
                m_CurPanelType = panelType;

                return panel;
            }
           
            return null;
        }
    }
}
