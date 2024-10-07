


CREATE procedure [dbo].[sp_character_mission_rewarded]
(
	@uid int = 0,
	@mission_id int = 0,
	@mission_idx int = 0,
	@mission_val bigint = 0,
	@mission_state tinyint = 0,
	@mission_pass_rewarded bit,
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
			--growth_level upsert
				merge dbo.tb_character_mission as t_cm
					using ( select @uid as uid ) as s_cm
					on t_cm.uid = s_cm.uid and t_cm.id = @mission_id
					when matched then
						update set	idx = @mission_idx,
									val = @mission_val,
									state = @mission_state,
									pass_rewarded = @mission_pass_rewarded,
									dw_update_time = @utc_date
					when not matched by target then
						insert (uid, id, idx, val, state, pass_rewarded, dw_update_time)
						values (@uid, @mission_id, @mission_idx, @mission_val, @mission_state, @mission_pass_rewarded, @utc_date);

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