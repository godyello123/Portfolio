
CREATE procedure [dbo].[sp_character_quest_checkIn_update]
(
	@db_uid int = 0,
	@db_quest_id varchar(max) = '',
	@db_mission_id int = 0,
	@db_mission_idx int = 0,
	@db_mission_val bigint = 0,
	@db_mission_state tinyint = 0,
	@db_exp_time datetime2(0),

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

		merge dbo.tb_character_quest_checkin as t_cqc
		using ( select @db_uid as uid ) as s_cqc
		on t_cqc.uid = s_cqc.uid and t_cqc.quest_id = @db_quest_id
		when matched then
			update set	id = @db_mission_id,
						idx = @db_mission_idx,
						val = @db_mission_val,
						state = @db_mission_state,
						exp_time = @db_exp_time,
						dw_update_time = @utc_date
		when not matched by target then
			insert (uid, quest_id, id, idx, val, state, exp_time ,dw_update_time)
			values (@db_uid, @db_quest_id, @db_mission_id, @db_mission_idx, @db_mission_val, @db_mission_state, @db_exp_time, @utc_date);

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