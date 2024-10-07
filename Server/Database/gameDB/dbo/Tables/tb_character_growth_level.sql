CREATE TABLE [dbo].[tb_character_growth_level] (
    [uid]            BIGINT   NOT NULL,
    [id]             INT      NOT NULL,
    [value]          INT      NOT NULL,
    [dw_update_time] DATETIME NOT NULL,
    CONSTRAINT [PK_tb_character_growth_level] PRIMARY KEY CLUSTERED ([uid] ASC, [id] ASC)
);

