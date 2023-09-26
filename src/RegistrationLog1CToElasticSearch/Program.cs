using RegistrationLog1CToElasticSearch;
using RegistrationLog1CToElasticSearch.EF;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddSingleton<MainConfig>();

        services.AddSingleton<FileLogger>();

        services.AddTransient<ReaderContext>();

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
