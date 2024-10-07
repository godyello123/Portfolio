


CREATE procedure [dbo].[sp_tool_system_block_user_upsert]
(
	@block_str_json varchar(max) ='',

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
			merge dbo.tb_system_block_user as t_sbu
				using 
				(
				select DeviceID, Count, ExpTime
				from openjson(@block_str_json) with
					(
						DeviceID nvarchar(128), Count int, ExpTime bigint
					)
				) as s_sbu(DeviceID, Count, ExpTime)
				on t_sbu.device_id = s_sbu.DeviceID
				when matched then
				update set cnt += 1,
						exp_time = s_sbu.ExpTime
				when not matched by target then
					insert (device_id, cnt, exp_time ,dw_update_time)
					values (s_sbu.DeviceID, 1, s_sbu.ExpTime, @utc_date);
		
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