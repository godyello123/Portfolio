using Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OperTool.Controls
{
    public partial class STabControl : MetroFramework.Controls.MetroTabControl
    {
        public STabControl()
        {
            InitializeComponent();
        }

        public void AddTab(string name, eLanguageType type = eLanguageType.Eng)
        {
            this.AddTab(OperStringTalbe.Instance.String(name, type));
        }
        
    }
}
