# Persistence SQL

This project should will reference the Domain project.

Responsibilities:

- UnitOfWork
- Repositories

Expecting the following environment varible:

- MORSLEY_UK_YEAR_PLANNER_USERS_PERSISTENCE_SQL_SERVER

This should contain the database connection string. i.e.

```
Server=localhost; Initial Catalog=Morsley_UK_YearPlanner_Users; User ID=sa; Password=Y34rPl4nn3r!;
```

## Repository

> Mediates between the domain and the data mapping layers, acting like an in-memory collection of doamin objects.

*PoEAA by Martin Fowler*

## Unit of Work

> Maintains a list of objects affected by a busiess transaction and coordinates the writung of changes.

*PoEAA by Martin Fowler*

---

## SQL Server in Docker ##

https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-2017&pivots=cs1-bash

```
docker pull mcr.microsoft.com/mssql/server:2017-latest
```



```
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Y34rPl4nn3r!' -p 1433:1433 --name sql_server -d mcr.microsoft.com/mssql/server:2017-latest
```

```
docker ps -a
```

```
docker kill []
```

```
docker start []
```


### Packages:

- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.SqlServer

The above packages are required by the migration process.

## Code First vs. Database First

---

### Resources:

- https://www.youtube.com/watch?v=OoVQdG1sqHk
- 