CREATE TABLE [dbo].[tb_character_event_shop] (
    [uid]            BIGINT        NOT NULL,
    [event_uid]      BIGINT        NOT NULL,
    [shop_id]        INT           NOT NULL,
    [limit_count]    INT           NOT NULL,
    [dw_update_time] DATETIME2 (0) NOT NULL,
    CONSTRAINT [pk_tb_character_event_shop] PRIMARY KEY CLUSTERED ([uid] ASC, [event_uid] ASC, [shop_id] ASC)
);

