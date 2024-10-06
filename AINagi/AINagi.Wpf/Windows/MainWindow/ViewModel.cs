using System.IO;
using AINagi.Model.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NAudio.Wave;

namespace AINagi.Wpf.Windows.MainWindow;

public partial class ViewModel(MainWindowService service) : ObservableObject
{
	[RelayCommand]
	private async Task GenerateVoice()
	{
		await PlayMp3Audio(await service.TextToSpeech("あらゆる現実をすべて自分の方へ捻じ曲げたのだ。"));
	}

	private async Task PlayMp3Audio(byte[] mp3)
	{
		using MemoryStream stream = new(mp3);
		using Mp3FileReader reader = new(stream);
		using WaveOut outputDevice = new();
		outputDevice.Init(reader);
		outputDevice.Play();

		await Task.Delay(reader.TotalTime);
	}
}
