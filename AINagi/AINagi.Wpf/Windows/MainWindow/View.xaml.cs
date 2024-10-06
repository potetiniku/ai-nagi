using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace AINagi.Wpf.Windows.MainWindow;

/// <summary>
/// View.xaml の相互作用ロジック
/// </summary>
public partial class View : Window
{
	public View()
	{
		InitializeComponent();
		DataContext = Ioc.Default.GetService<ViewModel>();
	}
}
