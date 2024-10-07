



CREATE procedure [dbo].[sp_tool_character_post_load]
(
	@user_id bigint = 0,
	@start_time bigint = 0,
	@end_time bigint = 0,
	
	@sp_rtn int = 0 output, 
	@sp_msg varchar(128) = '' output
)
as
	SET XACT_ABORT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	BEGIN TRY
		declare @utc_date datetime2 = sysutcdatetime();
		declare @utc_time bigint = DATEDIFF(SECOND,{d '1970-01-01'}, @utc_date)
		
		select id, type, title, msg, is_read, is_reward, begin_time, expire_time, reward
		from dbo.tb_character_post with(nolock)
		where uid = @user_id
		and begin_time between @start_time and @end_time
		
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