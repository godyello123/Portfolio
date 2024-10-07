


CREATE procedure [dbo].[sp_character_nickname_update]
(
	@uid bigint,
	@user_name nvarchar(40),
	@update_coins varchar(max),

	@sp_rtn int = 0 output, 
	@sp_msg varchar(128) = '' output
)
as
	SET XACT_ABORT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	BEGIN TRY

		declare @utc_date datetime2 = sysutcdatetime();
		declare @curr_time bigint = (datediff(s, '1970-01-01', @utc_date));
		
		declare @is_exists bit = (case when exists (select * from dbo.tb_character where name = @user_name) then 1 else 0 end)

		if @is_exists = 1
		begin
			select	@sp_rtn = 1, 
					@sp_msg = 'duplicated name';
			return;
		end

		begin transaction

			update dbo.tb_character
			set name = @user_name ,dw_update_time = @utc_date
			where uid = @uid
			

			if LEN(@update_coins) > 0
			begin
				exec dbo.sp_json_update_coin_count
				@uid = @uid, @ref_json =@update_coins,
				@sp_rtn = @sp_rtn out, @sp_msg = @sp_msg out

				if @sp_rtn <> 0
					begin
						if XACT_STATE() <> 0 rollback transaction;
						return;
					end
			end
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