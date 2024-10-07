CREATE TABLE [dbo].[tb_system_coupon] (
    [coupon_id]      NVARCHAR (64) NOT NULL,
    [cnt]            INT           NOT NULL,
    [use_level]      INT           NOT NULL,
    [begin_time]     BIGINT        NOT NULL,
    [expire_time]    BIGINT        NOT NULL,
    [reward]         VARCHAR (512) NOT NULL,
    [dw_update_time] DATETIME      NOT NULL,
    CONSTRAINT [PK_tb_system_coupon] PRIMARY KEY CLUSTERED ([coupon_id] ASC)
);

