using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.IO;

namespace GameLibraryManager;

public partial class GameUserControl : UserControl
{
    public MainWindow mainWindow = MainWindow.Instance!;
    public struct Game
    {
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Rate { get; set; }
        public string FilePath { get; set; }
    }
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
                mainWindow.ErrorText.Text = App.GetText("GameFileNotFound");
                mainWindow.ErrorPopup.IsVisible = true;
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
        
    }
    public void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (mainWindow != null && DataContext is Game gameToDelete)
        {
            mainWindow.Games.Remove(gameToDelete);
        }
    }
}