var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.AspireAppv6_ApiService>("apiservice");

var gateway = builder.AddProject<Projects.Gateway>("gateway").WithReference(apiService);

builder
    .AddProject<Projects.AspireAppv6_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(gateway);

builder.Build().Run();
