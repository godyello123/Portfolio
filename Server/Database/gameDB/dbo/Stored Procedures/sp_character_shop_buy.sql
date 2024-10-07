
create procedure [dbo].[sp_character_shop_buy]
(
	@uid bigint,
	
	@record_id bigint,
	@bought_cnt int,
	@reset_time bigint,

	@update_coins varchar(max) = '',
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
		declare @curr_time bigint = (datediff(s, '1970-01-01', @utc_date));

		begin transaction

			--shop
			merge dbo.tb_character_shop as t_cs
			using ( select @uid as uid ) as s_crr
			on t_cs.uid = s_crr.uid and t_cs.shop_id = @record_id
			when matched then
				update set	[limit_count] = @bought_cnt,
							reset_time = @reset_time,
							dw_update_time = @utc_date
			when not matched by target then
				insert (uid, shop_id, limit_count, reset_time, dw_period_reward_time, dw_update_time)
				values (@uid, @record_id, @bought_cnt, @reset_time, @curr_time, @utc_date);

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


		
			if @sp_rtn <> 0
			begin
				if XACT_STATE() <> 0 rollback transaction;
				return;
			end

		commit transaction;

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