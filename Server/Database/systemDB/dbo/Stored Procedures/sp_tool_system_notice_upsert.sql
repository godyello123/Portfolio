


create procedure [dbo].[sp_tool_system_notice_upsert]
(
	@notice_id bigint = 0,
	@notice_msg nvarchar(max) = '',
	@notice_start datetime2(0) = '',
	@notice_end datetime2(0) = '',
	@notice_loop int = 0,
	@notice_term int = 0,

	@sp_rtn int = 0 output, 
	@sp_msg varchar(128) = '' output
)
as
	SET XACT_ABORT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	BEGIN TRY
		declare @utc_date datetime2 = sysutcdatetime();
		declare @utc_time bigint = DATEDIFF(SECOND,{d '1970-01-01'}, @utc_date)
		DECLARE @InsertedId INT;

		begin transaction

		update dbo.tb_system_notice
		set msg = @notice_msg, begin_time = @notice_start, expire_time = @notice_end,
		loop = @notice_loop, term = @notice_term, dw_update_time = @utc_date
		where notice_id = @notice_id

		if @@ROWCOUNT = 0
		begin
			INSERT INTO [dbo].[tb_system_notice]
			   ([msg]
			   ,[begin_time]
			   ,[expire_time]
			   ,[loop]
			   ,[term]
			   ,[dw_update_time])
			VALUES
			   (@notice_msg,
				@notice_start,
				@notice_end,
				@notice_loop,
				@notice_term,
				@utc_date)

			SET @notice_id = SCOPE_IDENTITY();
		end
		
		select notice_id, msg, begin_time, expire_time, loop, term
		from dbo.tb_system_notice
		where notice_id = @notice_id

		select	@sp_rtn = 0, 
				@sp_msg = 'success';

		commit transaction
		return;
	END TRY
	BEGIN CATCH
		if XACT_STATE() <> 0 rollback transaction;
		select	@sp_rtn = ERROR_NUMBER(), @sp_msg = ERROR_MESSAGE();
		exec dbo.sp_sys_error_log_create;
		return;
	END CATCH