

CREATE procedure [dbo].[sp_character_coupon_use]
(
	@uid int = 0,
	@coupon_id nvarchar(max) = '',
	@used_coupon_json nvarchar(max) = '',
	@update_items nvarchar(max) = '',
	@update_coins nvarchar(max) = '',
	
	@sp_rtn int = 0 output, 
	@sp_msg varchar(128) = '' output
)
as
	SET XACT_ABORT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	BEGIN TRY
		declare @utc_date datetime2 = sysutcdatetime();
		declare @utc_time bigint = DATEDIFF(SECOND,{d '1970-01-01'}, @utc_date)
		
		begin transaction
			
			declare @is_exists bit = 
			(
				case when exists 
				(
					select 1
					from dbo.tb_system_coupon 
					where coupon_id = @coupon_id
					and cnt > 0
					and @utc_time between begin_time and expire_time
				) then 1 else 0 end
			)

			if @is_exists = 0
			begin
				select @sp_rtn = 1, @sp_msg ='fail_coupon'
				return;
			end

			update dbo.tb_system_coupon
			set cnt -= 1
			where coupon_id = @coupon_id

			merge dbo.tb_character_coupon as t_cc
				using ( select @uid as uid ) as s_cc
				on t_cc.uid = s_cc.uid
				when matched then
					update set	coupon_json = @used_coupon_json,
								dw_update_time = @utc_date
				when not matched by target then
					insert (uid, coupon_json, dw_update_time)
					values (@uid, @used_coupon_json, @utc_date);

			
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