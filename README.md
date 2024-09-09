# products
Full-stack app with ASP.NET Core minimal API, EF Core and React hosted in .NET Aspire

Set up should be as easy as running docker compose with the compose file in `TechTest.AppHost/aspirate-output/docker-compose.yaml`

Alternatively, it's possible to open the solution in Visual Studio / Rider and run as a .NET Aspire app which will handle all the orchestration for you.

The database will be automatically migrated and seeded by TechTest.MigrationService. If that doesn't work, running the docker service manually may be required.

<hr />

After the app is running, the React frontend should be available on http://localhost:8000 (please cross-reference with the orchestration output in case that's not working).

You will be presented with the list of products and at the bottom of a page will be a form to add new ones. Once submitted, the grid should refresh automatically and display the new product.

Navigating to `<app-root>/charts` will show the two charts required.

<hr />

The API also provides Swagger when navigating to `<api-root>/swagger`. This will allow you to test the API endpoints directly.
