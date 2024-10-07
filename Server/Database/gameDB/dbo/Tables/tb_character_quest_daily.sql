CREATE TABLE [dbo].[tb_character_quest_daily] (
    [uid]            BIGINT        NOT NULL,
    [id]             INT           NOT NULL,
    [idx]            INT           NOT NULL,
    [val]            BIGINT        NOT NULL,
    [state]          TINYINT       NOT NULL,
    [exp_time]       DATETIME2 (0) NOT NULL,
    [dw_update_time] DATETIME2 (0) NOT NULL,
    CONSTRAINT [PK_tb_character_quest_daily] PRIMARY KEY CLUSTERED ([uid] DESC, [id] DESC)
);

