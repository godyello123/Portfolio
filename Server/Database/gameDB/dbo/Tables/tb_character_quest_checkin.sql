CREATE TABLE [dbo].[tb_character_quest_checkin] (
    [uid]            BIGINT        NOT NULL,
    [quest_id]       VARCHAR (32)  NOT NULL,
    [id]             INT           NOT NULL,
    [idx]            INT           NOT NULL,
    [val]            BIGINT        NOT NULL,
    [state]          TINYINT       NOT NULL,
    [exp_time]       DATETIME2 (0) NOT NULL,
    [dw_update_time] DATETIME2 (0) NOT NULL,
    CONSTRAINT [PK_tb_character_quest_checkin] PRIMARY KEY CLUSTERED ([uid] DESC, [quest_id] ASC, [id] ASC)
);

