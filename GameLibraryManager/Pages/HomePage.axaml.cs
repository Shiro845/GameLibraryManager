using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.IO;
using Avalonia.Controls;
using GameLibraryManager.Model;

namespace GameLibraryManager.Pages;

public partial class HomePage : UserControl, INotifyPropertyChanged
{
    public static HomePage? Instance { get; private set; }
    public ObservableCollection<Game> LaunchSortedGames { get; set; } = new ObservableCollection<Game>();
    public ushort TotalGamesCount => (ushort)(MainWindow.Instance?.Games.Count ?? 0);
    public ushort FavouriteGamesCount => (ushort)(MainWindow.Instance?.Games.Count(g => g.Rate == "5/5") ?? 0);
    public new event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    public HomePage()
    {
        InitializeComponent();
        Instance = this;
        this.DataContext = this;

        if (MainWindow.Instance != null)
        {
            MainWindow.Instance.Games.CollectionChanged += (s, e) =>
            {
                UpdateLaunchSortedGames();
                OnPropertyChanged(nameof(TotalGamesCount));
                OnPropertyChanged(nameof(FavouriteGamesCount));
            };
        }
        UpdateLaunchSortedGames();
    }

    private void RandomGame_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (MainWindow.Instance != null)
        {
            var games = MainWindow.Instance?.Games;
            if (games == null || games.Count == 0) return;

            var random = new Random();
            var randomGame = games[random.Next(games.Count)];
            if (!File.Exists(randomGame.FilePath))
            {
                MainWindow.Instance.ErrorMessage = App.GetText("GameFileNotFound");
                MainWindow.Instance.IsErrorVisible = true;
                return;
            }
            randomGame.LaunchData = DateTime.Now.ToString("G");
            HomePage.Instance?.UpdateLaunchSortedGames();
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = randomGame.FilePath,
                UseShellExecute = true
            });
        }
    }
    public void UpdateLaunchSortedGames()
    {
        var games = MainWindow.Instance?.Games;
        if (games == null) return;
        var sorted = new List<Game>(games);
        sorted = games
            .Where(g => !string.IsNullOrEmpty(g.LaunchData))
            .OrderByDescending(g => DateTime.TryParse(g.LaunchData, out var parsed) ? parsed : DateTime.MinValue)
            .Take(5)
            .ToList();
        LaunchSortedGames.Clear();
        foreach (var game in sorted)
        {
            if (!string.IsNullOrEmpty(game.LaunchData))
            {
                LaunchSortedGames.Add(game);
            }
        }
        OnPropertyChanged(nameof(LaunchSortedGames));
    }
    public void UpdateAll()
    {
        UpdateLaunchSortedGames();
        OnPropertyChanged(nameof(TotalGamesCount));
        OnPropertyChanged(nameof(FavouriteGamesCount));
    }
}