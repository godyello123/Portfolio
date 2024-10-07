



CREATE procedure [dbo].[sp_character_gold_growth_update]
(
	@uid int = 0,
	@update_coin nvarchar(256) = '',
	@type int =0,
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
		
			merge dbo.tb_character_growth_gold as t_cc
				using ( select @uid as uid ) as s_cc
				on t_cc.uid = s_cc.uid and t_cc.id = @type
				when matched then
					update set	value = @value,
								dw_update_time = @utc_date
				when not matched by target then
					insert (uid, id, value, dw_update_time)
					values (@uid, @type, @value, @utc_date);

					--asset update;
			if LEN(@update_coin) > 0
				begin 
					exec dbo.sp_json_update_coin_count
							@uid = @uid, @ref_json = @update_coin,
							@sp_rtn = @sp_rtn out, @sp_msg = @sp_msg out;

					if @sp_rtn <> 0 
					begin 
						if XACT_STATE() <> 0 rollback transaction;
						return;
					end
				end

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