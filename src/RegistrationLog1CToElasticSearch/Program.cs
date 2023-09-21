using RegistrationLog1CToElasticSearch;
using RegistrationLog1CToElasticSearch.EF;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddSingleton<MainConfig>();

        services.AddSingleton<ReaderContext>();

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
