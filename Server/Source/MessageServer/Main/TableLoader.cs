using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Global;
using SCommon;

namespace MessageServer
{
    class CTableLoader
    {
        private static string m_CurrentPath = string.Empty;

        public static bool Init()
        {
            m_CurrentPath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "Table");

            if (!LoadTable())
                return false;

            Prepare();

            return true;
        }

        private static bool LoadTable()
        {
            StageTable.Instance.Load(LoadFile("StageTable.csv"));
            ActorTable.Instance.Load(LoadFile("ActorTable.csv"));
            DefineTable.Instance.Load(LoadFile("DefineTable.csv"));

            return true;
        }

        private static void Prepare()
        {
            StageTable.Instance.Prepare();
            ActorTable.Instance.Prepare();
            DefineTable.Instance.Prepare();
        }

        private static string LoadFile(string tableName)
        {
            string path = System.IO.Path.Combine(m_CurrentPath, tableName);
            using (var reader = new StreamReader(path))
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                var text = reader.ReadToEnd();
                return text;
            }
        }
    }
}
