namespace Ui.Console
{
    using Logic.Interfaces.Logic;

    using Microsoft.Extensions.Logging;

    using Spectre.Console;

    /// <summary>
    /// Contains the application code for the console app.
    /// </summary>
    public class App
    {
        #region member vars

        private readonly ILogger<App> _logger;

        #endregion

        #region constructors and destructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="openAiLogic">The logic to interact with OpenAI.</param>
        public App(ILogger<App> logger, IOpenAiLogic openAiLogic)
        {
            _logger = logger;
            OpenAiLogic = openAiLogic;
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
                AnsiConsole.Markup("[cyan]Assistant:[/] ");
                await OpenAiLogic.GetStreamedCompletion(
                    userPrompt,
                    chunk =>
                    {
                        AnsiConsole.Markup(chunk);
                        return Task.CompletedTask;
                    });
                AnsiConsole.WriteLine();
            }
            _logger.LogInformation("Ran to completion.");
            return await Task.FromResult(0);
        }

        #endregion

        #region properties

        public IOpenAiLogic OpenAiLogic { get; }

        #endregion
    }
}