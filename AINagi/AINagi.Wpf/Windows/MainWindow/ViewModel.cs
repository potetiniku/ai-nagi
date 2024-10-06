using AINagi.Model.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AINagi.Wpf.Windows.MainWindow;

public partial class ViewModel(MainWindowService service) : ObservableObject
{
	[RelayCommand]
	private void GenerateVoice()
	{

	}
}
