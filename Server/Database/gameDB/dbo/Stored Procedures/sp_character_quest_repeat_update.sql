
CREATE procedure [dbo].[sp_character_quest_repeat_update]
(
	@db_uid int = 0,
	@db_mission_json varchar(max) = '',

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
		merge dbo.tb_character_quest_repeat as t_cqr
			using 
			(
				select ID, Idx, Val, State, PassRewarded
				from openjson(@db_mission_json) with
					(
						ID int, Idx int, Val bigint, State tinyint, PassRewarded bit
					)
			) as s_cqr(ID, Idx, Val, State, PassRewarded) 
			on t_cqr.uid = @db_uid and
				t_cqr.id = s_cqr.ID
			when matched then
				update set	t_cqr.idx = s_cqr.idx,
							t_cqr.val = s_cqr.Val,
							t_cqr.state = s_cqr.State,
							t_cqr.pass_rewarded = s_cqr.PassRewarded,
							dw_update_time = @utc_date
			when not matched by target then
				insert (uid, id, idx, val, state, pass_rewarded,dw_update_time)
				values (@db_uid, s_cqr.ID, s_cqr.Idx, s_cqr.Val, s_cqr.State, s_cqr.PassRewarded, @utc_date);

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