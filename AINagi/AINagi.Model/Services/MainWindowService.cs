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

		openAIApi = new(appsettings["OpenAIApiKey"]!);
		conversation = openAIApi.Chat.CreateConversation();

		elevenLabs = new(httpClient, appsettings["ElevenLabsApiKey"]!);
		voiceId = appsettings["ElevenLabsVoiceId"]!;
	}

	private readonly OpenAIAPI openAIApi;
	private readonly Conversation conversation;

	private readonly ElevenLabsClient elevenLabs;
	private readonly string voiceId;

	public async Task<string> GetAnswer(string prompt)
	{
		conversation.AppendUserInput(prompt);
		string response = await conversation.GetResponseFromChatbotAsync();
		return response;
	}

	public async Task<byte[]> GenerateMp3Voice(string text) =>
		await elevenLabs.GenerateMp3Voice(text, voiceId);
}
