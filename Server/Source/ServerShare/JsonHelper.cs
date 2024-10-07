
using System;
using System.Collections.Generic;
using SCommon;
using Global;
using Newtonsoft;
using Newtonsoft.Json.Linq;

namespace Global
{
    public static class JsonHelper
    {
        //static public string ToJson(List<_RaidUserInfo> data)
        //{
        //    var jarr = new Newtonsoft.Json.Linq.JArray();
        //    foreach (var raidUserInfo in data)
        //    {
        //        var jobj = new Newtonsoft.Json.Linq.JObject();

        //        jobj["cid"] = raidUserInfo.m_UserData.m_AccountID;
        //        jobj["lv"] = raidUserInfo.m_UserData.m_Level;
        //        jobj["sid"] = raidUserInfo.m_UserData.m_ServerID;
        //        jobj["pow"] = raidUserInfo.m_Power;

        //        jarr.Add(jobj);
        //    }

        //    return jarr.Count < 1 ? "" : SJson.ObjectToJson(jarr);
        //}
    }
}
