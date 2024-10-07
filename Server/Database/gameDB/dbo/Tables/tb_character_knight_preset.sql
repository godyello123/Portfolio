CREATE TABLE [dbo].[tb_character_knight_preset] (
    [uid]            BIGINT         NOT NULL,
    [idx]            INT            NOT NULL,
    [equip_slot]     NVARCHAR (256) NOT NULL,
    [is_enable]      BIT            NOT NULL,
    [dw_update_time] DATETIME       NOT NULL,
    CONSTRAINT [PK_tb_character_knight_preset] PRIMARY KEY CLUSTERED ([uid] ASC, [idx] ASC)
);

