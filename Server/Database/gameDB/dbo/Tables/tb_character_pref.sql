CREATE TABLE [dbo].[tb_character_pref] (
    [uid]            BIGINT          NOT NULL,
    [type]           TINYINT         NOT NULL,
    [strVal]         NVARCHAR (1024) NOT NULL,
    [dw_update_time] DATETIME        NOT NULL,
    CONSTRAINT [PK_tb_character_pref] PRIMARY KEY CLUSTERED ([uid] DESC, [type] DESC)
);

