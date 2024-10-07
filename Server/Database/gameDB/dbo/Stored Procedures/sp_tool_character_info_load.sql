




CREATE procedure [dbo].[sp_tool_character_info_load]
(
	@uid bigint = 0,
	@deviceid nvarchar(max) = '',
	@name nvarchar(max) = '',

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
		
		if @uid = -1
		begin
			if len(@deviceid) > 0
			begin
				set @uid = (select isnull(MAX(UID), -1) UID
				from dbo.tb_character with(nolock)
				where device_id = @deviceid)

				if @uid = -1
				begin
					select	@sp_rtn = 1, 
							@sp_msg = 'NOT FOUND DATA';
				end
			end
			else if len(@name) > 0
			begin
				set @uid = (select isnull(MAX(UID), -1) UID
					from dbo.tb_character with(nolock)
					where name = @name)

					if @uid = -1
					begin
						select	@sp_rtn = 1, 
								@sp_msg = 'NOT FOUND DATA';
					end
			end
		end

		--character info
		select uid, name, device_id, level, exp, level_point, login_time, logout_time
		from dbo.tb_character with(nolock)
		where uid = @uid

		--gacha
		select id, lv, exp, rewarded
		from dbo.tb_character_gacha with(nolock)
		where uid = @uid

		--coin
		select type, value
		from dbo.tb_character_coin with(nolock)
		where uid = @uid

		--stage
		select type, cur_tid, max_tid, total_cnt
		from dbo.tb_character_stage with(nolock)
		where uid = @uid

		--receipt
		select transaction_id, store_type, tcs.product_id as product_id, ti.price as price,
		mail_guid, tcs.dw_update_time
		from dbo.tb_character_shop_iap_receipt as tcs
		join dbo.tb_test_iap_price as ti
		on ti.product_id = tcs.product_id
		where tcs.uid = @uid
		
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