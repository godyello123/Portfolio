



CREATE procedure [dbo].[sp_character_stage_update]
(
	@uid int = 0,
	@stage_type tinyint = 0,
	@cur_tid int = 0,
	@max_tid int = 0,
	@total_cnt bigint = 0,
	@is_loop bit = false,
	@update_coin nvarchar(256) = '',
	@user_level int =0,
	@user_exp bigint = 0,
	@user_level_point int = 0,
	@user_event_stage int = 0,
	@update_item nvarchar(max),
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

			merge dbo.tb_character_stage as t_cs
				using ( select @uid as uid ) as s_cs
				on t_cs.uid = s_cs.uid and t_cs.type = @stage_type
				when matched then
					update set	cur_tid = @cur_tid,
								max_tid = @max_tid,
								total_cnt = @total_cnt,
								is_loop = @is_loop,
								dw_update_time = @utc_date
				when not matched by target then
					insert (uid, type, cur_tid, max_tid, total_cnt, is_loop, dw_update_time)
					values (@uid, @stage_type, @cur_tid, @max_tid, @total_cnt, @is_loop, @utc_date);

					update dbo.tb_character
					set level = @user_level, exp = @user_exp, level_point = @user_level_point, event_stage = @user_event_stage
					where uid = @uid

					--asset update;
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