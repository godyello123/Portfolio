CREATE TABLE [dbo].[tb_system_post] (
    [id]             BIGINT        IDENTITY (1, 1) NOT NULL,
    [type]           TINYINT       NOT NULL,
    [title]          NVARCHAR (64) NOT NULL,
    [msg]            NVARCHAR (64) NOT NULL,
    [begin_time]     BIGINT        NOT NULL,
    [expire_time]    BIGINT        NOT NULL,
    [reward]         VARCHAR (512) NOT NULL,
    [dw_update_time] DATETIME      NOT NULL,
    CONSTRAINT [PK_tb_system_post] PRIMARY KEY CLUSTERED ([id] DESC, [type] DESC)
);

