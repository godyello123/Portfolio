


CREATE procedure [dbo].[sp_system_update_block_user]
(
	@device_id varchar(128) = 0,
	@cnt int = 0,
	@block_exp_time bigint = 0,
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
				using ( select @device_id as device_id ) as s_sbu
				on t_sbu.device_id = s_sbu.device_id
				when matched then
				update set cnt += 1,
						exp_time = @block_exp_time
				when not matched by target then
					insert (device_id, cnt, exp_time ,dw_update_time)
					values (@device_id, @cnt, @block_exp_time, @utc_date);
		

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