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

## Test pyramid
There are 3 different test suites set up in the solution:

### Unit tests
- Base of the test pyramid
- Lots of them, and they're quick/inexpensive to run
- Test individual classes or collections of classes
- Try to avoid test doubles when testing the domain model where possible

### Integration tests
- Middle of the test pyramid
- Slower/more expensive to run as they make a connection to a real database
- Test integration points with external systems
- In this case, just test the DAO class <-> database integration

### Component tests
- Top of the test pyramid
- Slowest to run ad they require a database, and start the whole web server
- Set up data in required state using DAO objects, call REST endpoints, then assert on the results
- Written in Gherkin in a BDD-style
- Currently use the DTO classes from the REST layer of the app - maybe they shouldn't

**Note**: It's not currently possible to run the integration and component tests at the same time, as they talk to the
same database. This needs changing!
