namespace Logic.Models.Options
{
    /// <summary>
    /// The OpenAI specific configurations.
    /// </summary>
    public class OpenAiOptions
    {
        #region properties

        /// <summary>
        /// The API key to access the OpenAI API.
        /// </summary>
        public string ApiKey { get; set; } = default!;

        /// <summary>
        /// The name of the model used to generate completions.
        /// </summary>
        public string Model { get; set; } = default!;

        #endregion
    }
}