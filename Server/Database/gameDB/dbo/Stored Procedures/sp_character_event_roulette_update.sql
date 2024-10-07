


create procedure [dbo].[sp_character_event_roulette_update]
(
	@uid int = 0,
	@event_uid bigint = 0,
	@exclude_list varchar(max) = '',

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

			merge dbo.tb_character_event_roulette as t_cer
				using ( select @uid as uid ) as s_cs
				on t_cer.uid = s_cs.uid and t_cer.event_uid = @event_uid
				when matched then
					update set	exclude_list = @exclude_list,
								dw_update_time = @utc_date
				when not matched by target then
					insert (uid, event_uid, exclude_list, dw_update_time)
					values (@uid, @event_uid, @exclude_list, @utc_date);

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