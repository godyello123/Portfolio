



CREATE procedure [dbo].[sp_character_quest_pass_update]
(
	@db_uid int = 0,
	@db_mission_json nvarchar(max) = '',
	@db_quest_id varchar(max) = '',
	@db_pass_active bit,

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

			merge dbo.tb_character_quest_pass as t_cqp
			using 
			(
				select ID, Idx, Val, State, PassRewarded
				from openjson(@db_mission_json) with
					(
						ID int, Idx int, Val bigint, State tinyint, PassRewarded bit
					)
			) as s_cqp(ID, Idx, Val, State, PassRewarded) 
			on t_cqp.uid = @db_uid and
				t_cqp.id = s_cqp.ID and
				t_cqp.quest_id = @db_quest_id
			when matched then
				update set	t_cqp.idx = s_cqp.idx,
							t_cqp.val = s_cqp.Val,
							t_cqp.state = s_cqp.State,
							t_cqp.pass_rewarded = s_cqp.PassRewarded,
							t_cqp.pass_active = @db_pass_active,
							dw_update_time = @utc_date
			when not matched by target then
				insert (uid, quest_id, id, idx, val, state, pass_rewarded, pass_active, dw_update_time)
				values (@db_uid, @db_quest_id, s_cqp.ID, s_cqp.Idx, s_cqp.Val, s_cqp.State, s_cqp.PassRewarded, @db_pass_active, @utc_date);

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