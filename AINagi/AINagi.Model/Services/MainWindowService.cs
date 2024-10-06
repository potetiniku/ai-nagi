using Microsoft.Extensions.Configuration;
using OpenAI_API;
using OpenAI_API.Chat;

namespace AINagi.Model.Services;

public class MainWindowService
{
	public MainWindowService(HttpClient httpClient)
	{
		IConfigurationRoot appsettings = new ConfigurationBuilder()
		   .AddJsonFile("appsettings.json")
		   .Build();

		openAIApiKey = appsettings["OpenAIApiKey"]!;
		elevenLabs = new(httpClient, appsettings["ElevenLabsApiKey"]!);
		voiceId = appsettings["ElevenLabsVoiceId"]!;
	}

	private readonly string openAIApiKey;
	private readonly ElevenLabsClient elevenLabs;
	private readonly string voiceId;

	public async Task<byte[]> GenerateMp3Voice(string text) =>
		await elevenLabs.GenerateMp3Voice(text, voiceId);

	public async Task<string> GetAnswer(string prompt)
	{
		OpenAIAPI api = new(openAIApiKey);
		Conversation chat = api.Chat.CreateConversation();
		chat.AppendUserInput(prompt);
		string response = await chat.GetResponseFromChatbotAsync();
		return response;
	}
}
