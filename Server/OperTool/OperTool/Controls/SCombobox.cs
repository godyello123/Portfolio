using Global;
using Newtonsoft.Json.Linq;
using OperTool.Form;
using SCommon;
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
    public partial class SCombobox : MetroFramework.Controls.MetroComboBox
    {
        public class SComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }
        }

        private BindingList<object> typeList = new BindingList<object>();

        public SCombobox()
        {
            InitializeComponent();
        }

        public void Init(List<string> arr, eLanguageType type = eLanguageType.Eng)
        {
            foreach(var str in arr)
            {
                string textStr = OperStringTalbe.Instance.String(str, type);
                typeList.Add(new SComboboxItem{ Text = $"{textStr}", Value = $"{str}" });
            }

            this.DataSource = typeList;
            this.DisplayMember = "Text";
            this.ValueMember = "Value";
        }

        public void SetData(List<string> texts, List<string> values)
        {
            if (texts.Count != values.Count)
                return;

            typeList.Clear();

            int cnt = texts.Count;
            for(int i = 0; i <cnt; ++i)
                typeList.Add(new { Text = $"{texts[i]}", Value = $"{values[i]}" });

            DataSource = typeList;
            this.DisplayMember = "Text";
            this.ValueMember = "Value";
        }

        public string GetValue()
        {
            if (this.SelectedItem == null)
                return string.Empty;

            SComboboxItem item = SelectedItem as SComboboxItem;
            return item.Value.ToString();
        }

        public T GetValue<T>()
        {
            if (this.SelectedItem == null)
                return default(T);

            SComboboxItem item = SelectedItem as SComboboxItem;
            return (T)Convert.ChangeType(item.Value, typeof(T));
        }

    }
}
