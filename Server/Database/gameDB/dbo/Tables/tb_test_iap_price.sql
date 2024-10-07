CREATE TABLE [dbo].[tb_test_iap_price] (
    [product_id]     VARCHAR (32) NOT NULL,
    [price]          BIGINT       NOT NULL,
    [dw_update_time] DATETIME     NOT NULL,
    CONSTRAINT [PK_tb_test_iap_price] PRIMARY KEY CLUSTERED ([product_id] ASC)
);

