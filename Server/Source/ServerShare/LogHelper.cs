using System;
using System.Collections.Generic;
using SCommon;
using Global;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using SDB;
using System.Diagnostics.Eventing.Reader;


namespace Global
{
    public static class LogHelper
    {
        public static _LogBase Create(eLogType type)
        {
            var retval = new _LogBase();
            retval.type = (ushort)type;
            retval.time = DateTime.UtcNow;

            return retval;
        }

#if SERVER_ONLY
        //public static _LogBase MakeLog(eLogType type, _UserData userData, string log_str, PlayServer.CDBMerge dbtran, long cnt)
        //{
        //    var retval = Create(type);
        //    retval.cnt = cnt;
        //    retval.deviceid = userData.m_DeviceID;
        //    retval.uid = userData.m_UID;
        //    retval.log_str = log_str;
        //    if (dbtran != null)
        //    {
        //        retval.u_asset = ToJson(dbtran.m_UpdateItemList);
        //        retval.u_item = ToJson(dbtran.m_UpdateItemList);
        //    }

        //    return retval;
        //}

        public static LogBson MakeLogBson(eLogType type, _UserData userdata, string log_str, PlayServer.CDBMerge dbtran, long cnt)
        {
            var retval = new LogBson();
            retval.Type = (ushort)type;
            retval.Time = DateTime.UtcNow;
            retval.Count = (int)cnt;
            retval.DeviceID = userdata.m_DeviceID;
            retval.UID = userdata.m_UID;
            retval.LogStr = log_str;

            if (dbtran != null)
            {
                retval.Update_Coins = ToJson(dbtran.m_UpdateCoinList);
                retval.Update_Items = ToJson(dbtran.m_UpdateItemList);
            }

            return retval;
        }


        public static string ToJson(_LevelData data)
        {
            var obj = new JObject();
            obj.Add("id", data.m_TableID);
            obj.Add("val", data.m_UseCount);
            
            return SJson.ObjectToJson(obj);
        }

        public static string ToJson(_GachaData data)
        {
            var obj = new JObject();
            obj.Add("id", data.m_GroupID);
            obj.Add("lv", data.m_Level);
            obj.Add("exp", data.m_Exp);
            obj.Add("reward", data.m_Rewarded);

            return SJson.ObjectToJson(obj);
        }

        public static string ToJson(_RankReward data)
        {
            var obj = new JObject();
            obj.Add("type", (ushort)data.m_Type);
            obj.Add("val", data.m_RankValue);

            return SJson.ObjectToJson(obj);
        }

        public static string ToJson(_RelicData data)
        {
            var obj = new JObject();
            obj.Add("id", data.m_GroupID);
            obj.Add("val", data.m_Level);
            obj.Add("etc", data.m_BonusProb);

            return SJson.ObjectToJson(obj);
        }

        public static string ToJson(_Mission data)
        {
            if (data == null)
                return "";

            var obj = new JObject();
            obj.Add("id", data.ID);
            obj.Add("val", data.Val);
            obj.Add("state", (ushort)data.State);
            obj.Add("pass", data.PassRewarded);

            return SJson.ObjectToJson(obj);
        }

        public static string ToJson(List<_Mission> datas)
        {
            if (datas.Count < 1)
                return "";

            var jarr = new JArray();
            foreach(var iter in datas)
            {
                var obj = new JObject();
                obj.Add("id", iter.ID);
                obj.Add("val", iter.Val);
                obj.Add("state", (ushort)iter.State);
                obj.Add("pass", iter.PassRewarded);

                jarr.Add(obj);
            }

            return SJson.ObjectToJson(jarr);
        }

        public static string ToJson(List<_AssetData> datas)
        {
            if (datas.Count < 1)
                return "";

            var jarr = new JArray();

            foreach(var iter in datas)
            {
                var obj = new JObject();
                obj.Add("id", iter.TableID);
                obj.Add("val", iter.Count);

                jarr.Add(obj);
            }

            return SJson.ObjectToJson(jarr);
        }

        public static string ToJson(_ItemData data)
        {
            var obj = new JObject();
            obj.Add("id", data.TableID);
            obj.Add("uid", data.ItemID);
            obj.Add("lv", data.Level);
            obj.Add("cnt", data.Count);

            return SJson.ObjectToJson(obj);
        }

        public static string ToJson(List<_ItemData> datas)
        {
            if (datas.Count < 1)
                return "";

            var jarr = new JArray();

            foreach(var iter in datas)
            {
                var obj = new JObject();
                obj.Add("id", iter.TableID);
                obj.Add("uid", iter.ItemID);
                obj.Add("lv", iter.Level);
                obj.Add("cnt", iter.Count);

                jarr.Add(obj);
            }

            return SJson.ObjectToJson(jarr);
        }
#endif
    }

}

//namespace Global
//{
//    public static class LogHelper
//    {
//        public static _LogBase Create(eLog type)
//        {
//            var retval = new LogBase();
//            retval.log_type = (int)type;
//            retval.dw_action_time = DateTime.UtcNow;
//            return retval;
//        }

