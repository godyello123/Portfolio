using Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OperTool.Controls
{
    public partial class SDateTimePicker : DateTimePicker
    {
        public SDateTimePicker()
        {
            InitializeComponent();
            CustomFormat = "yyyy-MM-dd HH:mm";
            Format = DateTimePickerFormat.Custom;
        }

        public void Init(DateTime start, DateTime end)
        {
            MaxDate = start;
            MinDate = end;
        }
    }
}
