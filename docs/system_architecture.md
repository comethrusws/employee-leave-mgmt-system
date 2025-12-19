```mermaid
architecture-beta
    group presentation(cloud)[Presentation Layer]
    service views(server)[Views (Razor)] in presentation
    service controllers(server)[Controllers] in presentation
    
    group business(cloud)[Business Logic Layer]
    service auth(server)[Authentication & Authorization] in business
    service logic(server)[Leave Management Logic] in business

    group data(database)[Data Access Layer]
    service efcore(server)[EF Core DbContext] in data
    service db(database)[SQLite Database] in data

    views:R -- L:controllers
    controllers:B -- T:auth
    controllers:B -- T:logic
    logic:R -- L:efcore
    auth:R -- L:efcore
    efcore:B -- T:db
```
