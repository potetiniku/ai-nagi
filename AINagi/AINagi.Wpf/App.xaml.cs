using System.Windows;
using AINagi.Model.Services;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace AINagi.Wpf;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	public App()
	{
		Ioc.Default.ConfigureServices(new ServiceCollection()
			.AddSingleton<MainWindowService>()
			.AddTransient<Windows.MainWindow.ViewModel>()
			.BuildServiceProvider());
	}
}

