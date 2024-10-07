using System;
using System.Collections.Generic;
using System.Text;
using SCommon;
using SNetwork;

//namespace Global
//{
//    public enum eLogMainType
//    {
//        Stage_Log,
//        Info_Log,
//        Asset_Log,
//        Skill_Log,
//        Enchant_Item_Log,
//        Upgrade_Item_Log,
//        Update_Item_Log,
//        Option_Change_Log,
//        Item_Sell_Log,
//        Use_Item_Log,
//        Ticket_Log,
//        Gacha_Log,
//        Cheat_Log,
//        Patten_Log,
//        Attendance_Log,
//        Post_Log,
//        Coupon_Log,
//        Quest_Log,
//        Shop_Log,
//        Pass_Log,
//        OffLineReward_Log,
//        Emblem_Log,
//        Transcendence_Item_Log,
//        PvP_Log,
//        Breakthroug_Log,
//        Guild_Log,
//        Auction_Log,
//        Transfer_Log,
//        Raid_Log,
//        Title_Log,
//        Max
//    }

//    public enum eLogDetailType
//    {
//        //Stage_Log
//        Stage_Clear_Log,
//        Stage_Skip_Log,

//        //Info_Log
//        Level_Up_Log,
//        Login_Log,
//        NickName_Change_Log,

//        //AssetLog
//        Update_Asset_Log,

//        //Skill_Log
//        Skill_Upgrade_Log,

//        //Enchant_Item_Log
//        Enchant_Weapon_Log,
//        Enchant_Glove_Log,
//        Enchant_Armor_Log,
//        Enchant_Helmet_Log,
//        Enchant_Treasure_Log,

//        //Upgrade_Item_Log
//        Upgrade_Weapon_Log,
//        Upgrade_Glove_Log,
//        Upgrade_Armor_Log,
//        Upgrade_Helmet_Log,
//        Upgrade_Pet_Log,
//        Upgrade_PetItem_Log,


//        //Update_Item_Log
//        Update_Item_Log,

//        //Option_Change_Log
//        Weapon_Option_Change_Log,
//        Pet_Option_Change_Log,
//        Skin_Option_Change_Log,

//        //Sell_Log
//        Sell_Treasure_Log,

//        //Use_Item_Log
//        Use_Item_Log,

//        //Ticket
//        Ticket_Log,

//        //Gacha
//        Gacha_Weapon_Log,
//        Gacha_Defence_Log,
//        Gacha_Treasure_Log,
//        Gacha_Reward_Log,

//        //cheat
//        Cheat_Log,

//        //patten
//        PattenLevelUp_Log,
//        PattenMix_Log,

//        //Attendance
//        Attendance_Log,

//        //Post
//        Post_Log,

//        //Coupon
//        Coupon_Log,

//        //Quest
//        OrderQuest_Log,
//        RepeatQuest_Log,
//        DailyQuest_Log,

//        //Shop
//        Shop_Log,
//        EventShop_Log,

//        //Pass_Log,
//        Pass_Log,

//        //OffLineReward
//        OffLineReward_Log,

//        //Emblem
//        Emblem_Log,

//        //Enchant_Item_Log
//        Enchant_Acc_Log,

//        //Transcendence_Item_Log
//        Transcendence_Acc_Log,

//        //PvP
//        PvP_Start_Log,
//        PvP_End_Log,

//        //Upgrade_Item
//        Upgrade_Acc_Log,

//        //Option_Change_Log
//        Acc_Option_Change_Log,

//        //Gacha
//        Gacha_Acc_Log,

//        //Breakthroug
//        Breakthroug_Log,

//        //Guild
//        Guild_Create_Log,
//        Guild_Jackpot_Log,

//        //Enchant
//        Enchant_Ego_Log,

//        //Option_chage
//        Ego_Option_Change_Log,
//        Defence_Option_Change_Log,

//        //Auction_Log
//        Auction_Register_Log,
//        Auction_DeRegister_Log,
//        Auction_Buy_Log,
//        Auction_Collect_Log,
//        Auction_Exchange_Log,

//        //Enchant
//        Enchant_Figure_Log,

//        //Transfer
//        Transfer_Figure_Log,

//        //Rald
//        Raid_Reward_Log,

//        //Figure_Option_Change
//        Figure_Option_Change_Log,
//        Title_Unlock_Log,
//        Max
//    }

//    public class LogFormat
//    {
//        public static List<eLogDetailType> GetDetailTypeList(eLogMainType type)
//        {
//            List<eLogDetailType> list = new List<eLogDetailType>();

//            switch (type)
//            {
//                case eLogMainType.Stage_Log:
//                {
//                    list.Add(eLogDetailType.Stage_Clear_Log);
//                    list.Add(eLogDetailType.Stage_Skip_Log);
//                }
//                break;
//                case eLogMainType.Info_Log:
//                {
//                    list.Add(eLogDetailType.Level_Up_Log);
//                    list.Add(eLogDetailType.NickName_Change_Log);
//                    list.Add(eLogDetailType.Login_Log);
//                    list.Add(eLogDetailType.OffLineReward_Log);
//                }
//                break;
//                case eLogMainType.Asset_Log:
//                {
//                    list.Add(eLogDetailType.Update_Asset_Log);
//                }
//                break;
//                case eLogMainType.Skill_Log:
//                {
//                    list.Add(eLogDetailType.Skill_Upgrade_Log);
//                }
//                break;
//                case eLogMainType.Enchant_Item_Log:
//                {
//                    list.Add(eLogDetailType.Enchant_Weapon_Log);
//                    list.Add(eLogDetailType.Enchant_Glove_Log);
//                    list.Add(eLogDetailType.Enchant_Armor_Log);
//                    list.Add(eLogDetailType.Enchant_Helmet_Log);
//                    list.Add(eLogDetailType.Enchant_Treasure_Log);
//                    list.Add(eLogDetailType.Enchant_Acc_Log);
//                }
//                break;
//                case eLogMainType.Upgrade_Item_Log:
//                {
//                    list.Add(eLogDetailType.Upgrade_Weapon_Log);
//                    list.Add(eLogDetailType.Upgrade_Glove_Log);
//                    list.Add(eLogDetailType.Upgrade_Armor_Log);
//                    list.Add(eLogDetailType.Upgrade_Helmet_Log);
//                    list.Add(eLogDetailType.Upgrade_PetItem_Log);
//                    list.Add(eLogDetailType.Upgrade_Acc_Log);
//                }
//                break;
//                case eLogMainType.Update_Item_Log:
//                {
//                    list.Add(eLogDetailType.Update_Item_Log);
//                }
//                break;
//                case eLogMainType.Option_Change_Log:
//                {
//                    list.Add(eLogDetailType.Weapon_Option_Change_Log);
//                    list.Add(eLogDetailType.Pet_Option_Change_Log);
//                    list.Add(eLogDetailType.Skin_Option_Change_Log);
//                    list.Add(eLogDetailType.Acc_Option_Change_Log);
//                }
//                break;
//                case eLogMainType.Item_Sell_Log:
//                {
//                    list.Add(eLogDetailType.Sell_Treasure_Log);
//                }
//                break;
//                case eLogMainType.Use_Item_Log:
//                {
//                    list.Add(eLogDetailType.Use_Item_Log);
//                }
//                break;
//                case eLogMainType.Ticket_Log:
//                {
//                    list.Add(eLogDetailType.Ticket_Log);
//                }
//                break;
//                case eLogMainType.Gacha_Log:
//                {
//                    list.Add(eLogDetailType.Gacha_Weapon_Log);
//                    list.Add(eLogDetailType.Gacha_Defence_Log);
//                    list.Add(eLogDetailType.Gacha_Treasure_Log);
//                    list.Add(eLogDetailType.Gacha_Reward_Log);
//                    list.Add(eLogDetailType.Gacha_Acc_Log);
//                }
//                break;
//                case eLogMainType.Cheat_Log:
//                {
//                    list.Add(eLogDetailType.Cheat_Log);
//                }
//                break;
//                case eLogMainType.Patten_Log:
//                {
//                    list.Add(eLogDetailType.PattenLevelUp_Log);
//                    list.Add(eLogDetailType.PattenMix_Log);
//                }
//                break;
//                case eLogMainType.Attendance_Log:
//                {
//                    list.Add(eLogDetailType.Attendance_Log);
//                }
//                break;
//                case eLogMainType.Post_Log:
//                {
//                    list.Add(eLogDetailType.Post_Log);
//                }
//                break;
//                case eLogMainType.Coupon_Log:
//                {
//                    list.Add(eLogDetailType.Coupon_Log);
//                }
//                break;
//                case eLogMainType.Quest_Log:
//                {
//                    list.Add(eLogDetailType.OrderQuest_Log);
//                }
//                break;
//                case eLogMainType.Shop_Log:
//                {
//                    list.Add(eLogDetailType.Shop_Log);
//                    list.Add(eLogDetailType.EventShop_Log);
//                }
//                break;
//                case eLogMainType.Pass_Log:
//                {
//                    list.Add(eLogDetailType.Pass_Log);
//                }
//                break;
//                case eLogMainType.Emblem_Log:
//                {
//                    list.Add(eLogDetailType.Emblem_Log);
//                }
//                break;
//                case eLogMainType.Transcendence_Item_Log:
//                {
//                    list.Add(eLogDetailType.Transcendence_Acc_Log);
//                }
//                break;
//                case eLogMainType.PvP_Log:
//                {
//                    list.Add(eLogDetailType.PvP_Start_Log);
//                    list.Add(eLogDetailType.PvP_End_Log);
//                }
//                break;
//                case eLogMainType.Breakthroug_Log:
//                {
//                    list.Add(eLogDetailType.Breakthroug_Log);
//                }
//                break;
//                case eLogMainType.Guild_Log:
//                {
//                    list.Add(eLogDetailType.Guild_Create_Log);
//                    list.Add(eLogDetailType.Guild_Jackpot_Log);
//                }
//                break;
//                case eLogMainType.Max:
//                break;
//                default:
//                break;
//            }

