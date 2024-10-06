using System.IO;
using System.Reactive.Linq;
using AINagi.Model.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NAudio.Wave;
using Reactive.Bindings;

namespace AINagi.Wpf.Windows.MainWindow;

public partial class ViewModel : ObservableObject
{
	public ViewModel(MainWindowService service)
	{
		this.service = service;

		CanSendMessage = Prompt
			.Select(p => p.Length != 0)
			.ToReadOnlyReactiveProperty();
	}

	public ReactiveProperty<string> Prompt { get; } = new(string.Empty);
	public ReactiveProperty<string> Answer { get; } = new(string.Empty);
	public ReadOnlyReactiveProperty<bool> CanSendMessage { get; }

	private readonly MainWindowService service;

	[RelayCommand]
	private async Task SendMessage()
	{
		string answer = await service.GetAnswer(Prompt.Value);
		byte[] mp3 = await service.GenerateMp3Voice(answer);

		Answer.Value = answer;
		await PlayMp3Audio(mp3);
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
