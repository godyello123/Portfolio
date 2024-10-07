CREATE TABLE [dbo].[tb_character_coin] (
    [uid]            BIGINT        NOT NULL,
    [type]           NVARCHAR (32) NOT NULL,
    [value]          BIGINT        NOT NULL,
    [dw_update_time] DATETIME      NOT NULL,
    CONSTRAINT [PK_tb_character_coin] PRIMARY KEY CLUSTERED ([uid] DESC, [type] DESC)
);

