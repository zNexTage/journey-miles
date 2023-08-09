using ChatGPT.Net;

namespace API.ExternalServices;

public class ChatBotService
{
    ChatGpt _chatGpt;

    public ChatBotService(string apiKey)
    {
        _chatGpt = new ChatGpt(apiKey);
    }

    public async Task<string> SendMessage(string message)
    {
        var response = await _chatGpt.Ask(message);

        return response;        
    }
}
