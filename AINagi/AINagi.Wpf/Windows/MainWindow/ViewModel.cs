using System.IO;
using AINagi.Model.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NAudio.Wave;
using Reactive.Bindings;

namespace AINagi.Wpf.Windows.MainWindow;

public partial class ViewModel(MainWindowService service) : ObservableObject
{
	public ReactiveProperty<string> Prompt { get; } = new();
	public ReactiveProperty<string> Answer { get; } = new();

	[RelayCommand]
	private async Task SendMessage()
	{
		Answer.Value = await service.GetAnswer(Prompt.Value);
	}

	private async Task GenerateVoice()
	{
		await PlayMp3Audio(await service.GenerateMp3Voice("あらゆる現実をすべて自分の方へ捻じ曲げたのだ。"));
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
