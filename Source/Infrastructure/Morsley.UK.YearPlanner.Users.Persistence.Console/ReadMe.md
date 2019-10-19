# Entity Framework Core Migrations

This project exists solely so that EF migrations can be performed.

EF Core requires 2 packages to perform migrations. 

```Microsft.EntityFrameworkCore.Tools```

And

```Microsft.EntityFrameworkCore.Design```

However, the 'Microsft.EntityFrameworkCore.Design' package requires an executable project.

Therefore:

Our current 'Morsley.UK.YearPlanner.Users.Persistence' library project requires the 'Microsft.EntityFrameworkCore.Tools' package.
We also need to create a C# Core Console project, which requires the 'Microsft.EntityFrameworkCore.Design' package. It also needs to reference the 'Morsley.UK.YearPlanner.Users.Persistence' library project.

1. 'Morsley.UK.YearPlanner.Users.Persistence.Console' needs references to 'Morsley.UK.YearPlanner.Users.Persistence' & 'Morsley.UK.YearPlanner.Users.Domain'.
2. It then needs the 'Microsft.EntityFrameworkCore.Design' package.
3. Set the 'Morsley.UK.YearPlanner.Users.Persistence.Console' as the 'StartUp' project.
4. In the 'Package Manager Console', set the 'Default project' to 'Morsley.UK.YearPlanner.Users.Persistence'.

EF commands should now execute.

The connection string should reside in an environment variable called:

```
MORSLEY_UK_YEAR_PLANNER_USERS_PERSISTENCE_SQL_SERVER
```

And for an SQL Server database it should be in the format:

```
Server=localhost; Initial Catalog=YearPlanner; User ID=sa; Password=<PASSWORD>;
```

i.e.

```
Server=localhost; Initial Catalog=Morsley_UK_YearPlanner_Users; User ID=sa; Password=I<3G0r1ll@$!;
```

To create a migration:

```
Add-Migration <Name>
```
For the initial migration, this could be 'Initial'.

Once this is done, then the database must be upgraded with the migrations:

```
Update-Database
```