//            return list;
//        }

//        public static eLogDetailType EnchantItemType(eItemDetailType type)
//        {
//            eLogDetailType detail_type = eLogDetailType.Max;
//            switch (type)
//            {
//                case eItemDetailType.Weapon:
//                detail_type = eLogDetailType.Enchant_Weapon_Log;
//                break;
//                case eItemDetailType.Armor:
//                detail_type = eLogDetailType.Enchant_Armor_Log;
//                break;
//                case eItemDetailType.Helmet:
//                detail_type = eLogDetailType.Enchant_Helmet_Log;
//                break;
//                case eItemDetailType.Glove:
//                detail_type = eLogDetailType.Enchant_Glove_Log;
//                break;
//                case eItemDetailType.Treasure_Normal:
//                case eItemDetailType.Treasure_Raid:
//                detail_type = eLogDetailType.Enchant_Treasure_Log;
//                break;
//                case eItemDetailType.Accessory_Earring:
//                case eItemDetailType.Accessory_HairPin:
//                case eItemDetailType.Accessory_Ring:
//                detail_type = eLogDetailType.Enchant_Acc_Log;
//                break;
//                case eItemDetailType.Ego_Attack:
//                detail_type = eLogDetailType.Enchant_Ego_Log;
//                break;
//                case eItemDetailType.Monster:
//                case eItemDetailType.Master:
//                detail_type = eLogDetailType.Enchant_Figure_Log;
//                break;
//                default:
//                break;
//            }

//            return detail_type;
//        }

//        public static eLogDetailType UpgradeItemType(eItemDetailType type)
//        {

//            eLogDetailType detail_type = eLogDetailType.Max;
//            switch (type)
//            {
//                case eItemDetailType.Weapon:
//                detail_type = eLogDetailType.Upgrade_Weapon_Log;
//                break;
//                case eItemDetailType.Glove:
//                detail_type = eLogDetailType.Upgrade_Glove_Log;
//                break;
//                case eItemDetailType.Armor:
//                detail_type = eLogDetailType.Upgrade_Armor_Log;
//                break;
//                case eItemDetailType.Helmet:
//                detail_type = eLogDetailType.Upgrade_Helmet_Log;
//                break;
//                case eItemDetailType.Ring:
//                case eItemDetailType.Neck:
//                case eItemDetailType.Earring:
//                detail_type = eLogDetailType.Upgrade_PetItem_Log;
//                break;
//                case eItemDetailType.Pet:
//                detail_type = eLogDetailType.Upgrade_Pet_Log;
//                break;
//                case eItemDetailType.Accessory_Earring:
//                case eItemDetailType.Accessory_HairPin:
//                case eItemDetailType.Accessory_Ring:
//                detail_type = eLogDetailType.Upgrade_Acc_Log;
//                break;
//                default:
//                break;
//            }

//            return detail_type;
//        }

//        public static eLogDetailType TranscendanceType(eItemDetailType type)
//        {
//            eLogDetailType detail_type = eLogDetailType.Max;
//            switch (type)
//            {
//                case eItemDetailType.Accessory_Earring:
//                case eItemDetailType.Accessory_HairPin:
//                case eItemDetailType.Accessory_Ring:
//                detail_type = eLogDetailType.Transcendence_Acc_Log;
//                break;
//                default:
//                break;
//            }

//            return detail_type;
//        }

//        public static eLogDetailType RandomOptionChange(eItemMainType type)
//        {
//            eLogDetailType detail_type = eLogDetailType.Max;
//            switch (type)
//            {
//                case eItemMainType.Weapon:
//                detail_type = eLogDetailType.Weapon_Option_Change_Log;
//                break;
//                case eItemMainType.Pet:
//                detail_type = eLogDetailType.Pet_Option_Change_Log;
//                break;
//                case eItemMainType.Defence:
//                detail_type = eLogDetailType.Skin_Option_Change_Log;
//                break;
//                case eItemMainType.Accessory:
//                detail_type = eLogDetailType.Acc_Option_Change_Log;
//                break;
//                case eItemMainType.Ego:
//                detail_type = eLogDetailType.Ego_Option_Change_Log;
//                break;
//                case eItemMainType.Figure:
//                detail_type = eLogDetailType.Figure_Option_Change_Log;
//                break;
//                case eItemMainType.Treasure:
//                case eItemMainType.Profile:
//                case eItemMainType.Costume:
//                case eItemMainType.Pet_Item:
//                case eItemMainType.Etc:
//                case eItemMainType.Max:
//                default:
//                break;
//            }

