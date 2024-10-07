CREATE TABLE [dbo].[tb_system_notice] (
    [notice_id]      BIGINT        IDENTITY (1, 1) NOT NULL,
    [msg]            NVARCHAR (64) NOT NULL,
    [begin_time]     DATETIME2 (0) NOT NULL,
    [expire_time]    DATETIME2 (0) NOT NULL,
    [loop]           INT           NOT NULL,
    [term]           INT           NOT NULL,
    [dw_update_time] DATETIME2 (0) NOT NULL,
    CONSTRAINT [PK_tb_system_coupon] PRIMARY KEY CLUSTERED ([notice_id] ASC)
);

