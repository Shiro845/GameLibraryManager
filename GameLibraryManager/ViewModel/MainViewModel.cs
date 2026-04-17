using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;
using GameLibraryManager.Model;
using GameLibraryManager.Services;

namespace GameLibraryManager.ViewModel;

public class MainViewModel : ViewModelBase
{
    public ObservableCollection<Game> Games { get; set; }

    private bool _isOverlayVisible;
    private string _errorMessage = "";
    private bool _isErrorVisible;
    private Game? _gameToEdit;
    private string? _searchText;

    public bool IsOverlayVisible
    {
        get => _isOverlayVisible;
        set { _isOverlayVisible = value; OnPropertyChanged(); }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(); }
    }

    public bool IsErrorVisible
    {
        get => _isErrorVisible;
        set { _isErrorVisible = value; OnPropertyChanged(); }
    }

    public Game? GameToEdit
    {
        get => _gameToEdit;
        set { _gameToEdit = value; OnPropertyChanged(); }
    }
    public string SearchText
    {
        get => _searchText!;
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilteredGames));
            }
        }
    }
    public double WindowOpacity
    {
        get => Settings.WindowOpacity;
        set
        {
            Settings.WindowOpacity = value;
            if (MainWindow.Instance != null) MainWindow.Instance.Opacity = value;
            OnPropertyChanged();
            JsonStorage.SaveSettings(Settings);
        }
    }

    public bool IsFullscreen
    {
        get => Settings.Fullscreen;
        set
        {
            Settings.Fullscreen = value;
            if (MainWindow.Instance != null)
                MainWindow.Instance.WindowState = value ? WindowState.FullScreen : WindowState.Normal;
            OnPropertyChanged();
            JsonStorage.SaveSettings(Settings);
        }
    }

    public int SelectedLanguage
    {
        get => Settings.Language == "ua" ? 1 : 0;
        set
        {
            Settings.Language = (value == 1) ? "ua" : "en";
            string langCode = (value == 1) ? "Ukrainian" : "English";
            ChangeLanguage(langCode);
            OnPropertyChanged();
            JsonStorage.SaveSettings(Settings);
        }
    }

    public int SelectedResolutionIndex
    {
        get => Settings.Resolution switch
        {
            "1280x720" => 0,
            "1366x768" => 1,
            "1600x900" => 2,
            "1920x1080" => 3,
            "2560x1440" => 4,
            "3840x2160" => 5,
            _ => 0
        };
        set
        {
            Settings.Resolution = value switch
            {
                0 => "1280x720",
                1 => "1366x768",
                2 => "1600x900",
                3 => "1920x1080",
                4 => "2560x1440",
                5 => "3840x2160",
                _ => "1280x720"
            };
            OnPropertyChanged();
            JsonStorage.SaveSettings(Settings);
            ApplyResolution(value);
        }
    }
    private AppSettings _settings = new();
    public AppSettings Settings
    {
        get => _settings;
        set { _settings = value; OnPropertyChanged(); }
    }
    public ObservableCollection<Game> FilteredGames
    {
        get
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                return Games;

            var query = SearchText.ToLower().Trim();
            var filtered = Games.Where(g =>
                (g.Name?.ToLower().Contains(query) ?? false)
            );

            return new ObservableCollection<Game>(filtered);
        }
    }
    public List<string> GenreList { get; } = new() { "Action", "Adventure", "Battle Royale", "RPG",
                                                    "MOBA", "Strategy", "Simulator", "Survival",
                                                    "Sports", "Platformer", "Puzzle", "Racing",
                                                    "Roguelike", "Horror", "Another" };
    public List<string> RateList { get; } = new() { "1/5", "2/5", "3/5", "4/5", "5/5" };
    public MainViewModel()
    {
        Settings = JsonStorage.LoadSettings();

        Games = new ObservableCollection<Game>(JsonStorage.Load());

        ChangeLanguage(Settings.Language == "ua" ? "Ukrainian" : "English");

        foreach (var game in Games)
        {
            game.PropertyChanged += (s, e) => { JsonStorage.Save(Games); UpdateStats(); };
        }

        Games.CollectionChanged += (s, e) =>
        {
            JsonStorage.Save(Games);
            UpdateStats();
            if (e.NewItems != null)
            {
                foreach (Game item in e.NewItems)
                    item.PropertyChanged += (s, e) => { JsonStorage.Save(Games); UpdateStats(); };
            }
        };
    }
    public int TotalGamesCount => Games.Count;
    public int FavouriteGamesCount => Games.Count(g => g.Rate == "5/5");
    public ObservableCollection<Game> LaunchSortedGames
    {
        get
        {
            var sorted = Games
                .Where(g => !string.IsNullOrEmpty(g.LaunchData)) 
                .OrderByDescending(g => DateTime.TryParse(g.LaunchData, out var parsed) ? parsed : DateTime.MinValue)
                .Take(5);
            return new ObservableCollection<Game>(sorted);
        }
    }
    public void UpdateStats()
    {
        OnPropertyChanged(nameof(TotalGamesCount));
        OnPropertyChanged(nameof(FavouriteGamesCount));
        OnPropertyChanged(nameof(LaunchSortedGames));
    }
    public void ApplyAllSettings()
    {
        if (MainWindow.Instance == null) return;

        MainWindow.Instance.Opacity = WindowOpacity;
        MainWindow.Instance.WindowState = IsFullscreen ? WindowState.FullScreen : WindowState.Normal;
        ApplyResolution(SelectedResolutionIndex);
    }
    public void ApplyResolution(int index)
    {
        if (MainWindow.Instance == null) return;

        (int width, int height) = index switch
        {
            0 => (1280, 720),
            1 => (1366, 768),
            2 => (1600, 900),
            3 => (1920, 1080),
            4 => (2560, 1440),
            5 => (3840, 2160),
            _ => (1280, 720)
        };
        MainWindow.Instance.Width = width;
        MainWindow.Instance.Height = height;
        MainWindow.Instance.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }
    public void ChangeLanguage(string langCode)
    {
        var currentDict = App.Current!.Resources.MergedDictionaries;
        currentDict.Clear();
        var uri = new Uri($"avares://GameLibraryManager/Assets/Languages/{langCode}.axaml");
        var resourceInclude = new ResourceInclude(uri) { Source = uri };
        currentDict.Add(resourceInclude);
    }
}
