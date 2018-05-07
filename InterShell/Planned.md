# Planned Features

Note: this is a work in progress and the descriptions below are incomplete.

***

Module _basicmssql_
-------------------

Purpose: MS SQL support.

* Install and reference MS SQL EF

```
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
ef dbcontext scaffold ...  Microsoft.EntityFrameworkCore.SqlServer ...
```

* In Startup.ConfigureServices use options.UseSqlServer

```
services.AddDbContext<DataStore>(options => options.UseSqlServer(Configuration.GetConnectionString("DataStore")));
```

* Use MS SQL Shell

```
sqlcmd -S localhost -U root -P mapleace -i migrate.sql
```

See: 
* [sqlcmd Utility](https://docs.microsoft.com/en-us/sql/tools/sqlcmd-utility?view=sql-server-2017)
* [sqlcmd - Use the utility](https://docs.microsoft.com/en-us/sql/relational-databases/scripting/sqlcmd-use-the-utility?view=sql-server-2017)


***

Module _basicsqlite_
--------------------

Purpose: SQLite support.

* Download SQLite and SQLite shell (sqlite3)
  - Go to [SQLite download page](https://www.sqlite.org/download.html) and download relevant ZIP files.
  - On Windows unzip them into: `C:/Program Files/sqlite`

* Install and reference SQLite EF

```
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
ef dbcontext scaffold ...  Microsoft.EntityFrameworkCore.Sqlite ...
```

* In Startup.ConfigureServices use options.UseSqlite

```
services.AddDbContext<DataStore>(options => options.UseSqlite(Configuration.GetConnectionString("DataStore")));
```

* Use SQLite Shell

```
sqlite3 mydatabase.db < migrate.sql
```

* Database file
  - It should be located in the project folder
  - Assuming its name is 'mydatabase.db', then the connection string must be: "datasource=mydatabase.db"
  - This must be added to the project config file:
    ```XML
    <ItemGroup>
        <None Update="mydatabase.db">
            <CopyToOutputDirectory>Always</CopyToOutputDirectory>
        </None>
    </ItemGroup>
    ```
  - This works too
    ```XML
    <ItemGroup>
        <None Remove="mydatabase.db" />
    </ItemGroup>
    <ItemGroup>
        <Content Include="mydatabase.db">
            <CopyToPublishDirectory>Always</CopyToPublishDirectory>
        </Content>
    </ItemGroup>

    ```