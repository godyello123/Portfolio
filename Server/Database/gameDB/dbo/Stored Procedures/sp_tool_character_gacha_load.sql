




CREATE procedure [dbo].[sp_tool_character_gacha_load]
(
	@uid bigint = 0,
	@deviceid nvarchar(max) = '',
	@name nvarchar(max) = '',

	@sp_rtn int = 0 output, 
	@sp_msg varchar(128) = '' output
)
as
	SET XACT_ABORT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	BEGIN TRY
		declare @utc_date datetime2 = sysutcdatetime();
		declare @login_time bigint = DATEDIFF(SECOND,{d '1970-01-01'}, @utc_date)
		
		--gacha level
		select id, lv, exp, rewarded
		from dbo.tb_character_gacha
		where uid = @uid
	
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