


CREATE procedure [dbo].[sp_system_load]
(
	@uid int = 0,
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

		--block user
		select device_id, cnt, exp_time
		from dbo.tb_system_block_user
		where @utc_time < exp_time 

		--white user
		select device_id, create_time
		from dbo.tb_system_white_user
		
		--notice
		select notice_id, msg, begin_time, expire_time, loop, term
		from dbo.tb_system_notice
		
		--servicetype
		select top(1)type
		from dbo.tb_system_service_type

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