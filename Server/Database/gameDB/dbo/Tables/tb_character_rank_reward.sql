CREATE TABLE [dbo].[tb_character_rank_reward] (
    [uid]            BIGINT   NOT NULL,
    [type]           TINYINT  NOT NULL,
    [state]          TINYINT  NOT NULL,
    [val]            INT      NOT NULL,
    [exp_time]       BIGINT   NOT NULL,
    [dw_update_time] DATETIME NOT NULL,
    CONSTRAINT [PK_tb_character_rank_reward] PRIMARY KEY CLUSTERED ([uid] DESC, [type] DESC)
);

