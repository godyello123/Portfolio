


create procedure [dbo].[sp_sys_error_log_create]
as
	set nocount on;
	set xact_abort on;

	begin
		begin try
			if error_number() is null
				return;

			if xact_state() = -1
			begin 
				print 'Cannot log error since the current transaction is in an uncommittable state. ' 
		            + 'Rollback the transaction before executing P_ERROR_LOG_C in order to successfully log error information.';
		        return;
			end

			insert dbo.tb_sys_error_log 
			(
				user_name, error_no, error_serverity, error_state, error_proc,
				error_line, error_msg, ipt_time
			)
			values
			(
				convert(sysname, suser_sname()), error_number(), error_severity(), error_state(), error_procedure(),
		        error_line(), error_message(), getdate()
			);

		end try
		begin catch
			EXEC dbo.sp_sys_error_log_create;
		end catch
	end;

