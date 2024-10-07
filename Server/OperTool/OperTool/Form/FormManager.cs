using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Global;
using MongoDB.Driver.Core.Servers;
using SCommon;

namespace OperTool
{
    public class IWorker
    {
        public virtual void Excute() { }
    }


    public class FormManager : SSingleton<FormManager>
    {
        private Form1 m_MainForm;
        private Form2 m_LoginForm;

        private eServerType m_ServerType = eServerType.Max;
        private string m_ID = string.Empty;
        private string m_PW = string.Empty;

        public eServerType ConnectServer { get { return m_ServerType; } }
        public string ID { get { return m_ID; } }
        public string PW { get { return m_PW; } }

        public Form1 MainForm { get { return m_MainForm; } }

        public void Start()
        {
            CTableLoader.Init();

            m_MainForm = new Form1();
            m_MainForm.Init();
            m_MainForm.FormClosing += new FormClosingEventHandler(OnFormClosed);
            m_MainForm.Hide();

            IntPtr mainFormHandle = m_MainForm.Handle;

            m_LoginForm = new Form2();
            m_LoginForm.FormClosing += new FormClosingEventHandler(OnFormClosed);
            m_LoginForm.Init();
            m_LoginForm.Show();

            IntPtr loginFormHandle = m_LoginForm.Handle;
            CNetManager.Instance.Start();
        }

        public void Stop()
        {
            MongoDBManager.Instance.Stop();
            CNetManager.Instance.Stop();
            Application.Exit();
        }

        public void ShowMainForm()
        {
            if (m_MainForm.InvokeRequired)
            {
                m_MainForm.Invoke(new Action(() =>
                {
                    m_MainForm.Show();
                    m_LoginForm.Hide();

                    m_MainForm.SetMainFormServerState(m_LoginForm.GetServerType(), m_LoginForm.GetLoginID(), CNetManager.Instance.m_ServerSession.Connected);

                    m_ServerType = m_LoginForm.GetServerType();
                    m_ID = m_LoginForm.GetLoginID();
                    m_PW = m_LoginForm.GetLoginPW();

                    if (m_MainForm.WindowState == FormWindowState.Minimized)
                        m_MainForm.WindowState = FormWindowState.Normal;
                }));
            }
            else
            {
                m_MainForm.Show();
                m_LoginForm.Hide();

                if (m_MainForm.WindowState == FormWindowState.Minimized)
                    m_MainForm.WindowState = FormWindowState.Normal;
            }

            OperLogConnectRecord record = OperLogConnectTable.Instance.Find(m_LoginForm.GetServerType().ToString());
            if (record == null)
                return;

            MongoDBManager.Instance.Start(record.LogConnection, record.DBName);
        }

        public void ShowLoginForm()
        {
            if (m_LoginForm.InvokeRequired)
            {
                m_LoginForm.Invoke(new Action(() => 
                {
                    m_LoginForm.Show();
                    m_MainForm.Hide();

                    if (m_LoginForm.WindowState == FormWindowState.Minimized)
                        m_LoginForm.WindowState = FormWindowState.Normal;

                }));
            }
            else
            {
                m_LoginForm.Show();
                m_MainForm.Hide();

                if (m_LoginForm.WindowState == FormWindowState.Minimized)
                    m_LoginForm.WindowState = FormWindowState.Normal;
            }
        }

        private void OnFormClosed(object sender, FormClosingEventArgs e)
        {
            // 모든 폼이 닫히면 애플리케이션이 종료됨
            if (Application.OpenForms.Count == 0)
            {
                Application.Exit();
            }
        }

        public void InsertMainFormWork(IWorker work)
        {
            m_MainForm.InsertWork(work);
        }
    }
}
