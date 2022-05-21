# MyInvest

Investment management system for a fictional wealth management business.

A toy project for:

- Learning C# and .NET Core
- Exercising domain-driven design
- Exercising TDD

## Getting started

### Prerequisites
Ensure you have the `dotnet` CLI installed. You will also need the Entity Framework extension; this can be installed
with:
```
dotnet tool install --global dotnet-ef
```

### Running the server
1. Start the database: `docker compose up`
2. Run the migrations: `dotnet ef database update --project src/MyInvest.Persistence`
3. Start the server `dotnet run --project src/MyInvest`
4. Visit https://localhost:7147/swagger/index.html

### Running the tests
Unit tests can be ran with:
```
dotnet test tests/MyInvest.UnitTests
```

To run the integration tests, ensure the test database is running in Docker, then:
```
dotnet test tests/MyInvest.IntegrationTests
```

To run the component tests, ensure the test database is running in Docker, then:
```
dotnet test tests/MyInvest.ComponentTests
```

### Creating database migrations
To create a migration, edit the relevant entity files, then run:
```
dotnet ef migrations add MigrationName --project src/MyInvest.Persistence --output-dir Migrations
```

After checking the generated migration, you can apply it with:
```
dotnet ef database update --project src/MyInvest.Persistence
```
