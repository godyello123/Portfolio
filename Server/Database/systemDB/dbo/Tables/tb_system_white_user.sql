CREATE TABLE [dbo].[tb_system_white_user] (
    [device_id]      NVARCHAR (128) NOT NULL,
    [create_time]    DATETIME2 (0)  NOT NULL,
    [dw_update_time] DATETIME2 (0)  NOT NULL,
    CONSTRAINT [pk_tb_sys_white_user] PRIMARY KEY CLUSTERED ([device_id] ASC)
);

