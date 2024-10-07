CREATE TABLE [dbo].[tb_system_event] (
    [uid]            BIGINT        NOT NULL,
    [event_id]       INT           NOT NULL,
    [start_date]     BIGINT        NOT NULL,
    [end_date]       BIGINT        NOT NULL,
    [dw_update_time] DATETIME2 (0) NOT NULL,
    CONSTRAINT [PK_tb_system_event] PRIMARY KEY CLUSTERED ([uid] DESC, [event_id] DESC)
);

