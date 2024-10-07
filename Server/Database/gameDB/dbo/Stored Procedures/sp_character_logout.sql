
CREATE procedure [dbo].[sp_character_logout]
(
	@uid int = 0,
	@user_level int =0,
	@user_exp bigint = 0,
	@user_level_point int = 0,
	@user_ad_skip bit = 'FALSE',
	@user_profile_id int = 0,

	@sp_rtn int = 0 output, 
	@sp_msg varchar(128) = '' output
)
as
	SET XACT_ABORT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	BEGIN TRY
		declare @utc_date datetime2 = sysutcdatetime();
		declare @logout_time bigint = DATEDIFF(SECOND,{d '1970-01-01'}, @utc_date)

		begin transaction

		update dbo.tb_character
		set 
			level = @user_level, 
			exp = @user_exp, 
			level_point = @user_level_point, 
			ad_skip = @user_ad_skip,
			profile_id = @user_profile_id,
			logout_time = @logout_time
		where uid = @uid
		
		commit transaction;

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
