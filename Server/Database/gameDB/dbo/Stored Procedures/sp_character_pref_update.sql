



CREATE procedure [dbo].[sp_character_pref_update]
(
	@uid int = 0,
	@type tinyint = 0,
	@strval nvarchar(1024) = '',
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

			merge dbo.tb_character_pref as t_cs
				using ( select @uid as uid ) as s_cs
				on t_cs.uid = s_cs.uid and t_cs.type = @type
				when matched then
					update set	strval = @strval,
								dw_update_time = @utc_date
				when not matched by target then
					insert (uid, type, [strVal], dw_update_time)
					values (@uid, @type, @strval, @utc_date);

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