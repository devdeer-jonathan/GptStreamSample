namespace Services.CoreApi.Controllers
{
    using System.Text;

    using Logic.Interfaces.Logic;
    using Logic.Models;

    using Microsoft.AspNetCore.Mvc;

    public class ChatController : ControllerBase
    {
        #region constructors and destructors

        public ChatController(IOpenAiLogic openAiLogic)
        {
            OpenAiLogic = openAiLogic;
        }

        #endregion

        #region methods

        [HttpPost("StreamCompletion")]
        public async Task StreamChatResponse([FromBody] ChatRequestModel chatRequest)
        {
            Response.ContentType = "text/event-stream";
            Response.StatusCode = 200;
            try
            {
                await OpenAiLogic.GetStreamedCompletion(
                    chatRequest.Prompt,
                    async chunk =>
                    {
                        if (!string.IsNullOrEmpty(chunk))
                        {
                            await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(chunk));
                            await Response.Body.FlushAsync();
                        }
                    });
            }
            catch (Exception ex)
            {
                await Response.Body.WriteAsync(Encoding.UTF8.GetBytes($"Error: {ex.Message}"));
            }
        }

        #endregion

        #region properties

        public IOpenAiLogic OpenAiLogic { get; }

        #endregion
    }
}