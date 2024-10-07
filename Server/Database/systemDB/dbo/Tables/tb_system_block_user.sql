CREATE TABLE [dbo].[tb_system_block_user] (
    [device_id]      NVARCHAR (128) NOT NULL,
    [cnt]            INT            NOT NULL,
    [exp_time]       BIGINT         NOT NULL,
    [dw_update_time] DATETIME2 (0)  NOT NULL,
    CONSTRAINT [pk_tb_sys_block_user] PRIMARY KEY CLUSTERED ([device_id] ASC)
);

