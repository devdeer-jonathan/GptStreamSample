namespace Logic.Interfaces.Logic
{
    /// <summary>
    /// Must be implemented by all logic interacting with OpenAI.
    /// </summary>
    public interface IOpenAILogic
    {
        #region methods

        /// <summary>
        /// Sends a prompt to OpenAI and streams the response as it is generated.
        /// </summary>
        /// <param name="userPrompt">The prompt to send to the OpenAI model.</param>
        /// <param name="onChunkReceived">
        /// A callback function that will be invoked each time a new chunk of content is received from the OpenAI model.
        /// </param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task GetStreamedCompletion(string userPrompt, Func<string, Task> onChunkReceived);

        #endregion
    }
}