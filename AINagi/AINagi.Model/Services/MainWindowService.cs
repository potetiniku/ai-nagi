using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace AINagi.Model.Services;

public class MainWindowService
{
	public MainWindowService()
	{
		IConfigurationRoot appsettings = new ConfigurationBuilder()
		   .AddJsonFile("appsettings.json")
		   .Build();

		ApiKey = appsettings["ElevenLabsApiKey"]!;
		VoiceId = appsettings["ElevenLabsVoiceId"]!;
	}

	private const string Url = "https://api.elevenlabs.io/v1/text-to-speech";

	private readonly string ApiKey;
	private readonly string VoiceId;

	public async Task<byte[]> TextToSpeech(string text)
	{
		using HttpClient client = new();
		client.DefaultRequestHeaders.Add("Accept", "audio/wav");
		client.DefaultRequestHeaders.Add("xi-api-key", ApiKey);

		var body = new
		{
			text = text,
			model_id = "eleven_turbo_v2_5",
			voice_settings = new
			{
				stability = 0.5,
				similarity_boost = 0.5
			},
			output_format = "wav"
		};

		StringContent content = new(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

		HttpResponseMessage response = await client.PostAsync($"{Url}/{VoiceId}", content);

		if (!response.IsSuccessStatusCode)
		{
			Console.WriteLine($"エラー: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
			return null;
		}

		return await response.Content.ReadAsByteArrayAsync();
	}

	public void SaveAudio(byte[] audioContent, string outputDir)
	{
		if (!Directory.Exists(outputDir))
		{
			Directory.CreateDirectory(outputDir);
		}

		string filename = $"audio_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.wav";
		string filePath = Path.Combine(outputDir, filename);

		File.WriteAllBytes(filePath, audioContent);
		Console.WriteLine($"音声ファイルを保存しました: {filePath}");
	}
}
