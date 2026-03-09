# Техническое описание проекта
Старт проекта: 2026-02-03

### Структура проекта
| №        | Модуль                           | Описание                              |
|----------|----------------------------------|---------------------------------------|
|  1       | PersonalAccount.Domain           | Общий модуль для всего приложения. Включает в себя: `доменные структуры и общие логическое элементы` |
|  2       | PersonalAccount.Console          | Отдельное приложение для запуска на клиента для сборка и передачи данных на сервер. |
|  3       | PersonalAccount.Common           | Общий модуль для хранения интерфейсов и элементов общей логики.
|  4       | PersonalAccount.Data             | Отдельный модуль для работы с данными / миграциями для серверной части. |
|  5       | PersonalAccount.Api              | Отдельный модуль Api для взаимодействия с клиентской частью приложения. |
|          |                                  |                                        |
|          | PersonalAccount.UnitTests        | Только модульные тесты с мокированием. |
|          | PersonalAccount.IntegrationTests | Только интеграционные тесты.           |


#### PersonalAccount.Data
1. Открыть каталог `PersonAccount.Data` в командной строек.
2. Обновить схему
```
dotnet ef dbcontext scaffold "User ID=admin;Password=123456;Host=localhost;Port=5433;Database=personal_account;" Npgsql.EntityFrameworkCore.PostgreSQL --output-dir Models  --force 
```

3. Далее, класс `PersonalAccountContext` переносим в верхний каталог приложения (или переносим изменения).


