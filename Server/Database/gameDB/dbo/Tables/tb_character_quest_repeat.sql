﻿CREATE TABLE [dbo].[tb_character_quest_repeat] (
    [uid]            BIGINT   NOT NULL,
    [id]             INT      NOT NULL,
    [idx]            INT      NOT NULL,
    [val]            BIGINT   NOT NULL,
    [state]          TINYINT  NOT NULL,
    [pass_rewarded]  BIT      NOT NULL,
    [dw_update_time] DATETIME NOT NULL,
    CONSTRAINT [PK_tb_character_quest_repeat] PRIMARY KEY CLUSTERED ([uid] DESC, [id] DESC)
);

