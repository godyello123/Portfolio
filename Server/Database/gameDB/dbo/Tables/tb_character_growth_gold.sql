CREATE TABLE [dbo].[tb_character_growth_gold] (
    [uid]            BIGINT   NOT NULL,
    [id]             INT      NOT NULL,
    [value]          INT      NOT NULL,
    [dw_update_time] DATETIME NOT NULL,
    CONSTRAINT [PK_tb_character_growth_gold] PRIMARY KEY CLUSTERED ([uid] ASC, [id] ASC)
);

