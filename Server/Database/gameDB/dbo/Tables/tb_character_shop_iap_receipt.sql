CREATE TABLE [dbo].[tb_character_shop_iap_receipt] (
    [uid]            BIGINT        NOT NULL,
    [transaction_id] VARCHAR (64)  NOT NULL,
    [store_type]     TINYINT       NOT NULL,
    [product_id]     VARCHAR (32)  NOT NULL,
    [mail_guid]      BIGINT        NOT NULL,
    [dw_update_time] DATETIME2 (0) NOT NULL,
    CONSTRAINT [pk_tb_character_shop_iap_receipt] PRIMARY KEY CLUSTERED ([uid] ASC, [transaction_id] ASC)
);