//            return detail_type;
//        }
//        public static eLogDetailType GachaType(eGachaType Type)
//        {
//            eLogDetailType detailType = eLogDetailType.Max;
//            switch (Type)
//            {
//                case eGachaType.Weapon:
//                detailType = eLogDetailType.Gacha_Weapon_Log;
//                break;
//                case eGachaType.Defence:
//                detailType = eLogDetailType.Gacha_Defence_Log;
//                break;
//                case eGachaType.Treasure:
//                detailType = eLogDetailType.Gacha_Treasure_Log;
//                break;
//                case eGachaType.Accessory:
//                detailType = eLogDetailType.Gacha_Acc_Log;
//                break;
//                case eGachaType.Max:
//                break;
//                default:
//                break;
//            }

//            return detailType;
//        }

//        public static eLogDetailType SellType(eItemMainType Type)
//        {
//            eLogDetailType detailType = eLogDetailType.Max;
//            switch (Type)
//            {
//                case eItemMainType.Treasure:
//                detailType = eLogDetailType.Sell_Treasure_Log;
//                break;
//                default:
//                break;
//            }

//            return detailType;
//        }

//        public static eLogDetailType TransferType(eItemMainType type)
//        {
//            eLogDetailType detail = eLogDetailType.Max;
//            switch (type)
//            {
//                case eItemMainType.Figure:
//                detail = eLogDetailType.Transfer_Figure_Log;
//                break;
//                default:
//                break;
//            }

//            return detail;
//        }
//    }

//    public class Base_Log
//    {
//        public long ID = 0;
//        public eLogMainType MainType = eLogMainType.Max;
//        public eLogDetailType DetailType = eLogDetailType.Max;
//        public DateTime TimeBinary = CDefine.MinValue();

//        public Base_Log()
//        {
//        }

//        public Base_Log(long userid , List<_AssetData> BeforAssetList , List<_AssetData> afterAssetList , DateTime datetime)
//        {
//        }

//        public void Base_Read(SNetReader reader)
//        {
//            if (!reader.Read(ref ID))
//                return;
//            short t = 0;
//            if (!reader.Read(ref t))
//                return;
//            MainType = (eLogMainType)t;

//            short t2 = 0;
//            if (!reader.Read(ref t2))
//                return;
//            DetailType = (eLogDetailType)t2;

//            if (!reader.Read(ref TimeBinary))
//                return;
//        }

//        public void Base_Write(SNetWriter writer)
//        {
//            if (!writer.Write(ID))
//                return;
//            if (!writer.Write((short)MainType))
//                return;
//            if (!writer.Write((short)DetailType))
//                return;
//            if (!writer.Write(TimeBinary))
//                return;
//        }

//        public void Init(long userid , DateTime datetime , eLogDetailType Type)
//        {
//            ID = userid;

//            TimeBinary = datetime;

//            DetailType = Type;

//            switch (DetailType)
//            {
//                case eLogDetailType.Stage_Clear_Log:
//                case eLogDetailType.Stage_Skip_Log:
//                MainType = eLogMainType.Stage_Log;
//                break;
//                case eLogDetailType.NickName_Change_Log:
//                case eLogDetailType.Level_Up_Log:
//                case eLogDetailType.Login_Log:
//                case eLogDetailType.OffLineReward_Log:
//                MainType = eLogMainType.Info_Log;
//                break;
//                case eLogDetailType.Update_Asset_Log:
//                MainType = eLogMainType.Asset_Log;
//                break;
//                case eLogDetailType.Skill_Upgrade_Log:
//                MainType = eLogMainType.Skill_Log;
//                break;
//                case eLogDetailType.Enchant_Weapon_Log:
//                case eLogDetailType.Enchant_Glove_Log:
//                case eLogDetailType.Enchant_Armor_Log:
//                case eLogDetailType.Enchant_Helmet_Log:
//                case eLogDetailType.Enchant_Treasure_Log:
//                case eLogDetailType.Enchant_Acc_Log:
//                case eLogDetailType.Enchant_Ego_Log:
//                case eLogDetailType.Enchant_Figure_Log:
//                MainType = eLogMainType.Enchant_Item_Log;
//                break;
//                case eLogDetailType.Upgrade_Weapon_Log:
//                case eLogDetailType.Upgrade_Glove_Log:
//                case eLogDetailType.Upgrade_Armor_Log:
//                case eLogDetailType.Upgrade_Helmet_Log:
//                case eLogDetailType.Upgrade_PetItem_Log:
//                case eLogDetailType.Upgrade_Pet_Log:
//                case eLogDetailType.Upgrade_Acc_Log:
//                MainType = eLogMainType.Upgrade_Item_Log;
//                break;
//                case eLogDetailType.Update_Item_Log:
//                MainType = eLogMainType.Update_Item_Log;
//                break;
//                case eLogDetailType.Sell_Treasure_Log:
//                MainType = eLogMainType.Item_Sell_Log;
//                break;
//                case eLogDetailType.Weapon_Option_Change_Log:
//                case eLogDetailType.Pet_Option_Change_Log:
//                case eLogDetailType.Skin_Option_Change_Log:
//                case eLogDetailType.Acc_Option_Change_Log:
//                case eLogDetailType.Ego_Option_Change_Log:
//                case eLogDetailType.Figure_Option_Change_Log:
//                MainType = eLogMainType.Option_Change_Log;
//                break;
//                case eLogDetailType.Use_Item_Log:
//                MainType = eLogMainType.Use_Item_Log;
//                break;
//                case eLogDetailType.Gacha_Weapon_Log:
//                case eLogDetailType.Gacha_Defence_Log:
//                case eLogDetailType.Gacha_Treasure_Log:
//                case eLogDetailType.Gacha_Reward_Log:
//                case eLogDetailType.Gacha_Acc_Log:
//                MainType = eLogMainType.Gacha_Log;
//                break;
//                case eLogDetailType.Cheat_Log:
//                MainType = eLogMainType.Cheat_Log;
//                break;
//                case eLogDetailType.PattenLevelUp_Log:
//                case eLogDetailType.PattenMix_Log:
//                MainType = eLogMainType.Patten_Log;
//                break;
//                case eLogDetailType.Attendance_Log:
//                MainType = eLogMainType.Attendance_Log;
//                break;
//                case eLogDetailType.Post_Log:
//                MainType = eLogMainType.Post_Log;
//                break;
//                case eLogDetailType.Coupon_Log:
//                MainType = eLogMainType.Coupon_Log;
//                break;
//                case eLogDetailType.Ticket_Log:
//                MainType = eLogMainType.Ticket_Log;
//                break;
//                case eLogDetailType.OrderQuest_Log:
//                MainType = eLogMainType.Quest_Log;
//                break;
//                case eLogDetailType.Shop_Log:
//                case eLogDetailType.EventShop_Log:
//                MainType = eLogMainType.Shop_Log;
//                break;
//                case eLogDetailType.Pass_Log:
//                MainType = eLogMainType.Pass_Log;
//                break;
//                case eLogDetailType.Emblem_Log:
//                MainType = eLogMainType.Emblem_Log;
//                break;
//                case eLogDetailType.Transcendence_Acc_Log:
//                MainType = eLogMainType.Transcendence_Item_Log;
//                break;
//                case eLogDetailType.PvP_Start_Log:
//                case eLogDetailType.PvP_End_Log:
//                MainType = eLogMainType.PvP_Log;
//                break;
//                case eLogDetailType.Breakthroug_Log:
//                MainType = eLogMainType.Breakthroug_Log;
//                break;
//                case eLogDetailType.Guild_Create_Log:
//                case eLogDetailType.Guild_Jackpot_Log:
//                MainType = eLogMainType.Guild_Log;
//                break;
//                case eLogDetailType.Auction_Buy_Log:
//                case eLogDetailType.Auction_Collect_Log:
//                case eLogDetailType.Auction_DeRegister_Log:
//                case eLogDetailType.Auction_Register_Log:
//                case eLogDetailType.Auction_Exchange_Log:
//                MainType = eLogMainType.Auction_Log;
//                break;
//                case eLogDetailType.Transfer_Figure_Log:
//                MainType = eLogMainType.Transfer_Log;
//                break;
//                case eLogDetailType.Raid_Reward_Log:
//                MainType = eLogMainType.Raid_Log;
//                break;
//                case eLogDetailType.Title_Unlock_Log:
//                MainType = eLogMainType.Title_Log;
//                break;
//                case eLogDetailType.Max:
//                break;
//                default:
//                break;
//            }
//        }

