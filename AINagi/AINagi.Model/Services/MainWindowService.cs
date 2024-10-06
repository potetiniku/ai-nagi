using Microsoft.Extensions.Configuration;

namespace AINagi.Model.Services;

public class MainWindowService
{
	public MainWindowService(HttpClient httpClient)
	{
		IConfigurationRoot appsettings = new ConfigurationBuilder()
		   .AddJsonFile("appsettings.json")
		   .Build();

		elevenLabs = new(httpClient, appsettings["ElevenLabsApiKey"]!);
		voiceId = appsettings["ElevenLabsVoiceId"]!;
	}

	private readonly ElevenLabsClient elevenLabs;
	private readonly string voiceId;

	public async Task<byte[]> GenerateMp3Voice(string text) =>
		await elevenLabs.GenerateMp3Voice(text, voiceId);
}
