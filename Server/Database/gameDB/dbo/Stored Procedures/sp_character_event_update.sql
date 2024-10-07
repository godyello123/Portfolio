


CREATE procedure [dbo].[sp_character_event_update]
(
	@uid int = 0,
	@event_uid bigint = 0,
	@event_id int = 0,
	@start_date bigint = 0,
	@end_date bigint = 0,
	@coin_type varchar(128) = 0,

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
			
			delete from dbo.tb_character_coin
			where uid = @uid and type = @coin_type

			merge dbo.tb_character_event as t_ce
				using ( select @uid as uid ) as s_cs
				on t_ce.uid = s_cs.uid and t_ce.event_uid = @event_uid and t_ce.event_id = @event_id
				when matched then
					update set	start_date = @start_date,
								end_date = @end_date,
								dw_update_time = @utc_date
				when not matched by target then
					insert (uid, event_uid, event_id, start_date, end_date, dw_update_time)
					values (@uid, @event_uid, @event_id, @start_date, @end_date, @utc_date);



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