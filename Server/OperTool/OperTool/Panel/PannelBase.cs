using Global;
using MetroFramework.Controls;
using OperTool.Form;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OperTool.Panel
{
    public partial class PannelBase : UserControl
    {
        protected ePanelType m_PanelType = ePanelType.Max;
        public Action m_ActionFunc = null;

        public PannelBase()
        {
            InitializeComponent();
        }

        public virtual void Init(ePanelType panel_type, eLanguageType language_type = eLanguageType.Eng)
        {
            m_PanelType = panel_type;
            this.Panel_Label.Text = panel_type.ToString();

            this.Size = OperDefine.PanelSize;
            this.Location = OperDefine.PanelLocation;
            this.Visible = false;
        }

        public virtual void SetData<T>(T data) { }
        public virtual void SetData<T>(List<T> datas) { }

        //public void SetGridViewHeader<T>(ref MetroGrid view)
        //{
        //    view.Columns.Clear();
        //    var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        //    foreach(var it in fields)
        //        view.Columns.Add(it.Name, it.Name);
        //}

        //public void SetGridViewData<T>(ref MetroGrid view, List<T> datas)
        //{
        //    view.Columns.Clear();
        //    SetGridViewHeader<T>(ref view);

        //    var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        //    int rowidx = 0;
        //    foreach (var iter in datas)
        //    {
        //        view.Rows.Add();
        //        foreach(var field in fields)
        //        {
        //            object value = field.GetValue(iter);
        //            view[field.Name, rowidx].Value = value.ToString(); 
        //        }

        //        rowidx++;
        //    }
        //}

        //public void SetGridViewData<T>(ref MetroGrid view, T data)
        //{
        //    view.Columns.Clear();
        //    SetGridViewHeader<T>(ref view);

        //    var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        //    int rowidx = 0;
        //    view.Rows.Add();
        //    foreach (var field in fields)
        //    {
        //        object value = field.GetValue(data);
        //        view[field.Name, rowidx].Value = value.ToString();
        //    }
        //}

        //public void AddGridViewData<T>(ref MetroGrid view, T data)
        //{
        //    var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        //    int rowidx = view.Rows.Add();
        //    foreach (var field in fields)
        //    {
        //        object value = field.GetValue(data);
        //        view[field.Name, rowidx].Value = value.ToString();
        //    }
        //}

        //public string GetGridViewData(MetroGrid view, string columName)
        //{
        //    var row = view.CurrentRow;
        //    if (row == null)
        //        return "";

        //    if (view.Columns.Contains(columName))
        //        return view[columName, row.Index].Value.ToString();

        //    return "";
        //}

        //public string GetGridViewData(MetroGrid view, string columnName, int rowIndex)
        //{
        //    if (rowIndex < 0 || rowIndex > view.Rows.Count)
        //        return "";

        //    if (view.Columns.Contains(columnName))
        //        return view[columnName, rowIndex].Value.ToString();

        //    return "";
        //}


        //public T GetGridViewData<T>(MetroGrid view, string columName)
        //{
        //    var row = view.CurrentRow;
        //    if (row == null)
        //        return default(T);

        //    if (view.Columns.Contains(columName))
        //        return (T)Convert.ChangeType(view[columName, row.Index].Value, typeof(T));

        //    return default(T);
        //}

        //public T GetGridViewData<T>(MetroGrid view, string columnName, int rowIndex)
        //{
        //    if (rowIndex < 0 || rowIndex > view.Rows.Count)
        //        return default(T);

        //    if (view.Columns.Contains(columnName))
        //        return (T)Convert.ChangeType(view[columnName, rowIndex].Value, typeof(T));

        //    return default(T);
        //}
    }
}
