

CREATE procedure [dbo].[sp_character_item_sell]
(
	@uid int = 0,
	@update_coins nvarchar(max) = '',
	@update_items varchar(max) = '',
	@sp_rtn int = 0 output, 
	@sp_msg varchar(128) = '' output
)
as
	SET XACT_ABORT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	BEGIN TRY
		declare @utc_date datetime2 = sysutcdatetime();
		declare @rand_option varchar(512) = ''

		begin transaction

			if LEN(@update_coins) > 0
			begin 
				exec dbo.sp_json_update_coin_count
						@uid = @uid, @ref_json = @update_coins,
						@sp_rtn = @sp_rtn out, @sp_msg = @sp_msg out;

				if @sp_rtn <> 0 
				begin 
					if XACT_STATE() <> 0 rollback transaction;
					return;
				end
			end

			if LEN(@update_items) > 0
			begin
				exec dbo.sp_json_update_item_count
					@uid = @uid, @ref_json = @update_items,
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