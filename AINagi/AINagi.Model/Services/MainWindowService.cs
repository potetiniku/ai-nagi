using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace AINagi.Model.Services;

public class MainWindowService
{
	public MainWindowService(HttpClient httpClient)
	{
		this.httpClient = httpClient;

		IConfigurationRoot appsettings = new ConfigurationBuilder()
		   .AddJsonFile("appsettings.json")
		   .Build();

		ApiKey = appsettings["ElevenLabsApiKey"]!;
		VoiceId = appsettings["ElevenLabsVoiceId"]!;
		httpClient.DefaultRequestHeaders.Add("xi-api-key", ApiKey);
	}

	private readonly HttpClient httpClient;
	private const string Url = "https://api.elevenlabs.io/v1/text-to-speech";

	private readonly string ApiKey;
	private readonly string VoiceId;

	public async Task<byte[]> TextToSpeech(string text)
	{
		var body = new
		{
			text = text,
			model_id = "eleven_multilingual_v2",
			voice_settings = new
			{
				stability = 0.5,
				similarity_boost = 0.75,
				style = 0.25
			}
		};

		StringContent content = new(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
		HttpResponseMessage response = await httpClient.PostAsync($"{Url}/{VoiceId}", content);

		if (!response.IsSuccessStatusCode)
			throw new HttpRequestException($"エラー: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

		return await response.Content.ReadAsByteArrayAsync();
	}
}
