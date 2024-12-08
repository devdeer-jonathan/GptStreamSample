namespace Ui.Console
{
    using System.Text;
    using System.Text.Json;

    using Logic.Models;

    using Microsoft.Extensions.Logging;

    using Spectre.Console;

    /// <summary>
    /// Contains the application code for the console app.
    /// </summary>
    public class App
    {
        #region member vars

        private readonly HttpClient _httpClient;

        private readonly ILogger<App> _logger;

        #endregion

        #region constructors and destructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="httpClient">Client to access the core API.</param>
        public App(ILogger<App> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        #endregion

        #region methods

        /// <summary>
        /// Represents the program logic of the console app.
        /// </summary>
        /// <param name="args">The command line arguments passed to the app at startup.</param>
        /// <returns>0 if the app ran successfully otherwise 1.</returns>
        public async Task<int> StartAsync(string[] args)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new FigletText("DEVDEER CHAT").Centered()
                    .Color(Color.Cyan1));
            AnsiConsole.Write(
                new Rule("Session start. Type <exit> to stop.").RuleStyle("grey")
                    .Centered());
            while (true)
            {
                var userPrompt = AnsiConsole.Prompt(new TextPrompt<string>("[green]User:[/]").AllowEmpty());
                if (userPrompt.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    AnsiConsole.Markup("[yellow]Goodbye![/]");
                    AnsiConsole.WriteLine();
                    AnsiConsole.Write(
                        new Rule("Session end").RuleStyle("grey")
                            .Centered());
                    break;
                }
                await StreamResponseFromApi(userPrompt);
                AnsiConsole.WriteLine();
            }
            _logger.LogInformation("Ran to completion.");
            return await Task.FromResult(0);
        }

        private async Task StreamResponseFromApi(string prompt)
        {
            var requestContent = new ChatRequestModel
            {
                Prompt = prompt
            };
            var json = JsonSerializer.Serialize(requestContent);
            //TODO: Get this endpoint from appsettings or maybe even get it from the launch settings of the core api?
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7110/StreamCompletion")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            try
            {
                using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                if (!response.IsSuccessStatusCode)
                {
                    AnsiConsole.Markup($"[red]Error: {response.StatusCode}[/]");
                    return;
                }
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                var buffer = new byte[1024];
                int bytesRead;
                AnsiConsole.Markup("[cyan]Assistant:[/] ");
                while ((bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    var chunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    AnsiConsole.Write(chunk);
                }
                AnsiConsole.WriteLine();
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]Error: {ex.Message}[/]");
            }
        }

        #endregion
    }
}