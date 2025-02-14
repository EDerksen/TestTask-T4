[TestTask-T4](TestTask-T4) - WebApi сервис  
[TestTask-T4.Contracts](TestTask-T4.Contracts) - интерфейс ITransaction и модели request/response

Комментарии по реализации:
- Для хранения данных использовал Postgres (для простоты тестирования БД очищается при каждом запуске AspireAppHost, миграции применяются автоматически при запуске сервиса) 
- Блокировки реализованы с помощью транзакций и ``` SELECT ... FOR UPDATE ```
- Откат транзакций реализован через создание новой транзакции с обратным эффектом 
- При создании транзакциям добавляется BalanceSnapshot, чтобы хранить состояние баланса на случай повторного запроса
- Валидация и обработка ошибок реализована с помощью атрибутов на контрактах и через исключения [TestTask-T4\Exceptions](TestTask-T4\Exceptions) и их обработку в middleware [TestTask-T4\Middleware\ExceptionHandlingMiddleware.cs](TestTask-T4\Middleware\ExceptionHandlingMiddleware.cs)

Для удобства добавил в проект Aspire
Для запуска потребуется Docker Desktop (из-за постгреса)
Запускать можно из студии через проект TestTask-T4.AppHost либо из консоли:
```
cd TestTask-T4.AppHost
dotnet run
```
в выводе будет url по которому можно открыть дашборд:
```
info: Aspire.Hosting.DistributedApplication[0]
      Login to the dashboard at https://localhost:17130/login?t=128d31a49779497d599d6ec7d865a91f
info: Aspire.Hosting.DistributedApplication[0]
      Distributed application started. Press Ctrl+C to shut down.
```



