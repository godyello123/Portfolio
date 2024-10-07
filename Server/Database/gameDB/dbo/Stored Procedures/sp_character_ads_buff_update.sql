
CREATE procedure [dbo].[sp_character_ads_buff_update]
(
	@db_uid int = 0,
	@db_buff_id int = 0,
	@db_buff_lv int = 0,
	@db_buff_exp int = 0,
	@db_buff_exp_time bigint = 0,
	@db_watch_exp_time bigint = 0,
	@db_watch_count int = 0,
	
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

		merge dbo.tb_character_ads_buff as t_cab
		using ( select @db_uid as uid ) as s_cab
		on t_cab.uid = s_cab.uid and t_cab.buff_id = @db_buff_id
		when matched then
			update set	buff_lv = @db_buff_lv,
						buff_exp = @db_buff_exp,
						buff_exp_time = @db_buff_exp_time,
						watch_exp_time = @db_watch_exp_time,
						watch_count = @db_watch_count,
						dw_update_time = @utc_date
		when not matched by target then
			insert ([uid] ,[buff_id] ,[buff_lv] ,[buff_exp] ,[buff_exp_time] ,[watch_exp_time] ,[watch_count] ,[dw_update_time])
			values (@db_uid, @db_buff_id, @db_buff_lv, @db_buff_exp, @db_buff_exp_time, @db_watch_exp_time, @db_watch_count, @utc_date);

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