CREATE TABLE [dbo].[tb_character_item] (
    [uid]            BIGINT         NOT NULL,
    [item_id]        BIGINT         NOT NULL,
    [table_id]       NVARCHAR (64)  NOT NULL,
    [level]          INT            NOT NULL,
    [count]          BIGINT         NOT NULL,
    [in_used]        BIT            NOT NULL,
    [random_option]  VARCHAR (1024) NOT NULL,
    [dw_update_time] DATETIME       NOT NULL,
    CONSTRAINT [PK_tb_character_item] PRIMARY KEY CLUSTERED ([uid] DESC, [item_id] DESC)
);





