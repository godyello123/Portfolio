using Global;
using MongoDB.Bson.Serialization.Conventions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperTool
{
    public static class OperDefine
    {
        public static readonly Size PanelSize = new Size(1043, 550);
        public static readonly Point PanelLocation = new Point(14, 90);
        public static readonly List<int> RewardMultipleValue = new List<int> { 1, 10, 100 };
        public static readonly List<string> RewardMultipleValueStr = new List<string> { "1", "10", "100" };
        public static readonly DateTime MaxTime = CDefine.MaxTime;
        public static readonly DateTime MinTime = new DateTime(1970, 01, 01);
        public static readonly string Zero = "0";
        

        public static Int64 GeneraterGUID()
        {
            var b_guid = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(b_guid, 0);
        }
    }

    public static class OperHelper
    {
        public static List<string> EnumToStringList<T>() where T : Enum
        {
            var retList = new List<string>();
            foreach(var iter in Enum.GetValues(typeof(T)))
            {
                var itString = iter.ToString();
                if (itString.Equals("Max"))
                    continue;

                retList.Add(iter.ToString());
            }

            return retList;
        }

        public static List<string> ConvertStringList<T>(List<T> param)
        {
            var retList = new List<string>();
            foreach (var iter in param)
                retList.Add(iter.ToString());

            return retList;
        }
    }

    

    public enum ePanelType
    {
        HOME,
        USER_INFOMATION,
        USER_POST,
        USER_RECEIPT,
        USER_ITEM,
        USER_CONTENTS,

        SYSTEM_POST_VIEW,
        SYSTEM_POST_SEND,
        SYSTEM_NOTICE,

        SYSTEM_SCHEDULE,
        SYSTEM_BLOCKUSER,
        SYSTEM_WHITEUSER,
        SYSTEM_COUPON,

        SERVER,
        GAME_LOG,

        Max
    }

    public enum eUserSearchType
    {
        DeviceID,
        UID,
        Name,

        Max
    }

    public enum eMessageBoxMessage
    {
        DataCheck,
        NotFoundData,
        Fail,
        Retry,
        SystemError,
        ServerSelectError,
        IDorPwCheck,            
        DateCheck,              
        Success,
        IPorPortCheck,
        AuthorityError,         
        AlreadyStart,
        PoseRecevierCheck,
        IsNotBlock,
        IsNotWhite,
        IsWhite,
        DisConnectServer,
        UserIDCheck,
        PostCheck,
        ServiceTypeChange,
        LogTypeSelect,
        NoneSelectedItem,


        Max,
    }

    public enum eServerType
    {
        Dev,
        Local,
        Custom,

        Max
    }

    public enum eOperRole
    {
        Owner,
        Editor,
        Viewer,
        Max
    }

    public enum eBlockPeriod
    {
        Day,
        Week,
        Monty,
        Year,
        Permanent,

        Max
    }

    public enum eServerOpenType
    {
        Open,
        Maintenance,

        Max
    }

    public enum eLanguageType
    {
        Kor,
        Eng,

        Max
    }
}
