



CREATE procedure [dbo].[sp_character_coupon_vaild]
(
	@uid bigint,
	@coupon_id nvarchar(20),
	
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
	
	declare @is_exists bit = 
	(
		case when exists 
		(
			select 1
			from dbo.tb_system_coupon 
			where coupon_id = @coupon_id
			and cnt > 0
			and @utc_time between begin_time and expire_time
		) then 1 else 0 end
	)

	if @is_exists = 0
	begin
		select @sp_rtn = 1, @sp_msg ='fail_coupon'
		return;
	end
	
	select coupon_id, reward
	from dbo.tb_system_coupon
	where coupon_id = @coupon_id

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