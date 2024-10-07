using System;
using System.IO;
using System.IO.Pipes;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace LogServer
{
    public partial class SPipeServer
    {
        private void SetupHandler()
        {
            AddHandler("close", new PipeHandler(OnClose));
        }

        public void OnClose()
        {
            SPipeServer.Instance.Stop();
            CNetManager.Instance.Form.Stop = true;
        }

    }

}
