
CREATE procedure [dbo].[sp_character_create]
(
	@uid int = 0,
	@name nvarchar(128) = '',
	@profile_id int = 0,

	@sp_rtn int = 0 output, 
	@sp_msg varchar(128) = '' output
)
as
	SET XACT_ABORT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	BEGIN TRY
		declare @utc_date datetime2 = sysutcdatetime();
		declare @user_cnt int = 0
		declare @user_level int = 1;
		declare @user_exp int = 0;
		declare @level_point int = 0;
		declare @skin_id int = '1';
		declare @cur_time bigint = DATEDIFF(SECOND,{d '1970-01-01'}, @utc_date)
		declare @event_stage int = 1001;
		declare @device_id nvarchar(128) = ''; 
		declare @ad_skip bit = 'FALSE'
	

		begin transaction

			set @user_cnt = (
			select count(uid)
			from dbo.tb_character
			where uid = @uid)

			if @user_cnt = 0
			begin
				set @device_id = (select token_id from systemDB.dbo.tb_account where uid = @uid)

				insert into dbo.tb_character
				(uid, device_id, name, event_stage, level, exp, level_point, ad_skip, profile_id, login_time, logout_time, dw_update_time)
				values
				(@uid, @device_id, @name, @event_stage, @user_level, @user_exp, @level_point, @ad_skip, @profile_id,@cur_time, @cur_time, @utc_date)
			end

			select uid, device_id
				from dbo.tb_character
				where uid = @uid

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
