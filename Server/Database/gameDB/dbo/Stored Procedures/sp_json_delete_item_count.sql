



CREATE procedure [dbo].[sp_json_delete_item_count]
(
	@uid bigint,
	@ref_json varchar(max),

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

			delete from dbo.tb_character_item
			where uid = @uid and item_id in
			(
				select ItemID
				from openjson (@ref_json) with
				(
					ItemID bigint
				)
			)

		commit transaction

		select	@sp_rtn = 0,
				@sp_msg = 'success';
		return;

	END TRY
	BEGIN CATCH
		select	@sp_rtn = ERROR_NUMBER(), @sp_msg = ERROR_MESSAGE();
		exec dbo.sp_sys_error_log_create;
		return;
	END CATCH