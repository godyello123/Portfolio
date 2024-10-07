using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Global;
using SCommon;

namespace MessageServer
{
    public class CheatManager : SSingleton<CheatManager>
    {
        public Packet_Result.Result ReqCheat(long sessionKey, long userSession, string cheatStr, List<string> cheatparam)
        {
            switch (cheatStr)
            {
                case "add_notice":
                    {
                        if (cheatparam.Count < 5)
                            return Packet_Result.Result.CheatParamError;

                        int idx = 0;
                        int id = 0;
                        if (!int.TryParse(cheatparam[idx++], out id))
                            return Packet_Result.Result.CheatParamError;

                        string msg = cheatparam[idx++];

                        DateTime begin = DateTime.MinValue;
                        if (!DateTime.TryParse(cheatparam[idx++], out begin))
                            return Packet_Result.Result.CheatParamError;

                        DateTime end = DateTime.MinValue;
                        if (!DateTime.TryParse(cheatparam[idx++], out end))
                            return Packet_Result.Result.CheatParamError;

                        int loop = 0;
                        if (!int.TryParse(cheatparam[idx++], out loop))
                            return Packet_Result.Result.CheatParamError;

                        int term = 0;
                        if (!int.TryParse(cheatparam[idx++], out term))
                            return Packet_Result.Result.CheatParamError;

                        _NoticeData notice = new _NoticeData();
                        notice.m_ID = id;
                        notice.m_Msg = msg;
                        notice.m_StartDate = begin;
                        notice.m_EndDate = end;
                        notice.m_Loop = loop;
                        notice.m_Term = term;

                        CSystemManager.Instance.PushNotice(notice);
                    }
                    break;
                case "erase_notice":
                    {
                        if (cheatparam.Count < 1)
                            return Packet_Result.Result.CheatParamError;

                        int idx = 0;
                        long id = 0;
                        if (!long.TryParse(cheatparam[idx++], out id))
                            return Packet_Result.Result.CheatParamError;

                        CSystemManager.Instance.EraseNotice(id);
                    }
                    break;
                default:
                    break;
            }


            return Packet_Result.Result.Success;
        }

    }
}
