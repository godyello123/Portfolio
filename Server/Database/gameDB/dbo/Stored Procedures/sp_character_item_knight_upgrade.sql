

create procedure [dbo].[sp_character_item_knight_upgrade]
(
	@uid int = 0,
	@itemid bigint = 0,
	@tableid nvarchar(32) = '',
	@level int = 0,
	@count bigint = 0,
	@rand_option varchar(max) = '',
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
		--declare @random_option varchar(512) = ''

		begin transaction

		merge dbo.tb_character_item as t_ci
			using ( select @uid as uid ) as s_ci
			on t_ci.uid = s_ci.uid and t_ci.item_id = @itemid
			when matched then
				update set	t_ci.level = @level,
							t_ci.random_option = @rand_option,
							dw_update_time = @utc_date
			when not matched by target then
				insert 
				(
					uid, item_id, table_id, level, 
					count, in_used, random_option, dw_update_time
				)
				values 
				(
					@uid, @itemId, @tableid, @level, 
					@count, 'false', @rand_option, @utc_date
				);
		
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