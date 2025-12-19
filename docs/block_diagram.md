```mermaid
block-beta
  columns 3
  User(("User")) space WebApp["Web Application\n(ASP.NET Core MVC)"]
  space space space
  space Database[("SQLite Database")] space

  User --> WebApp
  WebApp --> Database
  Database --> WebApp
  WebApp --> User
```
