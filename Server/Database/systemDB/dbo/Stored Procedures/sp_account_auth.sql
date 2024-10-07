


CREATE procedure [dbo].[sp_account_auth]
(
	@serverId int = 0,
	@token nvarchar(128),
	@auth_type tinyint,
	@sp_rtn int = 0 output, 
	@sp_msg varchar(128) = '' output
)
as
	SET XACT_ABORT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	BEGIN TRY
		declare @utc_date datetime2 = sysutcdatetime();
		declare @tmp_output table (uid bigint);

		begin transaction

			update dbo.tb_account
			set auth_type = @auth_type, dw_update_time = @utc_date
			output inserted.uid into @tmp_output
			where token_id = @token

			if @@ROWCOUNT = 0
			begin
				declare @new_uid bigint = (next value for dbo.seq_pc_id);

				insert into dbo.tb_account( token_id, uid, auth_type, create_time, dw_update_time )
				output inserted.uid into @tmp_output
				values( @token, @new_uid, @auth_type, @utc_date, @utc_date );
			end

			select top(1)uid
			from @tmp_output;

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
