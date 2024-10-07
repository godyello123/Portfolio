



CREATE procedure [dbo].[sp_json_update_coin_count]
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
		
		begin transaction

			merge dbo.tb_character_coin as t_ca
			using 
			(
				select Type, TableID, count
				from openjson(@ref_json) with
					(
						Type tinyint, TableID nvarchar(32), Count bigint
					)
			) as s_ca(Type, TableID , Count)
			on t_ca.uid = @uid and
				t_ca.type = s_ca.TableID
			when matched then
				update set	value += s_ca.count, 
							dw_update_time = @utc_date
			when not matched by target then
				insert (uid, type, value, dw_update_time)
				values (@uid, s_ca.TableID, s_ca.Count, @utc_date);

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
