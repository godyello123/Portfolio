

CREATE procedure [dbo].[sp_character_gacha_level_rewared]
(
	@uid int = 0,

	@update_items nvarchar(max) = '',
	@update_coins nvarchar(max) = '',

	@gacha_id int = 0,
	@gacha_lv int= 0,
	@gacha_exp bigint = 0,
	@gacha_rewarded int = 0,
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

			update dbo.tb_character_gacha
			set lv = @gacha_lv, exp = @gacha_exp, rewarded = @gacha_rewarded
			where uid = @uid and id = @gacha_id
			
			if @@ROWCOUNT = 0 --not found case
			begin
				if XACT_STATE() <> 0 rollback transaction;
					select	@sp_rtn = -1, 
							@sp_msg = 'not found data';
					return;
			end

			--item update
			if LEN(@update_items) > 0
			begin
				exec dbo.sp_json_update_item_count
				@uid = @uid, @ref_json = @update_items,
				@sp_rtn = @sp_rtn out , @sp_msg = @sp_msg out

				if @sp_rtn <> 0 
					begin 
						if XACT_STATE() <> 0 rollback transaction;
						return;
					end
			end
			
			--coin update
			if LEN(@update_coins) > 0
			begin
				exec dbo.sp_json_update_coin_count
				@uid = @uid, @ref_json =@update_coins,
				@sp_rtn = @sp_rtn out, @sp_msg = @sp_msg out

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