//#if SERVER_ONLY
//        public static LogBase PrepareLog(eLog type, PlayServer.CUser user, PlayServer.CRewardDBMerge dbtran, int actionCnt)
//        {
//            var log = Create(type)
//                        .SetPlayerData(user.PlayerData)
//                        .SetActionCnt(actionCnt);

//            if (dbtran != null)
//            {
//                dbtran.PrevObjsToJson4Log(log);
//                dbtran.UpdateObjsToJson4Log(log);
//            }

//            return log;
//        }

//        public static void PrevObjsFrom(LogBase log, PlayServer.CUser user, PlayServer.CRewardInfo info)
//        {
//            foreach (var item in info.m_StackItemDataList)
//            {
//                if (user.ItemAgent.FindItemToTableBegin(item.TID) is _ItemData hasData)
//                    log.SetPrevObjs(hasData);
//            }

//            foreach (var asset in info.m_AssetDataList)
//            {
//                if (user.GetAsset(asset.type) is _AssetData hasData)
//                    log.SetPrevObjs(hasData);
//            }

//            foreach (var ticket in info.m_TicketDataList)
//            {
//                if (user.TicketAgent.FindTicket(ticket.m_Type) is _TicketData hasData)
//                    log.SetPrevObjs(hasData);
//            }
//        }

//        public static void UpdateObjsFrom(LogBase log, PlayServer.CRewardInfo info)
//        {
//            info.m_StackItemDataList.ForEach(x => { log.SetUpdateObjs(x); });
//            info.m_NoStackItemDataList.ForEach(x => { log.SetUpdateObjs(x); });
//            info.m_AssetDataList.ForEach(x => { log.SetUpdateObjs(x); });
//            info.m_TicketDataList.ForEach(x => { log.SetUpdateObjs(x); });
//        }

//#endif



//        public static string ToJson(_ItemData data)
//        {
//            var obj = new JObject();

//            obj.Add("uid", data.UID);
//            obj.Add("tid", data.TID);
//            obj.Add("cnt", data.Cnt);
//            obj.Add("lv", data.Lv);
//            obj.Add("exp", data.Exp);
//            obj.Add("enchant", data.Enchant);
//            if (data.option.Count > 0)
//                obj.Add("opt", ToJson(data.option));
//            if (data.special_option.Count > 0)
//                obj.Add("s_opt", ToJson(data.special_option));

//            return SJson.ObjectToJson(obj);
//        }

//        public static string ToJson(List<_SkinOptionData> data)
//        {
//            if (data.Count < 1)
//                return "";

//            var jarr = new JArray();
//            foreach (var opt in data)
//            {
//                var obj = new JObject();
//                obj.Add("slot", opt.Slot);
//                obj.Add("key", opt.Key);
//                obj.Add("idx", opt.Idx);
//                jarr.Add(obj);
//            }

//            return SJson.ObjectToJson(jarr);
//        }
//        public static string ToJson(_AssetData data)
//        {
//            var obj = new JObject();
//            obj.Add("type", (uint)data.type);
//            obj.Add("val", data.val);

//            return SJson.ObjectToJson(obj);
//        }

//        public static string ToJson(_TicketData data)
//        {
//            var obj = new JObject();
//            obj.Add("type", (uint)data.m_Type);
//            obj.Add("cnt", data.m_FreeCnt);

//            return SJson.ObjectToJson(obj);
//        }

//        public static string PrevObjToJson(LogBase log)
//        {
//            var tmp = new Dictionary<eObjectType, JArray>();
//            tmp.Add(eObjectType.Asset, new JArray());
//            log.prev_asset.ForEach(x => { tmp[eObjectType.Asset].Add(ToJson(x)); });
//            tmp.Add(eObjectType.Item, new JArray());
//            log.prev_item.ForEach(x => { tmp[eObjectType.Item].Add(ToJson(x)); });
//            tmp.Add(eObjectType.Ticket, new JArray());
//            log.prev_ticket.ForEach(x => { tmp[eObjectType.Ticket].Add(ToJson(x)); });

//            return SJson.ObjectToJson(tmp);
//        }

//        public static string UpdateObjToJson(LogBase log)
//        {
//            var tmp = new Dictionary<eObjectType, JArray>();
//            tmp.Add(eObjectType.Asset, new JArray());
//            log.update_asset.ForEach(x => { tmp[eObjectType.Asset].Add(ToJson(x)); });
//            tmp.Add(eObjectType.Item, new JArray());
//            log.update_item.ForEach(x => { tmp[eObjectType.Item].Add(ToJson(x)); });
//            tmp.Add(eObjectType.Ticket, new JArray());
//            log.update_ticket.ForEach(x => { tmp[eObjectType.Ticket].Add(ToJson(x)); });

//            return SJson.ObjectToJson(tmp);
//        }
//    }
//}
