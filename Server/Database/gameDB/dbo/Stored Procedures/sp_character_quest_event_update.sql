



CREATE procedure [dbo].[sp_character_quest_event_update]
(
	@db_uid int = 0,
	@db_mission_json nvarchar(max) = '',
	@db_quest_id nvarchar(max) = '',
	@db_exp_time DateTime2(0) = sysutcdatetime,

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

			merge dbo.tb_character_quest_event as t_cqe
			using 
			(
				select ID, Idx, Val, State
				from openjson(@db_mission_json) with
					(
						ID int, Idx int, Val bigint, State tinyint
					)
			) as s_cqp(ID, Idx, Val, State) 
			on t_cqe.uid = @db_uid and
				t_cqe.id = s_cqp.ID and
				t_cqe.quest_id = @db_quest_id
			when matched then
				update set	t_cqe.idx = s_cqp.idx,
							t_cqe.val = s_cqp.Val,
							t_cqe.state = s_cqp.State,
							t_cqe.exp_time = @db_exp_time,
							t_cqe.dw_update_time = @utc_date
			when not matched by target then
				insert (uid, id, quest_id, idx, val, state, exp_time, dw_update_time)
				values (@db_uid, s_cqp.ID, @db_quest_id, s_cqp.Idx, s_cqp.Val, s_cqp.State, @db_exp_time, @utc_date);

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