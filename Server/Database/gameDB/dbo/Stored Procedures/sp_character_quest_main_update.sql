
CREATE procedure [dbo].[sp_character_quest_main_update]
(
	@db_uid int = 0,
	@db_mission_id int = 0,
	@db_mission_idx int = 0,
	@db_mission_val bigint = 0,
	@db_mission_state tinyint = 0,
	
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
				merge dbo.tb_character_quest_main as t_cm
					using ( select @db_uid as uid ) as s_cm
					on t_cm.uid = s_cm.uid and t_cm.id = @db_mission_id
					when matched then
						update set	idx = @db_mission_idx,
									val = @db_mission_val,
									state = @db_mission_state,
									dw_update_time = @utc_date
					when not matched by target then
						insert (uid, id, idx, val, state, dw_update_time)
						values (@db_uid, @db_mission_id, @db_mission_idx, @db_mission_val, @db_mission_state, @utc_date);

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