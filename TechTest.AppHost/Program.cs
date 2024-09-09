var builder = DistributedApplication.CreateBuilder(args);

var sqlPwd = builder.AddParameter("sql-password", true);
var sql = builder
    .AddSqlServer("sqlserver", sqlPwd)
    .WithDataVolume()
    .AddDatabase("TechTest");

var recreateDb = builder.AddParameter("recreateDatabase");
builder
    .AddProject<Projects.TechTest_MigrationService>("migrationService")
    .WithEnvironment("DELETE_AND_RECREATE_DB", recreateDb)
    .WithReference(sql);

var api = builder
    .AddProject<Projects.TechTest_Api>("api")
    .WithReference(sql)
    .WithExternalHttpEndpoints();

builder
    .AddNpmApp("react", "../TechTest.Web")
    .WithReference(api)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
