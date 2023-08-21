using System.Text;
using ChatGPT.Net;
using OpenAI_API;
using OpenAI_API.Chat;

namespace API.ExternalServices;

public class ChatBotService
{
    // ChatGpt _chatGpt;
    private OpenAIAPI _api;
    private Conversation _conversation;

    public const int GPT_MAX_CHAR_ANSWER = 100;
    // private const int GPT_MAX_PARAGRAPH = 2;

    public ChatBotService(string apiKey, string organizationKey)
    {
        _api = new OpenAIAPI(new APIAuthentication(apiKey, organizationKey)); 

        _conversation = _api.Chat.CreateConversation();

        var gptContext = new StringBuilder();

        //Prepare gpt context
        gptContext.Append("Você irá atuar como um guia turístico.");
        gptContext.Append($"Você irá fornecer um resumo sobre o local que lhe for solicitado utilizando no máximo {GPT_MAX_CHAR_ANSWER} caracteres.");
        gptContext.Append($"É imprescritivel que sua resposta tenha no máximo {GPT_MAX_CHAR_ANSWER} caracteres.");
        // gptContext.Append($"Utilize uma linguagem informal e até {GPT_MAX_CHAR_ANSWER} caracteres no máximo em cada parágrafo. É imprescritivel que sua resposta tenha no máximo {GPT_MAX_CHAR_ANSWER} caracteres.");        

        /// give instruction as System
        _conversation.AppendSystemMessage(gptContext.ToString());        
    }

    public async Task<string> SendMessage(string message)
    {
        _conversation.AppendUserInput(message);

        string response = "";

        do{
            response = await _conversation.GetResponseFromChatbotAsync();
        } while(response.Length > GPT_MAX_CHAR_ANSWER);
        
        return response;   
    }
}
