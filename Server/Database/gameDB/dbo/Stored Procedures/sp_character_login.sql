



CREATE procedure [dbo].[sp_character_login]
(
	@uid bigint = 0,
	@sp_rtn int = 0 output, 
	@sp_msg varchar(128) = '' output
)
as
	SET XACT_ABORT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	BEGIN TRY
		declare @utc_date datetime2 = sysutcdatetime();
		declare @login_time bigint = DATEDIFF(SECOND,{d '1970-01-01'}, @utc_date)
		
		--logintime update
		update dbo.tb_character
		set login_time = @login_time
		where uid = @uid

		--info
		select uid, device_id, name, event_stage , level, exp, level_point, ad_skip, profile_id, login_time, logout_time
		from dbo.tb_character
		where uid= @uid;
		
		--stage
		select type, cur_tid, max_tid, total_cnt, is_loop
		from dbo.tb_character_stage
		where uid = @uid;

		--coin
		select type, value
		from dbo.tb_character_coin
		where uid = @uid

		--growth gold
		select id, value
		from dbo.tb_character_growth_gold
		where uid = @uid

		--growth level
		select id, value
		from dbo.tb_character_growth_level
		where uid = @uid

		--quest main
		select id, idx, val, state
		from dbo.tb_character_quest_main
		where uid = @uid
		
		--itme
		select item_id, table_id, level, count, in_used, random_option
		from dbo.tb_character_item
		where uid = @uid

		--item equip preset
		select idx, equip_slot, is_enable
		from dbo.tb_character_equip_preset
		where uid = @uid

		--pref
		select type, strval
		from dbo.tb_character_pref
		where uid = @uid

		--knight preset
		select idx, equip_slot, is_enable
		from dbo.tb_character_knight_preset
		where uid = @uid

		--gacha level
		select id, lv, exp, rewarded
		from dbo.tb_character_gacha
		where uid = @uid

		--skill
		select group_id, tid, level, is_learend
		from dbo.tb_character_skill
		where uid = @uid

		--skill preset
		select idx, equip_slot, is_enable
		from dbo.tb_character_skill_preset
		where uid = @uid

		--post
		--insert into dbo.tb_character_post
		--( 
		--	uid, id, type, title, 
		--	msg, is_read, is_reward, begin_time, expire_time, 
		--	reward, dw_update_time
		--)		
		--select
		--    @uid, sp.id, sp.type, sp.title, 
		--	sp.msg, 'FALSE' as read_flag, 'FALSE' as reward_flag, sp.begin_time, sp.expire_time, 
		--    sp.reward, sp.dw_update_time
		--from dbo.tb_system_post as sp
		--left outer join dbo.tb_character_post as cp
		--on cp.id = sp.id and cp.type = sp.type and cp.uid = @uid
		--where cp.id is null
		--and sp.begin_time < @login_time
		--and @login_time < sp.expire_time;

		delete from dbo.tb_character_post
		where uid = @uid and expire_time <= @login_time

		--select id, type, title, msg, is_read, is_reward, begin_time, expire_time, reward
		--from dbo.tb_character_post
		--where uid = @uid

		--coupon
		select coupon_json
		from dbo.tb_character_coupon
		where uid = @uid

		--rank reward
		select type, state, val, exp_time
		from dbo.tb_character_rank_reward
		where uid = @uid

		--quest repeat
		select id, idx, val, state, pass_rewarded
		from dbo.tb_character_quest_repeat
		where uid = @uid

		--quest daily
		select id, idx, val, state, exp_time
		from dbo.tb_character_quest_daily
		where uid = @uid

		--quest checkin
		select quest_id, id, idx, val, state, exp_time
		from dbo.tb_character_quest_checkin
		where uid = @uid

		--quest_pass
		select quest_id, id, idx, val , state, pass_rewarded, pass_active
		from dbo.tb_character_quest_pass
		where uid = @uid

		--relic
		select group_id, lv, bonus_prob
		from dbo.tb_character_relic
		where uid = @uid

		--adsbuff
		select buff_id, buff_lv, buff_exp, buff_exp_time, watch_exp_time, watch_count
		from dbo.tb_character_ads_buff
		where uid = @uid

		--stage skill
		select type, equip_slot
		from dbo.tb_character_stage_skill
		where uid = @uid
		
		--event
		select event_uid, event_id, start_date, end_date
		from dbo.tb_character_event
		where uid = @uid

		--quest event
		select id, quest_id, idx, val, state, exp_time
		from dbo.tb_character_quest_event
		where uid = @uid

		--event shop
		select event_uid, shop_id, limit_count
		from dbo.tb_character_event_shop
		where uid = @uid

		--event roulette
		select event_uid, exclude_list
		from dbo.tb_character_event_roulette
		where uid = @uid

		select	@sp_rtn = 0, 
				@sp_msg = 'success';
		return;
	END TRY
	BEGIN CATCH
		if XACT_STATE() <> 0 rollback transaction;
		select	@sp_rtn = ERROR_NUMBER(), @sp_msg = ERROR_MESSAGE();
		exec dbo.sp_sys_error_log_create;
		return;
	END CATCH