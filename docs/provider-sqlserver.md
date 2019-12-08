
## Howto develop migrations

Official Docs: https://docs.microsoft.com/de-de/ef/core/managing-schemas/migrations/index?tabs=dotnet-core-cli

- create a dummy .NET Core project (e.g. ./tools/dbsetup)
  - Add `dotnet add package Microsoft.EntityFrameworkCore.Design`
- `dotnet ef migrations add InitialCreate -c Violet.WorkItems.Provider.SqlServer.WorkItemDbContext --project ..\..\src\Violet.WorkItems.Provider.SqlServer\Violet.WorkItems.Provider.SqlServer.csproj`
  (adds a migration to the SqlServer provider project)
- `dotnet ef database update -c Violet.WorkItems.Provider.SqlServer.WorkItemDbContext --project 
..\..\src\Violet.WorkItems.Provider.SqlServer\Violet.WorkItems.Provider.SqlServer.csproj`
  Perform the migration against a database. Connection String is hardcoded (currently).