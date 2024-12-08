using devdeer.Libraries.Abstractions.Extensions;

using Logic.Interfaces.Logic;
using Logic.Models.Options;
using Logic.OpenAI;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddSingleton<App>();
            services.AddTransient<IOpenAiLogic, OpenAiLogic>();
            services.RegisterOption<OpenAiOptions>("OpenAi");
        });
var app = builder.Build();
return await app.Services.GetRequiredService<App>()
    .StartAsync(Environment.GetCommandLineArgs());