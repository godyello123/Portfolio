




CREATE procedure [dbo].[sp_system_load_schedule]
(
	@sp_rtn int = 0 output, 
	@sp_msg varchar(128) = '' output
)
as
	SET XACT_ABORT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	BEGIN TRY
		declare @utc_date datetime2 = sysutcdatetime();
		
		select uid, type, param, val, start_date, end_date,
		day_week, start_time, end_time
		from dbo.tb_system_schedule

		--join dbo.tb_character_item_preset as cip
		--on cip.account_id = cs.account_id and cip.type = 0 and cip.no = 0
		--left join dbo.tb_guild_member as gm
		--on gm.account_id = cs.account_id
		--where 
		--	cs.stage_type = @db_stage_type and 
		--	cs.final_stage_tid >= @db_min_value and 
		--	cs.dw_update_time >= @final_date
		--order by 
		--	cs.final_stage_tid desc, cs.dw_final_stage_time asc
		--offset @page_offset rows
		--fetch next @page_fetch_cnt rows only;

		----rank
		--select min(dw_create_time) as dw_create_time
		--from tb_character_main_reward

		----set define ?
		--declare @sys_define_key int = 1;

		--update dbo.tb_system_define
		--set rank_sort_date = @final_date
		--where [index] = @sys_define_key

		--if @@ROWCOUNT = 0
		--begin 
		--	insert into dbo.tb_system_define([index], rank_sort_date)
		--	values(@sys_define_key, @final_date)
		--end

		select	@sp_rtn = 0, 
				@sp_msg = 'success';
		return;
	END TRY
	BEGIN CATCH
		select	@sp_rtn = ERROR_NUMBER(), @sp_msg = ERROR_MESSAGE();
		exec dbo.sp_sys_error_log_create;
		return;
	END CATCH