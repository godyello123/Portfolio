
CREATE procedure [dbo].[sp_character_quest_checkIn_rewarded]
(
	@db_uid int = 0,
	@db_quest_id varchar(max) = '',
	@db_mission_id int = 0,
	@db_mission_idx int = 0,
	@db_mission_val bigint = 0,
	@db_mission_state tinyint = 0,
	@db_exp_time datetime2(0),

	@db_update_coin nvarchar(max) = '',
	@db_update_item nvarchar(max) = '',
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

		exec dbo.sp_character_quest_checkIn_update
		@db_uid = @db_uid, @db_quest_id = @db_quest_id, @db_mission_id = @db_mission_id,
		@db_mission_idx = @db_mission_idx, @db_mission_val = @db_mission_val,
		@db_mission_state = @db_mission_state, @db_exp_time = @db_exp_time,
		@sp_rtn = @sp_rtn out, @sp_msg = @sp_msg out

		if @sp_rtn <> 0 
		begin 
			if XACT_STATE() <> 0 rollback transaction;
			return;
		end
			
		--update coin
		if LEN(@db_update_coin) > 0
		begin 
			exec dbo.sp_json_update_coin_count
				@uid = @db_uid, @ref_json = @db_update_coin,
				@sp_rtn = @sp_rtn out, @sp_msg = @sp_msg out;

			if @sp_rtn <> 0 
			begin 
				if XACT_STATE() <> 0 rollback transaction;
				return;
			end
		end

		--update item
		if LEN(@db_update_item) > 0
		begin
			exec dbo.sp_json_update_item_count
				@uid = @db_uid, @ref_json = @db_update_item,
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