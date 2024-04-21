Entity Framework Core with Sqlite
=================================
Simon Elms, 1 Jan 2023

Creating a Migration
--------------------
In Visual Studio Package Manager Console:
	`Add-Migration MIGRATION_NAME -Project SudokuDataAccess -StartupProject SudokuWebApp`

NOTE: MIGRATION_NAME represents the name of the migration.  Each migration needs a unique name so set it to an appropriate value before running the command.

Applying Unapplied Migrations to the Database
---------------------------------------------
In Visual Studio Package Manager Console:
	`Update-Database -Project SudokuDataAccess -StartupProject SudokuWebApp`

NOTES:
1. This command will apply all the migrations that have not yet been applied to the database, in order.  If the database doesn't exist this will create it before applying the migrations to it.

2. The database the migrations are applied to is specified via the connection string in `SudokuWebApp Program.cs` `options.UseSqlite(connectionString)`:
	`builder.Services.AddDbContextFactory<DataContext>(options => 
		options.UseSqlite("Data Source=Data/Games.db"));` 

3. In this case the connection string is `"Data Source=Data/Games.db"`.  Since the database is intended to be stored locally within the web application and Sqlite does not need user name and password in the connection string, the connection string can be hardcoded.

Reverting the Database to an Earlier Migration
----------------------------------------------
In Visual Studio Package Manager Console:
	`Update-Database MIGRATION_NAME -Project SudokuDataAccess -StartupProject SudokuWebApp`

NOTE: MIGRATION_NAME is the name of the migration to roll back to, NOT the name of the migration to revert.  After the command has run this will be the name of the most recent remaining migration applied to the database.  Set the name as applicable before running the command.

Removing the Most Recent Migration from the Visual Studio Data Access Project
-----------------------------------------------------------------------------
In Visual Studio Package Manager Console:
	`Remove-Migration -Project SudokuDataAccess -StartupProject SudokuWebApp`

NOTE: There is no need to include a name if you're just rolling back the most recent migration.

Database Creation and Migration Notes
-------------------------------------
1. Specifying `-Project` and `-StartupProject` command line arguments for `Add-Migration` and `Update-Database` avoids the need to set the Default project in the Package Manager Console window, which is easy to forget.

2. `Add-Migration` requires that the startup project (in this case SudokuWebApp) references NuGet package `Microsoft.EntityFrameworkCore.Design`.

3. DON'T USE THE `context.Database.EnsureCreated()` METHOD IN THE APPLICATION CODE TO CREATE THE DATABASE IF IT DOESN'T EXIST.  The following answer to Stackoverflow question "How and where to call Database.EnsureCreated and Database.Migrate?", https://stackoverflow.com/a/38241900/216440, states that the `EnsureCreated()` method does not use migrations to create the database and is not compatible with migrations - if used then additional migrations cannot be applied to the database.  Therefore, `context.Database.EnsureCreated()` is not appropriate if you want to use migrations.

4. DON'T USE THE `context.Database.Migrate()` METHOD IN THE APPLICATION CODE TO APPLY UNAPPLIED MIGRATIONS TO THE DATABASE.  The MS Learn article "Applying Migrations", https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=vs#apply-migrations-at-runtime, states that using the `Migrate()` method within an app to ensure all migrations are deployed to the database is only appropriate for dev and test databases.  It is inappropriate for production databases because it may cause problems if another instance of an app is accessing the database while one instance is applying the migrations.  Also, the application would require elevated permissions to execute DDL queries, which is a security risk.

Sqlite Notes
------------
1. Cannot use BigInteger data type in a model class.  Use long instead.

2. The seed data, in the model configuration classes such as `IconConfiguration`, must explicitly include the ID column values, even though the ID columns will be created as auto-incrementing columns in Sqlite.  When inserting a new row into a table with an auto-incrementing column, Sqlite will find the largest value in the column and increment it when creating the new ID.  So there won't be any collisions between the manually inserted ID values and later auto-inserted ones.