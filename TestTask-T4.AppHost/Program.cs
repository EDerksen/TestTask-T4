var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithImageTag("latest")
    .WithLifetime(ContainerLifetime.Session)
    .WithPgWeb();

var testDb = postgres.AddDatabase("FinanceDb");

builder.AddProject<Projects.TestTask_T4>("testtask-t4")
    .WithReference(testDb).WaitFor(testDb);

builder.Build().Run();
