using Avalonia.Controls;
using Avalonia.Interactivity;

namespace GameLibraryManager.Pages;

/// <summary>
/// Сторінка бібліотеки ігор.
/// </summary>
public partial class LibraryPage : UserControl
{
    public LibraryPage()
    {
        InitializeComponent();
    }

    private void AddGameButton(object sender, RoutedEventArgs e)
    {
        MainWindow.Instance?.ShowOverlay();
    }
}
