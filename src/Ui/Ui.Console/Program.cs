using devdeer.Libraries.Abstractions.Extensions;

using Logic.Models.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Ui.Console;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(
        (_, builder) =>
        {
            builder.AddUserSecrets(typeof(Program).Assembly);
        })
    .ConfigureServices(
        (_, services) =>
        {
            services.AddHttpClient<App>();
            services.RegisterOption<OpenAIOptions>(OpenAIOptions.ConigurationKey);
        })
    .ConfigureLogging(
        logging =>
        {
            logging.ClearProviders();
            logging.AddDebug();
        });
var app = builder.Build();
return await app.Services.GetRequiredService<App>()
    .StartAsync(Environment.GetCommandLineArgs());