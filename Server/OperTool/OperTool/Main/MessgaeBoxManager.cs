using Amazon.Runtime.Internal.Auth;
using Amazon.SecurityToken.Model.Internal.MarshallTransformations;
using Global;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;

namespace OperTool
{
    public class CMessageBoxManager : SSingleton<CMessageBoxManager>
    {
        public void Error(IWin32Window owner, eMessageBoxMessage msg)
        {
            MetroMessageBox.Show(owner, OperStringTalbe.Instance.String(msg.ToString()), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void Error(IWin32Window owner, Packet_Result.Result msg)
        {
            MetroMessageBox.Show(owner, OperStringTalbe.Instance.String(msg.ToString()), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public void Error(IWin32Window owner, string msg)
        {
            MetroMessageBox.Show(owner, msg, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void Info(IWin32Window owner, eMessageBoxMessage msg)
        {
            MetroMessageBox.Show(owner, OperStringTalbe.Instance.String(msg.ToString()), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void Info(IWin32Window owner, Packet_Result.Result msg)
        {
            MetroMessageBox.Show(owner, OperStringTalbe.Instance.String(msg.ToString()), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void Info(IWin32Window owner, string msg)
        {
            MetroMessageBox.Show(owner, msg, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void Warning(IWin32Window owner, eMessageBoxMessage msg)
        {
            MetroMessageBox.Show(owner, OperStringTalbe.Instance.String(msg.ToString()), "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public void Warning(IWin32Window owner, Packet_Result.Result msg)
        {
            MetroMessageBox.Show(owner, OperStringTalbe.Instance.String(msg.ToString()), "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public void Warning(IWin32Window owner, string msg)
        {
            MetroMessageBox.Show(owner, msg, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        
        //question is yes or no
        public bool Question(IWin32Window owner, eMessageBoxMessage msg)
        {
            return MetroMessageBox.Show(owner, OperStringTalbe.Instance.String(msg.ToString()), "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        public bool Question(IWin32Window owner, eMessageBoxMessage msg, string desc)
        {
            return MetroMessageBox.Show(owner, OperStringTalbe.Instance.String(msg.ToString()) + desc, "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
        }
    }
}
