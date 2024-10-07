using MetroFramework.Controls;
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
    public partial class STextBox : MetroTextBox
    {
        private bool m_OnlyNumber = false;

        public STextBox()
        {
            InitializeComponent();
        }

        public void Init(bool onlyNumber)
        {
            m_OnlyNumber = onlyNumber;
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
        }

        public string GetValue()
        {
            return Text;
        }

        public T GetValue<T>()
        {
            if (string.IsNullOrEmpty(Text))
                return default(T);

            return (T)Convert.ChangeType(Text, typeof(T));
        }

        public void SetText(string text)
        {
            Text = text;
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if(m_OnlyNumber)
            {
                if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
                {
                    e.Handled = true;
                }
            }
        }

    }
}
