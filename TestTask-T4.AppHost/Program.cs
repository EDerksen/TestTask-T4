var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.TestTask_T4>("testtask-t4");

builder.Build().Run();
