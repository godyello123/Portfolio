

CREATE procedure [dbo].[sp_character_view_user_info]
(
	@uid int = 0,

	@sp_rtn int = 0 output, 
	@sp_msg varchar(128) = '' output
)
as
	SET XACT_ABORT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	BEGIN TRY
		declare @utc_date datetime2 = sysutcdatetime();
		declare @stage_type tinyint = 0;
		
		select tc.name, tc.level, tc.profile_id, tcs.max_tid
		from dbo.tb_character as tc with(nolock)
		join dbo.tb_character_stage as tcs with(nolock)
		on tcs.uid = tc.uid and tcs.type = @stage_type
		where tc.uid = @uid
		
		--item preset
		select 
			ci.table_id as item_tid,
			ci.level as item_lv,
			ci.random_option as item_options,
			sub.*
		from dbo.tb_character_item as ci with(nolock)
		join 
		(
			select 
				uid,
				JSON_VALUE(json_equipped_data.value, '$.ID') as item_uid
			from dbo.tb_character_equip_preset as cip with(nolock)
			cross apply openjson(cip.equip_slot) as json_equipped_data
			where uid = @uid and is_enable = 1
		) as sub
		on ci.uid = sub.uid and ci.item_id = sub.item_uid
		
		--knight preset
		select 
			ci.table_id as item_tid,
			ci.level as item_lv,
			ci.random_option as item_options,
			sub.*
		from dbo.tb_character_item as ci with(nolock)
		join 
		(
			select 
				uid,
				JSON_VALUE(json_equipped_data.value, '$.ID') as item_uid
			from dbo.tb_character_knight_preset as ckp with(nolock)
			cross apply openjson(ckp.equip_slot) as json_equipped_data
			where uid = @uid and is_enable = 1
		) as sub
		on ci.uid = sub.uid and ci.item_id = sub.item_uid
		
		--skill preset
		select 
			cs.group_id, cs.tid, cs.level,
			sub.*
		from dbo.tb_character_skill as cs with(nolock)
		join 
		(
			select 
				uid,
				JSON_VALUE(json_equipped_data.value, '$.ID') as skill_groupID
			from dbo.tb_character_skill_preset as csp with(nolock)
			cross apply openjson(csp.equip_slot) as json_equipped_data
			where uid = @uid and is_enable = 1
		) as sub
		on cs.uid = sub.uid and cs.group_id = sub.skill_groupID

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