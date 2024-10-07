



create procedure [dbo].[sp_character_stage_skill_update]
(
	@uid int = 0,
	@type int = 0,
	@slot_json nvarchar(256) = '',
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

			merge dbo.tb_character_stage_skill as t_css
				using ( select @uid as uid ) as s_css
				on t_css.uid = s_css.uid and t_css.type = @type
				when matched then
					update set	equip_slot = @slot_json,
								dw_update_time = @utc_date
				when not matched by target then
					insert (uid, type, equip_slot, dw_update_time)
					values (@uid, @type, @slot_json, @utc_date);

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