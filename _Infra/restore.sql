-- Создать новую базу данных
CREATE DATABASE PersonalAccount
GO

-- Параметры подключения
localhost
master
sa / 123456


-- Переключиться на системную базу данных
USE master;
GO

-- Посмотреть наименование логическлое
RESTORE FILELISTONLY FROM DISK = '/backup/PersonalAccount.bak';
GO


-- Посмотреть физические файлы новой базы данных
SELECT 
    DB_NAME(database_id) AS DatabaseName,
    type_desc AS FileType,
    physical_name AS PhysicalPath
FROM sys.master_files
WHERE DB_NAME(database_id) = 'PersonalAccount';
GO

-- Восстановить базу данных
RESTORE DATABASE PersonalAccount
FROM DISK = '/backup/PersonalAccount.bak'
WITH REPLACE,
MOVE 'pos' TO '/var/opt/mssql/data/PersonalAccount.mdf', 
MOVE 'pos_log' TO '/var/opt/mssql/data/PersonalAccount_log.ldf'; 
GO

-- Переключиться на базу данных
USE PersonalAccount
GO

-- ПРОВЕРКА


-- Получить 10 записей из журнала
select Top 10 * from journal as t1
inner join transtype as t2 on t1.transtype = t2.transtypeid

receiptn
id - уникальный код
101 - код номенклатурв
Начало / окончание -  код сотрудника


select
    top 10
    
    -- Продажа
    case when transtype = 110 
        then id 
    else
        null
    end as ProductId,
    -- Начало работы
    case when transtype =  386
        then id
    else
        null
    end as EmploeeId
from journal



-- receiptn - номер чека
-- dater - дата транзакции
-- transtype - уникальный код транзакции

-- Получить типы транзакций
select * from [dbo].[transtype]
