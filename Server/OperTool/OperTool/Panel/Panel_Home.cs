using System;
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
    public partial class Panel_Home : PannelBase
    {
        public Panel_Home()
        {
            InitializeComponent();
        }

        public override void Init(ePanelType panel_type, eLanguageType type = eLanguageType.Eng)
        {
            base.Init(panel_type);

            this.VisibleChanged += new System.EventHandler(this.OnVisible);
        }

        public void OnVisible(object sender, EventArgs args)
        {
        }

        private void metroToolTip1_Popup(object sender, PopupEventArgs e)
        {

        }
    }
}
