CREATE TABLE [dbo].[tb_character_shop] (
    [uid]                   BIGINT        NOT NULL,
    [shop_id]               INT           NOT NULL,
    [limit_count]           INT           NOT NULL,
    [reset_time]            BIGINT        NOT NULL,
    [dw_period_reward_time] BIGINT        NOT NULL,
    [dw_update_time]        DATETIME2 (0) NOT NULL,
    CONSTRAINT [pk_tb_character_shop] PRIMARY KEY CLUSTERED ([uid] ASC, [shop_id] ASC)
);

