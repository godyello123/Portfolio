


CREATE procedure [dbo].[sp_tool_character_post_insert]
(
	@user_uid bigint = 0,
	@post_id bigint = 0,
	@type tinyint = 0,
	@title nvarchar(max) = '',
	@msg nvarchar(max) = '',
	@begin_time bigint = 0,
	@expire_time bigint = 0,
	@reward varchar(max) = '',

	@sp_rtn int = 0 output, 
	@sp_msg varchar(128) = '' output
)
as
	SET XACT_ABORT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	BEGIN TRY
		declare @utc_date datetime2 = sysutcdatetime();
		declare @login_time bigint = DATEDIFF(SECOND,{d '1970-01-01'}, @utc_date)
		
		begin transaction
			INSERT INTO [dbo].[tb_character_post]
			   ([uid]
			   ,[id]
			   ,[type]
			   ,[title]
			   ,[msg]
			   ,[is_read]
			   ,[is_reward]
			   ,[begin_time]
			   ,[expire_time]
			   ,[reward]
			   ,[dw_update_time])
			VALUES
			   (@user_uid
			   ,@post_id
			   ,@type
			   ,@title
			   ,@msg
			   ,'FALSE'
			   ,'FALSE'
			   ,@begin_time
			   ,@expire_time
			   ,@reward
			   ,@utc_date)

		select id, type, title, msg, is_read, is_reward, begin_time, expire_time, reward
		from dbo.tb_character_post
		where uid = @user_uid and id = @post_id

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