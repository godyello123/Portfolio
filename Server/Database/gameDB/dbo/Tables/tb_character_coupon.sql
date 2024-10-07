CREATE TABLE [dbo].[tb_character_coupon] (
    [uid]            BIGINT         NOT NULL,
    [coupon_json]    NVARCHAR (MAX) NOT NULL,
    [dw_update_time] DATETIME2 (0)  NOT NULL,
    CONSTRAINT [pk_tb_character_coupon] PRIMARY KEY CLUSTERED ([uid] ASC)
);

