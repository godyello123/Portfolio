

CREATE procedure [dbo].[sp_character_rank_reward_update]
(
	@uid int = 0,
	@rank_type tinyint = 0,
	@reward_state tinyint =0,
	@rank_val int = 0,
	@exp_time bigint = 0,

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
		merge dbo.tb_character_rank_reward as t_crr
				using ( select @uid as uid ) as s_crr
				on t_crr.uid = s_crr.uid and t_crr.type = @rank_type
				when matched then
					update set	state = @reward_state,
								val = @rank_val,
								exp_time = @exp_time,
								dw_update_time = @utc_date
				when not matched by target then
					insert (uid, type, state, val, exp_time, dw_update_time)
					values (@uid, @rank_type, @reward_state, @rank_val, @exp_time, @utc_date);

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