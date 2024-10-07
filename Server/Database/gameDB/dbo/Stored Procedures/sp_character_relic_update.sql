﻿




CREATE procedure [dbo].[sp_character_relic_update]
(
	@uid int = 0,
	@group_id int = 0,
	@lv int = 0,
	@bonus_prob int = 0,

	@update_items varchar(max) = '',
	@update_coins varchar(max) = '',
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

			merge dbo.tb_character_relic as t_cr
				using ( select @uid as uid ) as s_cs
				on t_cr.uid = s_cs.uid and t_cr.group_id = @group_id 
				when matched then
					update set	lv = @lv,
								bonus_prob = @bonus_prob,
								dw_update_time = @utc_date
				when not matched by target then
					insert (uid, group_id, lv, bonus_prob, dw_update_time)
					values (@uid, @group_id, @lv, @bonus_prob, @utc_date);

					--update coin
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

			--update item
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