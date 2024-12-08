namespace Logic.OpenAI
{
    using global::OpenAI;
    using global::OpenAI.Chat;

    using Interfaces.Logic;

    using Microsoft.Extensions.Options;

    using Models.Options;

    /// <summary>
    /// Logic to interact with OpenAI.
    /// </summary>
    public class OpenAiLogic : IOpenAiLogic
    {
        #region constructors and destructors

        /// <summary>
        /// Default ctor.
        /// </summary>
        /// <param name="openAiOptions">The OpenAI specific options.</param>
        public OpenAiLogic(IOptions<OpenAiOptions> openAiOptions)
        {
            OpenAiOptions = openAiOptions.Value;
            //TODO: Configure system prompt in seperate text file.
            ChatSession.Add(new SystemChatMessage("You are a helpful assistant."));
        }

        #endregion

        #region explicit interfaces

        /// <inheritdoc />
        public async Task GetStreamedCompletion(string userPrompt, Func<string, Task> onChunkReceived)
        {
            var openAiClient = new OpenAIClient(OpenAiOptions.ApiKey);
            var client = openAiClient.GetChatClient(OpenAiOptions.Model);
            ChatSession.Add(new UserChatMessage(userPrompt));
            var fullResponse = string.Empty;
            try
            {
                await foreach (var result in client.CompleteChatStreamingAsync(ChatSession))
                {
                    if (result.ContentUpdate == null || result.ContentUpdate.Count <= 0)
                    {
                        continue;
                    }
                    var chunk = result.ContentUpdate[0].Text;
                    fullResponse += chunk;
                    await onChunkReceived(chunk);
                }
            }
            catch (Exception ex)
            {
                await onChunkReceived($"Error: {ex.Message}");
            }
            ChatSession.Add(new AssistantChatMessage(fullResponse));
        }

        #endregion

        #region properties

        /// <summary>
        /// The OpenAI specific options.
        /// </summary>
        public OpenAiOptions OpenAiOptions { get; }

        /// <summary>
        /// A simple implementation of a chat session to create a chat session for locale development.
        /// </summary>
        public static List<ChatMessage> ChatSession { get; } = [];

        #endregion
    }
}