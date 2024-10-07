CREATE TABLE [dbo].[tb_system_schedule] (
    [uid]            BIGINT        NOT NULL,
    [type]           TINYINT       NOT NULL,
    [param]          VARCHAR (128) NOT NULL,
    [val]            VARCHAR (512) NOT NULL,
    [start_date]     DATETIME2 (0) NOT NULL,
    [end_date]       DATETIME2 (0) NOT NULL,
    [day_week]       VARCHAR (16)  NOT NULL,
    [start_time]     VARCHAR (16)  NOT NULL,
    [end_time]       VARCHAR (16)  NOT NULL,
    [dw_update_time] DATETIME2 (0) NOT NULL,
    CONSTRAINT [PK_tb_system_schedule] PRIMARY KEY CLUSTERED ([uid] DESC, [type] DESC)
);

