


CREATE procedure [dbo].[sp_character_gacha]
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

			merge dbo.tb_character_gacha as t_cg
				using ( select @uid as uid ) as s_cg
				on t_cg.uid = s_cg.uid and t_cg.id = @gacha_id
				when matched then
					update set	lv = @gacha_lv,
								exp = @gacha_exp,
								rewarded = @gacha_rewarded,
								dw_update_time = @utc_date
				when not matched by target then
					insert (uid, id, lv, exp, rewarded, dw_update_time)
					values (@uid, @gacha_id, @gacha_lv, @gacha_exp, @gacha_rewarded, @utc_date);

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