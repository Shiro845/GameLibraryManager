using System;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using GameLibraryManager.ViewModel;

namespace GameLibraryManager.Pages;

public partial class HomePage : UserControl
{
    private MainViewModel? ViewModel => DataContext as MainViewModel;
    public HomePage()
    {
        InitializeComponent();
    }

    private void RandomGame_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (ViewModel == null) return;
        var games = ViewModel.Games;
        if (games == null || games.Count == 0) return;

        var random = new Random();
        var randomGame = games[random.Next(games.Count)];
        if (!File.Exists(randomGame.FilePath))
        {
            ViewModel!.ErrorMessage = App.GetText("GameFileNotFound");
            ViewModel!.IsErrorVisible = true;
            return;
        }
        randomGame.LaunchData = DateTime.Now.ToString("G");
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = randomGame.FilePath,
            UseShellExecute = true
        });
    }
}