using System.Text;
using System.Text.Json;

namespace AINagi.Model.Services;

public class MainWindowService
{
	private const string Url = "https://api.elevenlabs.io/v1/text-to-speech";
	public string ApiKey { get; set; } = null!;
	public string VoiceId { get; set; } = null!;

	public async Task<byte[]> TextToSpeech(string text)
	{
		using HttpClient client = new();
		// APIリクエストのヘッダーを設定
		client.DefaultRequestHeaders.Add("Accept", "audio/wav");
		client.DefaultRequestHeaders.Add("xi-api-key", ApiKey);

		// APIリクエストのボディを設定
		var body = new
		{
			text = text,
			model_id = "eleven_turbo_v2_5",  // モデルID
			voice_settings = new { stability = 0.5, similarity_boost = 0.5 },  // 音声設定
			output_format = "wav"
		};

		StringContent content = new(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

		// APIにPOSTリクエストを送信
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

		// タイムスタンプを使用してユニークなファイル名を生成
		string filename = $"audio_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.wav";
		string filePath = Path.Combine(outputDir, filename);

		// 音声データをファイルに書き込む
		File.WriteAllBytes(filePath, audioContent);
		Console.WriteLine($"音声ファイルを保存しました: {filePath}");
	}
}
