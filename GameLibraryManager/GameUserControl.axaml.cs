using Avalonia.Controls;
using Avalonia.Interactivity;
using System.IO;
using GameLibraryManager.Pages;

namespace GameLibraryManager;

public partial class GameUserControl : UserControl
{
    public MainWindow mainWindow = MainWindow.Instance!;
    public GameUserControl()
    { 
        InitializeComponent();
    }

    public void LaunchButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is Game gameData)
        {
            if (!File.Exists(gameData.FilePath))
            {
                mainWindow.ErrorMessage = App.GetText("GameFileNotFound");
                mainWindow.IsErrorVisible = true;
                return;
            }
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = gameData.FilePath,
                UseShellExecute = true
            });
        }
    }

    public void EditButton_Click(object sender, RoutedEventArgs e)
    {
         mainWindow.ShowOverlay();
         if (DataContext is Game gameData)
         {
            mainWindow.GameToEdit = gameData;
            mainWindow.IsOverlayVisible = true;
        }
    }
    public void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (mainWindow != null && DataContext is Game gameToDelete)
        {
            mainWindow.Games.Remove(gameToDelete);
            LibraryPage.Instance?.UpdateGamesList();
        }
    }
}