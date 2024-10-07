
CREATE procedure [dbo].[sp_character_quest_daily_update]
(
	@db_uid int = 0,
	@db_mission_json varchar(max) = '',
	@db_exp_time datetime2(0) = '',

	@sp_rtn int = 0 output, 
	@sp_msg varchar(128) = '' output
)
as
	SET XACT_ABORT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	BEGIN TRY
		declare @utc_date datetime2 = sysutcdatetime();
		begin transaction
		merge dbo.tb_character_quest_daily as t_cqd
			using 
			(
				select ID, Idx, Val, State
				from openjson(@db_mission_json) with
					(
						ID int, Idx int, Val bigint, State tinyint
					)
			) as s_cqr(ID, Idx, Val, State) 
			on t_cqd.uid = @db_uid and
				t_cqd.id = s_cqr.ID
			when matched then
				update set	t_cqd.idx = s_cqr.idx,
							t_cqd.val = s_cqr.Val,
							t_cqd.state = s_cqr.State,
							t_cqd.exp_time = @db_exp_time,
							dw_update_time = @utc_date
			when not matched by target then
				insert (uid, id, idx, val, state, exp_time, dw_update_time)
				values (@db_uid, s_cqr.ID, s_cqr.Idx, s_cqr.Val, s_cqr.State, @db_exp_time, @utc_date);

		commit transaction 
		
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