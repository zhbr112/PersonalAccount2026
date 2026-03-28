-- Скрипт миграции
-- 2026-03-27
create table if not exists journal
(
    -- Уникальный код транзакции.
    transnumber bigint,
    -- Уникальный код типа транзакции.
    transtype bigint,
    -- Номер чека.
    receiptn bigint,
    -- Уникальный код продукта.
    productid bigint,
    -- Наименование товара.
    product_name text,
    -- Уникальный код категории продуктов.
    categoryid bigint,
    -- Наименование категории.
    category_name text,
    -- Код сотрудника.
    emploeeid bigint,
    -- Наименование сотрудника
    emploee_name text,
    -- Дата время транзакции.
    dater timestamp with time zone,
    -- Количество.
    quantity float,
    -- Цена.
    Price float,
    -- Сумма скидки
    discountamount float
);

