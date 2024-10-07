CREATE TABLE [dbo].[tb_character_ads_buff] (
    [uid]            BIGINT        NOT NULL,
    [buff_id]        INT           NOT NULL,
    [buff_lv]        INT           NOT NULL,
    [buff_exp]       INT           NOT NULL,
    [buff_exp_time]  BIGINT        NOT NULL,
    [watch_exp_time] BIGINT        NOT NULL,
    [watch_count]    INT           NOT NULL,
    [dw_update_time] DATETIME2 (0) NOT NULL,
    CONSTRAINT [PK_tb_character_ads_buff] PRIMARY KEY CLUSTERED ([uid] ASC, [buff_id] ASC)
);

