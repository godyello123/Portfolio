CREATE TABLE [dbo].[tb_character_post] (
    [uid]            BIGINT        NOT NULL,
    [id]             BIGINT        NOT NULL,
    [type]           TINYINT       NOT NULL,
    [title]          NVARCHAR (64) NOT NULL,
    [msg]            NVARCHAR (64) NOT NULL,
    [is_read]        BIT           NOT NULL,
    [is_reward]      BIT           NOT NULL,
    [begin_time]     BIGINT        NOT NULL,
    [expire_time]    BIGINT        NOT NULL,
    [reward]         VARCHAR (512) NOT NULL,
    [dw_update_time] DATETIME      NOT NULL,
    CONSTRAINT [PK_tb_character_post] PRIMARY KEY CLUSTERED ([uid] DESC, [id] DESC)
);

