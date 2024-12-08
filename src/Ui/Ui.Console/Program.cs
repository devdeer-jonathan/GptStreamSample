using devdeer.Libraries.Abstractions.Extensions;

using Logic.Interfaces.Logic;
using Logic.Models.Options;
using Logic.OpenAI;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Ui.Console;

var builder = Host.CreateDefaultBuilder(args).ConfigureAppConfiguration(
        (ctx, builder) =>
        {
            builder.AddUserSecrets(typeof(Program).Assembly);
        })
    .ConfigureServices((hostContext, services) =>
    {
        // TODO add your service dependencies here
        services.AddSingleton<App>();
        services.AddTransient<IOpenAiLogic, OpenAiLogic>();
        services.RegisterOption<OpenAiOptions>("OpenAI");
    });
var app = builder.Build();
return await app.Services.GetRequiredService<App>().StartAsync(Environment.GetCommandLineArgs());