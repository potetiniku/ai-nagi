// Ignore Spelling: api

using System.Text;
using System.Text.Json;

namespace AINagi.Model;

public class ElevenLabsClient
{
	public ElevenLabsClient(HttpClient httpClient, string apiKey)
	{
		this.apiKey = apiKey;

		this.httpClient = httpClient;
		httpClient.DefaultRequestHeaders.Add("xi-api-key", this.apiKey);
	}

	private readonly HttpClient httpClient;
	private readonly string apiKey;
	private const string endpoint = "https://api.elevenlabs.io/v1/text-to-speech";

	public async Task<byte[]> GenerateMp3Voice(string text, string voiceId)
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
		HttpResponseMessage response = await httpClient.PostAsync($"{endpoint}/{voiceId}", content);

		if (!response.IsSuccessStatusCode)
			throw new HttpRequestException($"エラー: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

		return await response.Content.ReadAsByteArrayAsync();
	}
}
