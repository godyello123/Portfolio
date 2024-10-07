



CREATE procedure [dbo].[sp_character_skill_preset_update]
(
	@uid int = 0,
	@idx int = 0,
	@slot_json nvarchar(256) = '',
	@is_enable bit,
	@sp_rtn int = 0 output, 
	@sp_msg varchar(128) = '' output
)
as
	SET XACT_ABORT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	BEGIN TRY
		declare @utc_date datetime2 = sysutcdatetime();

		begin transaction

			merge dbo.tb_character_skill_preset as t_csp
				using ( select @uid as uid ) as s_cep
				on t_csp.uid = s_cep.uid and t_csp.idx = @idx
				when matched then
					update set	equip_slot = @slot_json,
								is_enable = @is_enable,
								dw_update_time = @utc_date
				when not matched by target then
					insert (uid, idx, equip_slot, is_enable, dw_update_time)
					values (@uid, @idx, @slot_json, @is_enable, @utc_date);

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