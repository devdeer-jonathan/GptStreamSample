namespace Logic.Models
{
    /// <summary>
    /// Represents the a chat request.
    /// </summary>
    public class ChatRequestModel
    {
        #region properties

        /// <summary>
        /// The user prompt to send to the LLM.
        /// </summary>
        public string Prompt { get; set; } = default!;

        #endregion
    }
}