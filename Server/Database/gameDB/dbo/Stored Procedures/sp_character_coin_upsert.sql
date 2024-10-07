

CREATE procedure [dbo].[sp_character_coin_upsert]
(
	@uid int = 0,
	@type nvarchar(32) = '',
	@value bigint = 0,
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

			merge dbo.tb_character_coin as t_cc
			using ( select @uid as uid ) as s_cc
			on t_cc.uid = s_cc.uid and t_cc.type = @type
			when matched then
				update set	value = @value,
							dw_update_time = @utc_date
			when not matched by target then
				insert (uid, type, value, dw_update_time)
				values (@uid, @type, @value, @utc_date);
		
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