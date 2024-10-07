CREATE TABLE [dbo].[tb_character] (
    [uid]            BIGINT         NOT NULL,
    [device_id]      NVARCHAR (128) NOT NULL,
    [name]           NVARCHAR (64)  NOT NULL,
    [event_stage]    INT            NOT NULL,
    [level]          INT            NOT NULL,
    [exp]            BIGINT         NOT NULL,
    [level_point]    INT            NOT NULL,
    [ad_skip]        BIT            NOT NULL,
    [profile_id]     INT            NOT NULL,
    [login_time]     BIGINT         NOT NULL,
    [logout_time]    BIGINT         NOT NULL,
    [dw_update_time] DATETIME       NOT NULL,
    CONSTRAINT [PK_tb_character_info] PRIMARY KEY CLUSTERED ([uid] ASC)
);