//        public List<string> BaseVarString()
//        {
//            List<string> list = new List<string>();
//            list.Add(ID.ToString());
//            list.Add(MainType.ToString());
//            list.Add(DetailType.ToString());
//            list.Add(TimeBinary.ToString());
//            return list;
//        }

//    }

//    public class Stage_Clear_Log : Base_Log, INetSerialize
//    {
//        public eStageType Type = eStageType.Max;
//        public int TID = 0;
//        public double Calc_Damage;
//        public double Calc_Critical1;
//        public double Calc_Critical2;


//        public Stage_Clear_Log()
//        {

//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            short t = 0;
//            if (!reader.Read(ref t)) return;
//            Type = (eStageType)t;
//            if (!reader.Read(ref TID)) return;
//            if (!reader.Read(ref Calc_Damage)) return;
//            if (!reader.Read(ref Calc_Critical1)) return;
//            if (!reader.Read(ref Calc_Critical2)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write((short)Type)) return;
//            if (!writer.Write(TID)) return;
//            if (!writer.Write(Calc_Damage)) return;
//            if (!writer.Write(Calc_Critical1)) return;
//            if (!writer.Write(Calc_Critical2)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Stage_Clear_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Stage_Skip_Log : Base_Log, INetSerialize
//    {
//        public eStageType Type = eStageType.Max;
//        public int TID = 0;
//        public int Count = 0;
//        public eTicketType TicketType = eTicketType.Max;
//        public int CostTicket = 0;
//        public CRewardLogData Reward = new CRewardLogData();

//        public Stage_Skip_Log()
//        {

//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            ushort t = 0;
//            if (!reader.Read(ref t)) return;
//            Type = (eStageType)t;
//            if (!reader.Read(ref TID)) return;
//            if (!reader.Read(ref Count)) return;
//            t = 0;
//            if (!reader.Read(ref t)) return;
//            TicketType = (eTicketType)t;
//            if (!reader.Read(ref CostTicket)) return;
//            if (!reader.Read(ref Reward)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write((ushort)Type)) return;
//            if (!writer.Write(TID)) return;
//            if (!writer.Write(Count)) return;
//            if (!writer.Write((ushort)TicketType)) return;
//            if (!writer.Write(CostTicket)) return;
//            if (!writer.Write(Reward)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Stage_Skip_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class PlayerData_Log : Base_Log, INetSerialize
//    {
//        public string Name = string.Empty;
//        public int BeforeLv;
//        public int Lv;
//        public DateTime LoginTime = CDefine.MinValue();

//        public PlayerData_Log()
//        {

//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref Name))
//                return;
//            if (!reader.Read(ref BeforeLv))
//                return;
//            if (!reader.Read(ref Lv))
//                return;
//            if (!reader.Read(ref LoginTime))
//                return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(Name)) return;
//            if (!writer.Write(BeforeLv)) return;
//            if (!writer.Write(Lv)) return;
//            if (!writer.Write(LoginTime)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(PlayerData_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Skill_Upgrade_Log : Base_Log, INetSerialize
//    {
//        public int TID = 0;
//        public int Before_Level = 0;
//        public int Level = 0;
//        public CRewardLogData Reward = new CRewardLogData();

//        public Skill_Upgrade_Log()
//        {

//        }
//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref TID)) return;
//            if (!reader.Read(ref Before_Level)) return;
//            if (!reader.Read(ref Level)) return;
//            if (!reader.Read(ref Reward)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(TID)) return;
//            if (!writer.Write(Before_Level)) return;
//            if (!writer.Write(Level)) return;
//            if (!writer.Write(Reward)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Skill_Upgrade_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }


//    public class Enchant_Item_Log : Base_Log, INetSerialize
//    {
//        public long ItemUID = 0;
//        public long ItemTID = 0;
//        public bool Success = false;
//        public int Lv = 0;
//        public CRewardLogData Reward = new CRewardLogData();

//        public Enchant_Item_Log()
//        {

//        }

//        public Enchant_Item_Log(long item_uid , long item_tid , bool result , int lv)
//        {
//            ItemUID = item_uid;
//            ItemTID = item_tid;
//            Success = result;
//            Lv = lv;
//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref ItemUID)) return;
//            if (!reader.Read(ref ItemTID)) return;
//            if (!reader.Read(ref Success)) return;
//            if (!reader.Read(ref Lv)) return;
//            if (!reader.Read(ref Reward)) return;
//        }
//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(ItemUID)) return;
//            if (!writer.Write(ItemTID)) return;
//            if (!writer.Write(Success)) return;
//            if (!writer.Write(Lv)) return;
//            if (!writer.Write(Reward)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Enchant_Item_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Upgrade_Item_Log : Base_Log, INetSerialize
//    {
//        public int Count = 0;
//        public CRewardLogData Reward = new CRewardLogData();

//        public Upgrade_Item_Log()
//        {

//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref Count)) return;
//            if (!reader.Read(ref Reward)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(Count)) return;
//            if (!writer.Write(Reward)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Upgrade_Item_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Update_Item_Log : Base_Log, INetSerialize
//    {
//        public List<_ItemData> AddList = new List<_ItemData>();
//        public List<_ItemData> DeleteList = new List<_ItemData>();
//        public List<_ItemData> UpdateList = new List<_ItemData>(); //수량 이나 그런거 변경 될때
//        public eLogDetailType Event = eLogDetailType.Max;

//        public Update_Item_Log()
//        {

//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            int count = 0;
//            if (!reader.Read(ref count)) return;
//            for (int i = 0; i < count; ++i)
//            {
//                _ItemData Item = new _ItemData();
//                if (!reader.Read(ref Item)) return;
//                AddList.Add(Item);
//            }

//            int count2 = 0;
//            if (!reader.Read(ref count2)) return;
//            for (int i = 0; i < count2; ++i)
//            {
//                _ItemData Item = new _ItemData();
//                if (!reader.Read(ref Item)) return;
//                DeleteList.Add(Item);
//            }

//            int count3 = 0;
//            if (!reader.Read(ref count3)) return;
//            for (int i = 0; i < count3; ++i)
//            {
//                _ItemData item = new _ItemData();
//                if (!reader.Read(ref item)) return;
//                UpdateList.Add(item);
//            }

//            short t = 0;
//            if (!reader.Read(ref t)) return;
//            Event = (eLogDetailType)t;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(AddList.Count)) return;
//            foreach (var Data in AddList)
//            {
//                if (!writer.Write(Data)) return;
//            }

