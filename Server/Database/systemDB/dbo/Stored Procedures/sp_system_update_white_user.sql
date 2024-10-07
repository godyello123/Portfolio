


CREATE procedure [dbo].[sp_system_update_white_user]
(
	@device_id varchar(128) = 0,
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

			merge dbo.tb_system_white_user as t_swu
				using ( select @device_id as device_id ) as s_swu
				on t_swu.device_id = s_swu.device_id
				when not matched by target then
					insert (device_id, create_time, dw_update_time)
					values (@device_id, @utc_date, @utc_date);

			select device_id, create_time
			from dbo.tb_system_white_user
			where device_id = @device_id
		

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