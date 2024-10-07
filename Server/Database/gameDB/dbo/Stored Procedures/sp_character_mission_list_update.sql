


create procedure [dbo].[sp_character_mission_list_update]
(
	@uid int = 0,
	@mission_json nvarchar(max) = '',

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
		merge dbo.tb_character_mission as t_cm
			using 
			(
				select ID, Idx, Val, State, PassRewarded
				from openjson(@mission_json) with
					(
						ID int, Idx int, Val bigint, State tinyint, PassRewarded bit
					)
			) as s_cm(ID,Idx, Val, State, PassRewarded)
			on t_cm.uid = @uid and
				t_cm.ID = s_cm.ID
			when matched then
				update set	t_cm.idx = s_cm.idx, t_cm.val = s_cm.Val,
							t_cm.state = s_cm.State, t_cm.pass_rewarded = s_cm.PassRewarded,
							dw_update_time = @utc_date
			when not matched by target then
				insert 
				(
					uid, id, idx, val, state, pass_rewarded, dw_update_time
				)
				values 
				(
					@uid, s_cm.ID, s_cm.Idx, s_cm.Val, s_cm.State, s_cm.PassRewarded, @utc_date
				);
			
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