//            if (!writer.Write(DeleteList.Count)) return;
//            foreach (var Data in DeleteList)
//            {
//                if (!writer.Write(Data)) return;
//            }

//            if (!writer.Write(UpdateList.Count)) return;
//            foreach (var data in UpdateList)
//            {
//                if (!writer.Write(data)) return;
//            }
//            if (!writer.Write((short)Event)) return;
//        }

//        public void Merge()
//        {
//            Dictionary<int , _ItemData> Add = new Dictionary<int , _ItemData>();
//            foreach (var Data in AddList)
//            {
//                if (Add.ContainsKey(Data.TID))
//                    Add[Data.TID].Cnt += Data.Cnt;
//                else
//                    Add.Add(Data.TID , SCopy<_ItemData>.DeepCopy(Data));
//            }

//            AddList.Clear();
//            foreach (var Data in Add)
//            {
//                AddList.Add(Data.Value);
//            }

//            Dictionary<int , _ItemData> Delete = new Dictionary<int , _ItemData>();
//            foreach (var Data in DeleteList)
//            {
//                if (Delete.ContainsKey(Data.TID))
//                    Delete[Data.TID].Cnt += Data.Cnt;
//                else
//                    Delete.Add(Data.TID , SCopy<_ItemData>.DeepCopy(Data));
//            }

//            DeleteList.Clear();
//            foreach (var Data in Delete)
//            {
//                DeleteList.Add(Data.Value);
//            }

//            Dictionary<int , _ItemData> Update = new Dictionary<int , _ItemData>();
//            foreach (var Data in UpdateList)
//            {
//                if (Update.ContainsKey(Data.TID))
//                    Update[Data.TID].Cnt += Data.Cnt;
//                else
//                    Update.Add(Data.TID , SCopy<_ItemData>.DeepCopy(Data));
//            }

//            Update.Clear();
//            foreach (var Data in Update)
//            {
//                UpdateList.Add(Data.Value);
//            }
//        }

//#if OPERATION
//        public string ListToString(List<_ItemData> list)
//        {
//            StringBuilder builder = new StringBuilder();
//            for(int i = 0; i < list.Count; ++i)
//            {
//                if(i > 0)
//                    builder.Append(", ");

//                int tid = list[i].TID;
//                ItemTableData data = csTableManager.Instance.mItemTable.Find<ItemTableData>(tid.ToString());
//                if(data == null)
//                    continue;

//                string itemname = csTableManager.Instance.GetTableString(data.mNameID);
//                builder.Append(itemname);
//                if(i < list.Count - 1)
//                    builder.Append(", ");

//                //builder.Append(list[i].ToString());
//            }

//            return builder.ToString();
//        }

//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Update_Item_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                if(info.Name == "AddList")
//                    builder.Append(ListToString(AddList));
//                else if(info.Name == "DeleteList")
//                    builder.Append(ListToString(DeleteList));
//                else if(info.Name == "UpdateList")
//                    builder.Append(ListToString(UpdateList));
//                else
//                    builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Option_Change_Log : Base_Log, INetSerialize
//    {
//        public Int64 ItemUID;
//        public int ItemTID;
//        public int Count = 0;
//        public int lockCount = 0;

//        public Option_Change_Log()
//        {

//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref ItemUID)) return;
//            if (!reader.Read(ref ItemTID)) return;
//            if (!reader.Read(ref Count)) return;
//            if (!reader.Read(ref lockCount)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(ItemUID)) return;
//            if (!writer.Write(ItemTID)) return;
//            if (!writer.Write(Count)) return;
//            if (!writer.Write(lockCount)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Option_Change_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Update_Asset_Log : Base_Log, INetSerialize
//    {
//        public List<_AssetData> AddAsset = new List<_AssetData>();
//        public List<_AssetData> DelAsset = new List<_AssetData>();
//        public eLogDetailType Event = eLogDetailType.Max;

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            int count1 = 0;
//            if (!reader.Read(ref count1)) return;
//            for (int i = 0; i < count1; ++i)
//            {
//                _AssetData asset = new _AssetData();
//                if (!reader.Read(ref asset)) return;
//                AddAsset.Add(asset);
//            }

//            count1 = 0;
//            if (!reader.Read(ref count1)) return;
//            for (int i = 0; i < count1; ++i)
//            {
//                _AssetData asset = new _AssetData();
//                if (!reader.Read(ref asset)) return;
//                DelAsset.Add(asset);
//            }

//            short t = 0;
//            if (!reader.Read(ref t)) return;
//            Event = (eLogDetailType)t;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(AddAsset.Count)) return;
//            foreach (var Data in AddAsset)
//            {
//                if (!writer.Write(Data)) return;
//            }

//            if (!writer.Write(DelAsset.Count)) return;
//            foreach (var Data in DelAsset)
//            {
//                if (!writer.Write(Data)) return;
//            }
//            if (!writer.Write((short)Event)) return;
//        }

//        public void Merge()
//        {
//            Dictionary<eAssetType , _AssetData> Add = new Dictionary<eAssetType , _AssetData>();
//            foreach (var Data in AddAsset)
//            {
//                if (Add.ContainsKey(Data.type))
//                    Add[Data.type].val += Data.val;
//                else
//                    Add.Add(Data.type , SCopy<_AssetData>.DeepCopy(Data));
//            }

//            AddAsset.Clear();
//            foreach (var Data in Add)
//            {
//                AddAsset.Add(new _AssetData(Data.Value));
//            }

//            Dictionary<eAssetType , _AssetData> Del = new Dictionary<eAssetType , _AssetData>();
//            foreach (var Data in DelAsset)
//            {
//                if (Del.ContainsKey(Data.type))
//                    Del[Data.type].val += Data.val;
//                else
//                    Del.Add(Data.type , SCopy<_AssetData>.DeepCopy(Data));
//            }

//            DelAsset.Clear();
//            foreach (var Data in Del)
//            {
//                DelAsset.Add(new _AssetData(Data.Value));
//            }
//        }

//#if OPERATION
//        public string ListToString<T>(List<T> list)
//        {
//            StringBuilder builder = new StringBuilder();
//            for(int i = 0; i < list.Count; ++i)
//            {
//                if(i > 0)
//                    builder.Append(", ");

//                builder.Append(list[i].ToString());
//            }

//            return builder.ToString();
//        }

//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Update_Asset_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                if(info.Name == "AddAsset")
//                    builder.Append(ListToString(AddAsset));
//                else if(info.Name == "DelAsset")
//                    builder.Append(ListToString(DelAsset));
//                else
//                    builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Gacha_Log : Base_Log, INetSerialize
//    {
//        public eGachaType Type = eGachaType.Max;
//        public int Level = 0;
//        public int GachaCount = 0;
//        public List<int> Reward = new List<int>();

//        public Gacha_Log()
//        {

//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            short t = 0;
//            if (!reader.Read(ref t)) return;
//            Type = (eGachaType)t;
//            if (!reader.Read(ref Level)) return;
//            if (!reader.Read(ref GachaCount)) return;

//            int count = 0;
//            if (!reader.Read(ref count)) return;
//            for (int i = 0; i < count; ++i)
//            {
//                int a = 0;
//                if (!reader.Read(ref a)) return;
//                Reward.Add(a);
//            }
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write((short)Type)) return;
//            if (!writer.Write(Level)) return;
//            if (!writer.Write(GachaCount)) return;

