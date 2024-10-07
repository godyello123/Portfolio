CREATE TABLE [dbo].[tb_character_coin_teset] (
    [uid]            BIGINT        NOT NULL,
    [type]           NVARCHAR (32) NOT NULL,
    [value]          BIGINT        NOT NULL,
    [dw_update_time] DATETIME      NOT NULL,
    CONSTRAINT [PK_tb_character_coin_teste] PRIMARY KEY CLUSTERED ([uid] DESC, [type] DESC)
);

