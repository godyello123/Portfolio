


CREATE procedure [dbo].[sp_tool_system_coupon_upsert]
(
	@coupon_id nvarchar(64) = '',
	@cnt int = 0,
	@use_level int = 0,
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
			
		merge dbo.tb_system_coupon as t_cs
		using ( select @coupon_id as cid ) as s_cc
		on t_cs.coupon_id = s_cc.cid
		when matched then
		update set	cnt = @cnt, use_level = @use_level,
					begin_time = @begin_time,
					expire_time = @expire_time,
					dw_update_time = @utc_date
		when not matched by target then
			insert (coupon_id, cnt, use_level, [begin_time], [expire_time], [reward], [dw_update_time])
			values (@coupon_id, @cnt, @use_level, @begin_time, @expire_time, @reward, @utc_date);

		select coupon_Id, cnt, use_level, begin_time, expire_time, reward
		from dbo.tb_system_coupon with(nolock)
		where coupon_id = @coupon_id



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