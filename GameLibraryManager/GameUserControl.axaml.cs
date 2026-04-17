using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using GameLibraryManager.Model;
using GameLibraryManager.Pages;

namespace GameLibraryManager;

public partial class GameUserControl : UserControl
{
    private MainWindow mainWindow = MainWindow.Instance!;
    public GameUserControl()
    { 
        InitializeComponent();
    }

    private void LaunchButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is Game gameData)
        {
            if (!File.Exists(gameData.FilePath))
            {
                mainWindow.ErrorMessage = App.GetText("GameFileNotFound");
                mainWindow.IsErrorVisible = true;
                return;
            }
            gameData.LaunchData = DateTime.Now.ToString("G");
            HomePage.Instance?.UpdateLaunchSortedGames();
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = gameData.FilePath,
                UseShellExecute = true
            });
        }
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
         mainWindow.ShowOverlay();
         if (DataContext is Game gameData)
         {
            mainWindow.GameToEdit = gameData;
            mainWindow.IsOverlayVisible = true;
         }
            LibraryPage.Instance?.UpdateGamesList();
    }
    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (mainWindow != null && DataContext is Game gameToDelete)
        {
            mainWindow.Games.Remove(gameToDelete);
            LibraryPage.Instance?.UpdateGamesList();
        }
    }
}