CREATE TABLE [dbo].[tb_character_stage] (
    [uid]            BIGINT   NOT NULL,
    [type]           TINYINT  NOT NULL,
    [cur_tid]        INT      NOT NULL,
    [max_tid]        INT      NOT NULL,
    [total_cnt]      BIGINT   NOT NULL,
    [is_loop]        BIT      NOT NULL,
    [dw_update_time] DATETIME NOT NULL,
    CONSTRAINT [PK_tb_character_stage] PRIMARY KEY CLUSTERED ([uid] DESC, [type] DESC)
);



