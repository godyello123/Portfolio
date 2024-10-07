



CREATE procedure [dbo].[sp_character_quest_main_rewarded]
(
	@uid int = 0,
	@remove_mission_id int = 0,
	@mission_id int = 0,
	@mission_idx int = 0,
	@mission_val bigint = 0,
	@mission_state tinyint = 0,
	@update_coin nvarchar(max) = '',
	@update_item nvarchar(max) = '',
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

				update dbo.tb_character_quest_main
				set id = @mission_id, idx = @mission_idx, val = @mission_val,
				state = @mission_state, dw_update_time = @utc_date
				where uid = @uid and id = @remove_mission_id

				--update coin
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

				--update item
				if LEN(@update_item) > 0
					begin
						exec dbo.sp_json_update_item_count
							@uid = @uid, @ref_json = @update_item,
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