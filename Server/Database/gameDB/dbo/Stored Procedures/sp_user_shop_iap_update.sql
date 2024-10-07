
CREATE procedure [dbo].[sp_user_shop_iap_update]
(
	@uid bigint,
	@store_type tinyint,
	@transaction_id varchar(64),
	@product_id varchar(32),

	@record_id bigint,
	@bought_cnt int,
	@reset_time bigint,

	@mail_type tinyint = 0,
	@mail_uid bigint = -1,
	@mail_title nvarchar(128) = N'',
	@mail_msg nvarchar(2048) = N'',
	@mail_exp_time bigint = -1,
	@mail_rewards nvarchar(2048) = '',

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

		--임시 주석
		--declare @b_exists bit = case when exists
		--(
		--	select 1 from dbo.tb_character_shop_iap_receipt where transaction_id = @transaction_id
		--) then 1 else 0 end;

		--if @b_exists = 1
		--begin
		--	select	@sp_rtn = -1, @sp_msg = 'already gave';
		--	return;
		--end;

		begin transaction

			--receipt 
			--임시 주석 테스트용
			--insert into dbo.tb_character_shop_iap_receipt(uid, store_type, transaction_id, product_id, mail_guid, dw_update_time)
			--values(@uid, @store_type, @transaction_id, @product_id, @mail_uid, @utc_date);

			--shop
			merge dbo.tb_character_shop as t_cs
			using ( select @uid as uid ) as s_crr
			on t_cs.uid = s_crr.uid and t_cs.shop_id = @record_id
			when matched then
				update set	[limit_count] = @bought_cnt,
							reset_time = @reset_time,
							dw_update_time = @utc_date
			when not matched by target then
				insert (uid, shop_id, limit_count, reset_time, dw_period_reward_time, dw_update_time)
				values (@uid, @record_id, @bought_cnt, @reset_time, @curr_time, @utc_date);

			--post
			insert into dbo.tb_character_post
			(	uid, id, type, title, msg, is_read, is_reward, 
			begin_time, expire_time, reward, dw_update_time)
			values
			(	@uid, @mail_uid, @mail_type, @mail_title, @mail_msg, 'FALSE', 'FALSE',
			@curr_time, @mail_exp_time, @mail_rewards, @utc_date)


			if @sp_rtn <> 0
			begin
				if XACT_STATE() <> 0 rollback transaction;
				return;
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