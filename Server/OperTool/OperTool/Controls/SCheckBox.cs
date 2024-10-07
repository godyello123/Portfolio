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
    public partial class SCheckBox : MetroFramework.Controls.MetroCheckBox
    {
        public SCheckBox()
        {
            InitializeComponent();
        }

        public void Init(string text, eLanguageType type = eLanguageType.Eng)
        {
            this.Text = OperStringTalbe.Instance.String(text, type);
        }
    }
}
