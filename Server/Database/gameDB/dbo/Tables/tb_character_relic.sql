CREATE TABLE [dbo].[tb_character_relic] (
    [uid]            BIGINT   NOT NULL,
    [group_id]       INT      NOT NULL,
    [lv]             INT      NOT NULL,
    [bonus_prob]     INT      NOT NULL,
    [dw_update_time] DATETIME NOT NULL,
    CONSTRAINT [PK_tb_character_relic] PRIMARY KEY CLUSTERED ([uid] DESC, [group_id] ASC)
);