//            if (!writer.Write(Reward.Count)) return;
//            foreach (var Data in Reward)
//            {
//                if (!writer.Write(Data)) return;
//            }
//        }


//#if OPERATION
//        public string ListToString(List<int> strlist)
//        {
//            StringBuilder builder = new StringBuilder();
//            int RewardCount = Reward.Count;
//            for(int i = 0; i < RewardCount; i++)
//            {
//                int tid = Reward[i];
//                ItemTableData data = csTableManager.Instance.mItemTable.Find<ItemTableData>(tid.ToString());
//                if(data == null)
//                    continue;

//                string itemname = csTableManager.Instance.GetTableString(data.mNameID);
//                builder.Append(itemname);
//                if(i < Reward.Count - 1)
//                    builder.Append(", ");
//            }

//            return builder.ToString();
//        }

//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Gacha_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                if(info.Name == "Reward")
//                    builder.Append(ListToString(Reward));
//                else
//                    builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Sell_Log : Base_Log, INetSerialize
//    {
//        public long ItemUID = 0;
//        public int ItemTID = 0;
//        public int SellCount = 0;

//        public Sell_Log()
//        {

//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref ItemUID)) return;
//            if (!reader.Read(ref ItemTID)) return;
//            if (!reader.Read(ref SellCount)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(ItemUID)) return;
//            if (!writer.Write(ItemTID)) return;
//            if (!writer.Write(SellCount)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Sell_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Use_Item_Log : Base_Log, INetSerialize
//    {
//        public long ItemTID = 0;
//        public int Count = 0;
//        public List<_DropInfo> Reward = new List<_DropInfo>();

//        public Use_Item_Log()
//        {

//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref ItemTID)) return;
//            if (!reader.Read(ref Count)) return;
//            int count = 0;
//            if (!reader.Read(ref count)) return;
//            for (int i = 0; i < count; ++i)
//            {
//                _DropInfo info = new _DropInfo();
//                if (!reader.Read(ref info)) return;
//                Reward.Add(info);
//            }
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(ItemTID)) return;
//            if (!writer.Write(Count)) return;
//            if (!writer.Write(Reward.Count)) return;
//            foreach (var Data in Reward)
//            {
//                if (!writer.Write(Data)) return;
//            }
//        }
//#if OPERATION
//        public string ListToString(List<_DropInfo> droplist)
//        {
//            StringBuilder builder = new StringBuilder();
//            for(int i = 0; i < droplist.Count; i++)
//            {
//                _DropInfo info = droplist[i];
//                DropTableData dropdata = csTableManager.Instance.mDropTable.Find<DropTableData>(info.ToString());
//                switch(info.m_Type)
//                {
//                    case eObjectType.Item:
//                    {
//                        var data = csTableManager.Instance.mItemTable.Find<ItemTableData>(info.m_DropKey.ToString());
//                        if(data == null)
//                            builder.Append(info.m_DropKey.ToString());
//                        else
//                        {
//                            string stritem = csTableManager.Instance.GetTableString(data.mNameID);
//                            if(stritem == null)
//                                builder.Append(info.m_DropKey.ToString());
//                            else
//                            {
//                                builder.Append(stritem);
//                                builder.Append(":");
//                                builder.Append(info.m_DropCount);
//                                builder.Append(", ");
//                            }
//                        }

//                    }
//                    break;
//                    case eObjectType.Asset:
//                    {
//                        eAssetType type = (eAssetType)info.m_DropKey;
//                        long count = info.m_DropCount;
//                        builder.Append(type.ToString());
//                        builder.Append(":");
//                        builder.Append(count.ToString());
//                        builder.Append(", ");
//                    }
//                    break;
//                    case eObjectType.Link:
//                    break;
//                    case eObjectType.Ticket:
//                    {
//                        eTicketType type = (eTicketType)info.m_DropKey;
//                        long count = info.m_DropCount;
//                        builder.Append(type.ToString());
//                        builder.Append(":");
//                        builder.Append(count.ToString());
//                        builder.Append(", ");
//                    }
//                    break;
//                    case eObjectType.Max:
//                    break;
//                    default:
//                    break;
//                }
//            }

//            return builder.ToString();
//        }

//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Use_Item_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                if(info.Name == "Reward")
//                    builder.Append(ListToString(Reward));
//                else
//                    builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Cheat_Log : Base_Log, INetSerialize
//    {
//        public eCheatType Type = eCheatType.Max;

//        public Cheat_Log()
//        {

//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            ushort t = 0;
//            if (!reader.Read(ref t)) return;
//            Type = (eCheatType)t;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write((ushort)Type)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Cheat_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Patten_Log : Base_Log, INetSerialize
//    {
//        public ePatternType PattenType = ePatternType.Max;
//        public int Level = 0;
//        public long SumCount = 0;
//        public bool Success = false;

//        public Patten_Log()
//        {

//        }


//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            short t = 0;
//            if (!reader.Read(ref t)) return;
//            PattenType = (ePatternType)t;
//            if (!reader.Read(ref Level)) return;
//            if (!reader.Read(ref SumCount)) return;
//            if (!reader.Read(ref Success)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write((short)PattenType)) return;
//            if (!writer.Write(Level)) return;
//            if (!writer.Write(SumCount)) return;
//            if (!writer.Write(Success)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Patten_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Attendance_Log : Base_Log, INetSerialize
//    {
//        public int m_Order;

//        public Attendance_Log()
//        {

//        }
//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref m_Order)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(m_Order)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Attendance_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Post_Log : Base_Log, INetSerialize
//    {
//        public List<Int64> m_ReadPostList = new List<Int64>();

//        public Post_Log()
//        {

//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            int count = 0;
//            if (!reader.Read(ref count)) return;
//            for (int i = 0; i < count; ++i)
//            {
//                Int64 uid = 0;
//                if (!reader.Read(ref uid)) return;
//                m_ReadPostList.Add(uid);
//            }
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(m_ReadPostList.Count)) return;
//            foreach (var data in m_ReadPostList)
//            {
//                if (!writer.Write(data)) return;
//            }
//        }

//#if OPERATION
//        public string ListToString(List<Int64> list)
//        {
//            StringBuilder builder = new StringBuilder();
//            for(int i = 0; i < list.Count; ++i)
//            {
//                builder.Append(list[i].ToString());
//                builder.Append(",");
//            }

//            return builder.ToString();
//        }

//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Post_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                if(info.Name == "m_ReadPostList")
//                    builder.Append(ListToString(m_ReadPostList));
//                else
//                    builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Coupon_Log : Base_Log, INetSerialize
//    {
//        public string m_CouponCode;

//        public Coupon_Log()
//        {
//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref m_CouponCode)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(m_CouponCode)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Coupon_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Quest_Log : Base_Log, INetSerialize
//    {
//        public long TableID;

//        public Quest_Log()
//        {

//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref TableID)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(TableID)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Quest_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Shop_Log : Base_Log, INetSerialize
//    {
//        public bool IsSuccess = false;

//        public Shop_Log()
//        {
//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref IsSuccess)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(IsSuccess)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Shop_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class EventShop_Log : Base_Log, INetSerialize
//    {
//        public int ShopID = 0;
//        public int Count = 0;
//        public bool IsSuccess = false;

