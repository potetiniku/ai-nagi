using AINagi.Model.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AINagi.Wpf.Windows.MainWindow;

public partial class ViewModel(MainWindowService service) : ObservableObject
{
	[RelayCommand]
	private async Task GenerateVoice()
	{
		string apiKey = "";
		string voiceId = "";

		service.ApiKey = apiKey;
		service.VoiceId = voiceId;
		MainWindowService tts = service;

		byte[] audioContent = await tts.TextToSpeech("あらゆる現実をすべて自分の方へ捻じ曲げたのだ。");

		if (audioContent != null)
			tts.SaveAudio(audioContent, "generatedVoices");
	}
}
