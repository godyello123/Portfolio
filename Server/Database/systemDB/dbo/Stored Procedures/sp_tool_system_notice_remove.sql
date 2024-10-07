


CREATE procedure [dbo].[sp_tool_system_notice_remove]
(
	@notice_id bigint = 0,
	
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
		DECLARE @InsertedId INT;

		begin transaction

		delete from dbo.tb_system_notice
		where notice_id = @notice_id

		select	@sp_rtn = 0, 
				@sp_msg = 'success';

		commit transaction
		return;
	END TRY
	BEGIN CATCH
		if XACT_STATE() <> 0 rollback transaction;
		select	@sp_rtn = ERROR_NUMBER(), @sp_msg = ERROR_MESSAGE();
		exec dbo.sp_sys_error_log_create;
		return;
	END CATCH