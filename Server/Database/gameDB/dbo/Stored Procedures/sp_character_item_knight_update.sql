

CREATE procedure [dbo].[sp_character_item_knight_update]
(
	@uid bigint = 0,
	@itemid bigint = 0,
	@in_used bit = 0,
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

			update dbo.tb_character_item
			set in_used = @in_used
			where uid = @uid and item_id = @itemid

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