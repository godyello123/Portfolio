CREATE TABLE [dbo].[tb_account] (
    [token_id]       NVARCHAR (128) NOT NULL,
    [uid]            BIGINT         NOT NULL,
    [auth_type]      TINYINT        NOT NULL,
    [create_time]    DATETIME2 (0)  NOT NULL,
    [dw_update_time] DATETIME2 (0)  NOT NULL,
    CONSTRAINT [pk_tb_account] PRIMARY KEY CLUSTERED ([token_id] ASC)
);



