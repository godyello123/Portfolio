

create procedure [dbo].[sp_character_stage_save]
(
	@uid int = 0,
	@stage_type tinyint = 0,
	@cur_tid int = 0,
	@max_tid int = 0,
	@total_cnt bigint = 0,
	@is_loop bit = false,
	
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

			merge dbo.tb_character_stage as t_cs
				using ( select @uid as uid ) as s_cs
				on t_cs.uid = s_cs.uid and t_cs.type = @stage_type
				when matched then
					update set	cur_tid = @cur_tid,
								max_tid = @max_tid,
								total_cnt = @total_cnt,
								is_loop = @is_loop,
								dw_update_time = @utc_date
				when not matched by target then
					insert (uid, type, cur_tid, max_tid, total_cnt, is_loop, dw_update_time)
					values (@uid, @stage_type, @cur_tid, @max_tid, @total_cnt, @is_loop, @utc_date);

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