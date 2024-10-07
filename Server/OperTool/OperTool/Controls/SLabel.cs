﻿using Global;
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
    public partial class SLabel : MetroFramework.Controls.MetroLabel
    {
        public SLabel()
        {
            InitializeComponent();
        }

        public void Init(string name, eLanguageType type = eLanguageType.Eng)
        {
            this.Text = OperStringTalbe.Instance.String(name,type);
        }
    }
}
