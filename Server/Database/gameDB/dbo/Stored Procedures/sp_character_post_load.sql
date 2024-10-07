


CREATE procedure [dbo].[sp_character_post_load]
(
	@uid int = 0,
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

			--delete from dbo.tb_character_post
		--where uid = @uid and expire_time <= @login_time

		insert into dbo.tb_character_post
		( 
			uid, id, type, title, 
			msg, is_read, is_reward, begin_time, expire_time, 
			reward, dw_update_time
		)		
		select
		    @uid, sp.id, sp.type, sp.title, 
			sp.msg, 'FALSE', 'FALSE', sp.begin_time, sp.expire_time, 
		    sp.reward, sp.dw_update_time
		from dbo.tb_system_post as sp
		left outer join dbo.tb_character_post as cp
		on cp.id = sp.id and cp.type = sp.type and cp.uid = @uid
		where cp.id is null
		and sp.begin_time <= @utc_time
		and @utc_time < sp.expire_time;

		select id, type, title, msg, is_read, is_reward, begin_time, expire_time, reward
		from dbo.tb_character_post
		where uid = @uid

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