CREATE TABLE [dbo].[tb_character_skill] (
    [uid]            BIGINT   NOT NULL,
    [group_id]       INT      NOT NULL,
    [tid]            INT      NOT NULL,
    [level]          INT      NOT NULL,
    [is_learend]     BIT      NOT NULL,
    [dw_update_time] DATETIME NOT NULL,
    CONSTRAINT [PK_tb_character_skill] PRIMARY KEY CLUSTERED ([uid] DESC, [group_id] DESC)
);

