



CREATE procedure [dbo].[sp_json_update_item_count]
(
	@uid bigint,
	@ref_json varchar(max),

	@sp_rtn int = 0 output, 
	@sp_msg varchar(128) = '' output
)
as
	SET XACT_ABORT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	BEGIN TRY
		declare @utc_date datetime2 = sysutcdatetime();
		declare @default_in_used bit = 'false';
		declare @default_rand_option varchar(32) = ''

		begin transaction

			merge dbo.tb_character_item as t_ci
			using 
			(
				select ItemID, TableID, Level, Count
				from openjson(@ref_json) with
					(
						ItemID bigint, TableID nvarchar(64), Level int, Count bigint
					)
			) as s_ci(ItemID, TableID, Level, Count)
			on t_ci.uid = @uid and
				t_ci.item_id = s_ci.ItemID
			when matched then
				update set	t_ci.Count += s_ci.count, 
							dw_update_time = @utc_date
			when not matched by target then
				insert 
				(
					uid, item_id, table_id, level, 
					Count, in_used, random_option, dw_update_time
				)
				values 
				(
					@uid, s_ci.ItemID, s_ci.TableID, s_ci.Level, 
					s_ci.Count, @default_in_used, @default_rand_option, @utc_date
				);

		commit transaction

		select	@sp_rtn = 0,
				@sp_msg = 'success';
		return;

	END TRY
	BEGIN CATCH
		select	@sp_rtn = ERROR_NUMBER(), @sp_msg = ERROR_MESSAGE();
		exec dbo.sp_sys_error_log_create;
		return;
	END CATCH
