CREATE TABLE [dbo].[tb_character_gacha] (
    [uid]            BIGINT   NOT NULL,
    [id]             INT      NOT NULL,
    [lv]             INT      NOT NULL,
    [exp]            BIGINT   NOT NULL,
    [rewarded]       INT      NOT NULL,
    [dw_update_time] DATETIME NOT NULL,
    CONSTRAINT [PK_tb_character_gacha] PRIMARY KEY CLUSTERED ([uid] DESC, [id] DESC)
);

