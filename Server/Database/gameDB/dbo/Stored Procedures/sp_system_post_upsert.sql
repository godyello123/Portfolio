


CREATE procedure [dbo].[sp_system_post_upsert]
(
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
		declare @InsertedIds TABLE (id bigint);

		begin transaction
		merge dbo.tb_system_post as t_cp
		using ( select @post_id as pid ) as s_cp
		on t_cp.id = s_cp.pid
		when matched then
		update set	type = @type, title = @title, msg = @msg,
					begin_time = @begin_time,
					expire_time = @expire_time,
					dw_update_time = @utc_date
		when not matched by target then
			insert ([type], [title], [msg], [begin_time], [expire_time], [reward], [dw_update_time])
			values (@type, @title, @msg, @begin_time, @expire_time, @reward, @utc_date)
		OUTPUT INSERTED.id INTO @InsertedIds; 

		select id, type, title, msg, begin_time, expire_time, reward
		from dbo.tb_system_post
		where id = (select top(1)id
		from @InsertedIds)

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