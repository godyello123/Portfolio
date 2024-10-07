using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace OperTool.Panel
{
    public partial class Panel_User_Item : PannelBase
    {
        public Panel_User_Item()
        {
            InitializeComponent();
        }

        public override void Init(ePanelType panel_type, eLanguageType type = eLanguageType.Eng)
        {
            base.Init(panel_type);

            sLabel_user_uid.Init("UID");
            sLabel_user_deviceID.Init("DeviceID");
            sLabel_user_name.Init("Name");

            sTextBox_user_uid.Init(true);
            sTextBox_user_deviceid.Init(false);
            sTextBox_user_name.Init(false);
        }

        public void SetData(string deviceID, string name, long uid)
        {
            sTextBox_user_uid.Text = uid.ToString();
            sTextBox_user_deviceid.Text = deviceID;
            sTextBox_user_name.Text = name;
        }
    }
}
