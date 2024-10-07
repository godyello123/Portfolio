using Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OperTool.Controls
{
    public partial class CustomGridView : MetroFramework.Controls.MetroGrid
    {
        public CustomGridView()
        {
            InitializeComponent();
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            RowHeadersVisible = false;
            //ColumnHeadersVisible = false;
            ReadOnly = true;
        }

        public void SetHeader<T>()
        {
            Columns.Clear();

            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var it in fields)
                Columns.Add(it.Name, OperStringTalbe.Instance.Kor(it.Name));
        }

        public void SetData<T>(T data)
        {
            Columns.Clear();
            SetHeader<T>();

            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            int rowidx = Rows.Add();
            foreach (var field in fields)
            {
                object value = field.GetValue(data);
                this[field.Name, rowidx].Value = value.ToString();
            }
        }

        public void SetData<T>(List<T> datas)
        {
            this.Columns.Clear();
            SetHeader<T>();

            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var iter in datas)
            {
                int rowidx = Rows.Add();
                foreach (var field in fields)
                {
                    object value = field.GetValue(iter);
                    this[field.Name, rowidx].Value = value.ToString();
                }
            }
        }

        public void AddData<T>(T data)
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            int rowidx = Rows.Add();
            foreach (var field in fields)
            {
                object value = field.GetValue(data);
                this[field.Name, rowidx].Value = value.ToString();
            }
        }

        public void AddData<T>(List<T> datas)
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach(var iter in datas)
            {
                int rowidx = Rows.Add();

                foreach (var field in fields)
                {
                    object value = field.GetValue(iter);
                    this[field.Name, rowidx].Value = value.ToString();
                }
            }
        }

        public string GetData(string columnName)
        {
            var row = CurrentRow;
            if (row == null)
                return "";

            if (Columns.Contains(columnName))
                return this[columnName, row.Index].Value.ToString();

            return "";
        }

        public T GetData<T>(string columnName)
        {
            var row = CurrentRow;
            if (row == null)
                return default(T);

            if (Columns.Contains(columnName))
                return (T)Convert.ChangeType(this[columnName, row.Index].Value, typeof(T));

            return default(T);
        }

        public string GetData(string columnName, int rowIndex)
        {
            if (rowIndex < 0 || rowIndex > Rows.Count)
                return string.Empty;

            if (Columns.Contains(columnName))
                return this[columnName, rowIndex].Value.ToString();

            return string.Empty;
        }

        public T GetData<T>(string columnName, int rowIndex)
        {
            if (rowIndex < 0 || rowIndex > Rows.Count)
                return default(T);

            if (Columns.Contains(columnName))
                return (T)Convert.ChangeType(this[columnName, rowIndex].Value, typeof(T));

            return default(T);
        }

        public void RemoveRow()
        {
            var row = CurrentRow;
            if(row != null)
                Rows.Remove(row);
        }

        public void RemoveRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex > Rows.Count)
                return;

            Rows.RemoveAt(rowIndex);
        }

        public int FindRow(string columnName, string findVal)
        {
            this.ClearSelection();

            int rowCnt = Rows.Count;
            for (int i = 0; i < rowCnt; i++)
            {
                string value = this[columnName, i].Value.ToString();
                if (value == findVal)
                {
                    Rows[i].Selected = true;
                    return i;
                }

            }

            return -1;
        }
    }
}
