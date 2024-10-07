﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OperTool.Panel
{
    public partial class Panel_BulkPostSend : PannelBase
    {
        public Panel_BulkPostSend()
        {
            InitializeComponent();
        }

        public override void Init(ePanelType panel_type, eLanguageType type = eLanguageType.Eng)
        {
            base.Init(panel_type);
        }

        private void SetLabalText()
        {
            sLabel_post_end.Text = "End";
            sLabel_post_start.Text = "Start";
            sLabel_post_title.Text = "Title";
            sLabel_post_msg.Text = "Msg";
            sLabel_post_coins.Text = "Coins";
            sLabel_post_items.Text = "Items";
        }
    }
}