//        public EventShop_Log()
//        {

//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref ShopID)) return;
//            if (!reader.Read(ref Count)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(ShopID)) return;
//            if (!writer.Write(Count)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(EventShop_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Ticket_Log : Base_Log, INetSerialize
//    {
//        public eTicketType TicketType = eTicketType.Max;
//        public int Count = 0;
//        public eLogDetailType Event = eLogDetailType.Max;

//        public Ticket_Log()
//        {

//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            short t = 0;
//            if (!reader.Read(ref t)) return;
//            TicketType = (eTicketType)t;
//            if (!reader.Read(ref Count)) return;
//            t = 0;
//            if (!reader.Read(ref t)) return;
//            Event = (eLogDetailType)t;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);

//            if (!writer.Write((short)TicketType)) return;
//            if (!writer.Write(Count)) return;
//            if (!writer.Write((short)Event)) return;
//        }

//#if OPERATION
//        //public string ListToString(List<_TicketData> list)
//        //{
//        //    StringBuilder builder = new StringBuilder();
//        //    for (int i = 0; i < list.Count; ++i)
//        //    {
//        //        builder.Append(list[i].ToString());
//        //    }

//        //    return builder.ToString();
//        //}

//        //public List<string> GetVarString()
//        //{
//        //    List<string> strlist = new List<string>();
//        //    strlist.Add(ListToString(PrevList));
//        //    strlist.Add(ListToString(UpdateList));
//        //    strlist.Add(ListToString(NowList));
//        //    strlist.Add(Event.ToString());

//        //    foreach (var data in base.BaseVarString())
//        //    {
//        //        strlist.Add(data);
//        //    }

//        //    return strlist;
//        //}

//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Ticket_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class OffLineReward_Log : Base_Log, INetSerialize
//    {
//        public Int64 Total_Sec;
//        public int Count = 0;
//        public CRewardLogData Reward = new CRewardLogData();

//        public OffLineReward_Log()
//        {

//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref Total_Sec)) return;
//            if (!reader.Read(ref Count)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(Total_Sec)) return;
//            if (!writer.Write(Count)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(OffLineReward_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Pass_Step_Log : Base_Log, INetSerialize
//    {
//        public int PassID;
//        public int NormalStep;
//        public int RewardStep;
//        public CRewardLogData Reward = new CRewardLogData();

//        public Pass_Step_Log()
//        {

//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref PassID)) return;
//            if (!reader.Read(ref NormalStep)) return;
//            if (!reader.Read(ref RewardStep)) return;
//            if (!reader.Read(ref Reward)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(PassID)) return;
//            if (!writer.Write(NormalStep)) return;
//            if (!writer.Write(RewardStep)) return;
//            if (!writer.Write(Reward)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Pass_Step_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Emblem_Log : Base_Log, INetSerialize
//    {
//        public int EmblemIndex = 0;

//        public Emblem_Log()
//        {

//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref EmblemIndex)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(EmblemIndex)) return;
//        }
//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Emblem_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class NickName_Log : Base_Log, INetSerialize
//    {
//        public string BeforeNickName;
//        public string AfterNickName;

//        public NickName_Log()
//        {

//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref BeforeNickName)) return;
//            if (!reader.Read(ref AfterNickName)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(BeforeNickName)) return;
//            if (!writer.Write(AfterNickName)) return;
//        }
//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(NickName_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Transcendence_Item_Log : Base_Log, INetSerialize
//    {
//        public long ItemUID = 0;
//        public long ItemTID = 0;
//        public int Transcendance = 0;
//        public CRewardLogData Reward = new CRewardLogData();

//        public Transcendence_Item_Log()
//        {

//        }

//        public Transcendence_Item_Log(long item_uid , long item_tid , int tran)
//        {
//            ItemUID = item_uid;
//            ItemTID = item_tid;
//            Transcendance = tran;
//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref ItemUID)) return;
//            if (!reader.Read(ref ItemTID)) return;
//            if (!reader.Read(ref Transcendance)) return;
//            if (!reader.Read(ref Reward)) return;
//        }
//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(ItemUID)) return;
//            if (!writer.Write(ItemTID)) return;
//            if (!writer.Write(Transcendance)) return;
//            if (!writer.Write(Reward)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Transcendence_Item_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class PvP_Log : Base_Log, INetSerialize
//    {
//        public eTicketType Type = eTicketType.Ticket_Pvp;
//        public int Cost = 0;
//        public long PvP_Point = 0;

//        public PvP_Log()
//        {

//        }

//        public PvP_Log(eTicketType type , int cost , long pvp_point)
//        {
//            Type = type;
//            Cost = cost;
//            PvP_Point = pvp_point;
//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            ushort t = 0;
//            if (!reader.Read(ref t)) return;
//            Type = (eTicketType)t;
//            if (!reader.Read(ref Cost)) return;
//            if (!reader.Read(ref PvP_Point)) return;
//        }
//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write((ushort)Type)) return;
//            if (!writer.Write(Cost)) return;
//            if (!writer.Write(PvP_Point)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Transcendence_Item_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Breakthroug_Log : Base_Log, INetSerialize
//    {
//        public int Step;
//        public List<_DropInfo> Reward = new List<_DropInfo>();

//        public Breakthroug_Log()
//        {

//        }

//        public Breakthroug_Log(int step_id , List<_DropInfo> drop_list)
//        {
//            Step = step_id;
//            Reward = SCopy<List<_DropInfo>>.DeepCopy(drop_list);
//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref Step)) return;
//            int count = 0;
//            if (!reader.Read(ref count)) return;
//            for (int i = 0; i < count; ++i)
//            {
//                _DropInfo info = new _DropInfo();
//                if (!reader.Read(ref info)) return;
//                Reward.Add(info);
//            }
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(Step)) return;
//            if (!writer.Write(Reward.Count)) return;
//            foreach (var Data in Reward)
//            {
//                if (!writer.Write(Data)) return;
//            }
//        }
//#if OPERATION
//        public string ListToString(List<_DropInfo> droplist)
//        {
//            StringBuilder builder = new StringBuilder();
//            for(int i = 0; i < droplist.Count; i++)
//            {
//                _DropInfo info = droplist[i];
//                DropTableData dropdata = csTableManager.Instance.mDropTable.Find<DropTableData>(info.ToString());
//                switch(info.m_Type)
//                {
//                    case eObjectType.Item:
//                    {
//                        var data = csTableManager.Instance.mItemTable.Find<ItemTableData>(info.m_DropKey.ToString());
//                        if(data == null)
//                            builder.Append(info.m_DropKey.ToString());
//                        else
//                        {
//                            string stritem = csTableManager.Instance.GetTableString(data.mNameID);
//                            if(stritem == null)
//                                builder.Append(info.m_DropKey.ToString());
//                            else
//                            {
//                                builder.Append(stritem);
//                                builder.Append(":");
//                                builder.Append(info.m_DropCount);
//                                builder.Append(", ");
//                            }
//                        }

