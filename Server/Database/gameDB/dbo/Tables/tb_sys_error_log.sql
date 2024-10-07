CREATE TABLE [dbo].[tb_sys_error_log] (
    [seq_no]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [user_name]       [sysname]       NOT NULL,
    [error_no]        INT             NOT NULL,
    [error_serverity] INT             NULL,
    [error_state]     INT             NULL,
    [error_proc]      NVARCHAR (126)  NULL,
    [error_line]      INT             NULL,
    [error_msg]       NVARCHAR (4000) NOT NULL,
    [ipt_time]        DATETIME        NULL,
    [dw_update_time]  DATETIME        CONSTRAINT [df_tb_sys_error_log_dw_update_time] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_tb_sys_error_log] PRIMARY KEY CLUSTERED ([seq_no] DESC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRY..CATCH블록을실행시킨오류정보', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_sys_error_log';


GO
EXECUTE sp_addextendedproperty @name = N'Caption', @value = N'데이터베이스사용자이름 ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_sys_error_log', @level2type = N'COLUMN', @level2name = N'user_name';


GO
EXECUTE sp_addextendedproperty @name = N'Caption', @value = N'TRY_CATCH오류의번호', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_sys_error_log', @level2type = N'COLUMN', @level2name = N'error_no';


GO
EXECUTE sp_addextendedproperty @name = N'Caption', @value = N'TRY_CATCH오류의심각도', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_sys_error_log', @level2type = N'COLUMN', @level2name = N'error_serverity';


GO
EXECUTE sp_addextendedproperty @name = N'Caption', @value = N'TRY_CATCH오류의상태번호', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_sys_error_log', @level2type = N'COLUMN', @level2name = N'error_state';


GO
EXECUTE sp_addextendedproperty @name = N'Caption', @value = N'TRY_CATCH오류의저장프로시저이름', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_sys_error_log', @level2type = N'COLUMN', @level2name = N'error_proc';


GO
EXECUTE sp_addextendedproperty @name = N'Caption', @value = N'TRY_CATCH오류의줄번호', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_sys_error_log', @level2type = N'COLUMN', @level2name = N'error_line';


GO
EXECUTE sp_addextendedproperty @name = N'Caption', @value = N'TRY_CATCH오류의메시지텍스트', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_sys_error_log', @level2type = N'COLUMN', @level2name = N'error_msg';


GO
EXECUTE sp_addextendedproperty @name = N'Caption', @value = N'오류발생시간', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_sys_error_log', @level2type = N'COLUMN', @level2name = N'ipt_time';

