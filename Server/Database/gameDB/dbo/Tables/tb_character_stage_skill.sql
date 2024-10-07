CREATE TABLE [dbo].[tb_character_stage_skill] (
    [uid]            BIGINT         NOT NULL,
    [type]           INT            NOT NULL,
    [equip_slot]     NVARCHAR (256) NOT NULL,
    [dw_update_time] DATETIME       NOT NULL,
    CONSTRAINT [PK_tb_character_stage_skill] PRIMARY KEY CLUSTERED ([uid] ASC, [type] ASC)
);

