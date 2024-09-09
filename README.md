# products
Full-stack app with ASP.NET Core minimal API, EF Core and React hosted in .NET Aspire

Set up should be as easy as running docker compose with the compose file in `TechTest.AppHost/aspirate-output/docker-compose.yaml`

Alternatively, it's possible to open the solution in Visual Studio / Rider and run as a .NET Aspire app which will handle all the orchestration for you.

The database will be automatically migrated and seeded by TechTest.MigrationService. If that doesn't work, running the docker service manually may be required.