//                    }
//                    break;
//                    case eObjectType.Asset:
//                    {
//                        eAssetType type = (eAssetType)info.m_DropKey;
//                        long count = info.m_DropCount;
//                        builder.Append(type.ToString());
//                        builder.Append(":");
//                        builder.Append(count.ToString());
//                        builder.Append(", ");
//                    }
//                    break;
//                    case eObjectType.Link:
//                    break;
//                    case eObjectType.Ticket:
//                    {
//                        eTicketType type = (eTicketType)info.m_DropKey;
//                        long count = info.m_DropCount;
//                        builder.Append(type.ToString());
//                        builder.Append(":");
//                        builder.Append(count.ToString());
//                        builder.Append(", ");
//                    }
//                    break;
//                    case eObjectType.Max:
//                    break;
//                    default:
//                    break;
//                }
//            }

//            return builder.ToString();
//        }

//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Use_Item_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                if(info.Name == "Reward")
//                    builder.Append(ListToString(Reward));
//                else
//                    builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Guild_Create_Log : Base_Log, INetSerialize
//    {
//        public string m_GuildName;

//        public Guild_Create_Log()
//        {

//        }

//        public Guild_Create_Log(string guild_name)
//        {
//            m_GuildName = guild_name;
//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref m_GuildName)) return;
//        }
//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(m_GuildName)) return;
//        }

//#if OPERATION
//        public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Transcendence_Item_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }

//    public class Guild_Jackpot_Log : Base_Log, INetSerialize
//    {
//        public int Guild_UID;
//        public int Jackpot_ID;
//        public int Jackpot_Value;

//        public Guild_Jackpot_Log()
//        {

//        }

//        public Guild_Jackpot_Log(int guild_uid , int jackpot_id , int jackpot_value)
//        {
//            Guild_UID = guild_uid;
//            Jackpot_ID = jackpot_id;
//            Jackpot_Value = jackpot_value;
//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref Guild_UID)) return;
//            if (!reader.Read(ref Jackpot_ID)) return;
//            if (!reader.Read(ref Jackpot_Value)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(Guild_UID)) return;
//            if (!writer.Write(Jackpot_ID)) return;
//            if (!writer.Write(Jackpot_Value)) return;
//        }
//#if OPERATION
//               public override string ToString()
//        {
//            StringBuilder builder = new StringBuilder();
//            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;
//            foreach(System.Reflection.FieldInfo info in typeof(Transcendence_Item_Log).GetFields(bindingFlags))
//            {
//                builder.Append("[");
//                builder.Append(info.Name);
//                builder.Append(" : ");
//                builder.Append(info.GetValue(this));
//                builder.Append("]");
//            }

//            return builder.ToString();
//        }
//#endif
//    }


//    public class Auction_Log : Base_Log, INetSerialize
//    {
//        public List<_AuctionItem> Items = new List<_AuctionItem>();
//        public CRewardLogData Reward = new CRewardLogData();

//        public Auction_Log()
//        {

//        }


//        public Auction_Log(_AuctionItem items)
//        {
//            Items.Add(items);
//        }

//        public Auction_Log(List<_AuctionItem> item_list)
//        {
//            Items = item_list;
//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            int count = 0;
//            if (!reader.Read(ref count)) return;
//            for (int i = 0; i < count; ++i)
//            {
//                _AuctionItem item = new _AuctionItem();
//                if (!reader.Read(ref item)) return;
//                Items.Add(item);
//            }

//            if (!reader.Read(ref Reward)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(Items.Count)) return;
//            foreach (var data in Items)
//            {
//                if (!writer.Write(data)) return;
//            }

//            if (!writer.Write(Reward)) return;
//        }
//    }

//    public class Transfer_Log : Base_Log, INetSerialize
//    {
//        public _ItemData ExtractingItem = new _ItemData();
//        public _ItemData TransferringItem = new _ItemData();

//        public Transfer_Log()
//        {

//        }

//        public Transfer_Log(_ItemData extract_item, _ItemData Transfer_item)
//        {
//            ExtractingItem = extract_item;
//            TransferringItem = Transfer_item;
//        }


//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref ExtractingItem)) return;
//            if (!reader.Read(ref TransferringItem)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(ExtractingItem)) return;
//            if (!writer.Write(TransferringItem)) return;
//        }
//    }

//    public class Raid_Reward_Log : Base_Log, INetSerialize
//    {
//        public CRewardLogData Reward = new CRewardLogData();

//        public Raid_Reward_Log()
//        {

//        }


//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref Reward)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(Reward)) return;
//        }
//    }

//    public class CRewardLogData : INetSerialize
//    {
//        public List<_ItemData> Item_list_insert = new List<_ItemData>();
//        public List<_ItemData> Item_list_update_level = new List<_ItemData>();
//        public List<_AssetData> Asset_list = new List<_AssetData>();
//        public List<_TicketData> Ticket_list = new List<_TicketData>();

//        public CRewardLogData()
//        {

//        }

//        public CRewardLogData(List<_ItemData> item_list_insert, List<_ItemData> item_list_update_level, List<_AssetData> asset_list, List<_TicketData> ticket_list)
//        {
//            Item_list_insert = item_list_insert;
//            Item_list_update_level = item_list_update_level;
//            Asset_list = asset_list;
//            Ticket_list = ticket_list;
//        }

//        public void Read(SNetReader reader)
//        {
//            int count = 0;
            
//            if (!reader.Read(ref count)) return;
//            for (int i = 0; i < count; ++i)
//            {
//                _ItemData data = new _ItemData();
//                if (!reader.Read(ref data)) return;
//                Item_list_insert.Add(data);
//            }

//            if (!reader.Read(ref count)) return;
//            for (int i = 0; i < count; ++i)
//            {
//                _ItemData data = new _ItemData();
//                if (!reader.Read(ref data)) return;
//                Item_list_update_level.Add(data);
//            }

//            if (!reader.Read(ref count)) return;
//            for (int i = 0; i < count; ++i)
//            {
//                _AssetData data = new _AssetData();
//                if (!reader.Read(ref data)) return;
//                Asset_list.Add(data);
//            }

//            if (!reader.Read(ref count)) return;
//            for (int i = 0; i < count; ++i)
//            {
//                _TicketData data = new _TicketData();
//                if (!reader.Read(ref data)) return;
//                Ticket_list.Add(data);
//            }
//        }

//        public void Write(SNetWriter writer)
//        {
//            if (!writer.Write(Item_list_insert.Count)) return;
//            foreach (var data in Item_list_insert)
//            {
//                if (!writer.Write(data)) return;
//            }

//            if (!writer.Write(Item_list_update_level.Count)) return;
//            foreach (var data in Item_list_update_level)
//            {
//                if (!writer.Write(data)) return;
//            }

//            if (!writer.Write(Asset_list.Count)) return;
//            foreach (var data in Asset_list)
//            {
//                if (!writer.Write(data)) return;
//            }

//            if (!writer.Write(Ticket_list.Count)) return;
//            foreach (var data in Ticket_list)
//            {
//                if (!writer.Write(data)) return;
//            }
//        }

//    }

//    public class Title_UnLock_Log : Base_Log, INetSerialize
//    {
//        public int TitleID = 0;
//        public CRewardLogData Reward = new CRewardLogData();

//        public Title_UnLock_Log()
//        {

//        }

//        public void Read(SNetReader reader)
//        {
//            base.Base_Read(reader);
//            if (!reader.Read(ref TitleID)) return;
//            if (!reader.Read(ref Reward)) return;
//        }

//        public void Write(SNetWriter writer)
//        {
//            base.Base_Write(writer);
//            if (!writer.Write(TitleID)) return;
//            if (!writer.Write(Reward)) return;
//        }
//    }
//}
