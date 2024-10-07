



CREATE procedure [dbo].[sp_character_level_growth_update]
(
	@uid int = 0,
	@type int =0,
	@value bigint = 0,
	@level_point int = 0,
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
				--growth_level upsert
				merge dbo.tb_character_growth_level as t_cc
					using ( select @uid as uid ) as s_cc
					on t_cc.uid = s_cc.uid and t_cc.id = @type
					when matched then
						update set	value = @value,
									dw_update_time = @utc_date
					when not matched by target then
						insert (uid, id, value, dw_update_time)
						values (@uid, @type, @value, @utc_date);

				--user info
				update dbo.tb_character
				set level_point = @level_point 
				where uid = @uid 

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