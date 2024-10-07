CREATE TABLE [dbo].[tb_character_quest_event] (
    [uid]            BIGINT         NOT NULL,
    [id]             INT            NOT NULL,
    [quest_id]       NVARCHAR (128) NOT NULL,
    [idx]            INT            NOT NULL,
    [val]            BIGINT         NOT NULL,
    [state]          TINYINT        NOT NULL,
    [exp_time]       DATETIME2 (0)  NOT NULL,
    [dw_update_time] DATETIME2 (0)  NOT NULL,
    CONSTRAINT [PK_tb_character_quest_event] PRIMARY KEY CLUSTERED ([uid] DESC, [quest_id] ASC, [id] DESC)
);

