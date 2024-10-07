CREATE TABLE [dbo].[tb_character_event_roulette] (
    [uid]            BIGINT        NOT NULL,
    [event_uid]      BIGINT        NOT NULL,
    [exclude_list]   VARCHAR (128) NOT NULL,
    [dw_update_time] DATETIME2 (0) NOT NULL,
    CONSTRAINT [pk_tb_character_event_roulette] PRIMARY KEY CLUSTERED ([uid] ASC, [event_uid] ASC)
);

