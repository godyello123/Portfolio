CREATE TABLE [dbo].[tb_character_event] (
    [uid]            BIGINT        NOT NULL,
    [event_uid]      BIGINT        NOT NULL,
    [event_id]       INT           NOT NULL,
    [start_date]     BIGINT        NOT NULL,
    [end_date]       BIGINT        NOT NULL,
    [dw_update_time] DATETIME2 (0) NOT NULL,
    CONSTRAINT [pk_tb_character_event] PRIMARY KEY CLUSTERED ([uid] ASC, [event_uid] ASC, [event_id] ASC)
);

