using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Threading;
using GameLibraryManager.Model;
using GameLibraryManager.Services;

namespace GameLibraryManager.ViewModel;

/// <summary>
/// Основний ViewModel для керування даними та логікою програми, включаючи список ігор, налаштування та фільтрацію.
/// </summary>
public class MainViewModel : ViewModelBase
{
    /// <summary>
    /// Динамічна колекція ігор, яка відображається в інтерфейсі та зберігається у файлі. Зміни в цій колекції автоматично зберігаються.
    /// </summary>
    public ObservableCollection<Game> Games { get; set; }

    /// <summary>
    /// Показує, чи відображається накладка для додавання/редагування гри.
    /// </summary>
    private bool _isOverlayVisible;

    /// <summary>
    /// Повідомлення про помилку, яке відображається у накладці.
    /// </summary>
    private string _errorMessage = "";

    /// <summary>
    /// Показує, чи відображається повідомлення про помилку.
    /// </summary>
    private bool _isErrorVisible;

    /// <summary>
    /// Гра, яка редагується у накладці.
    /// </summary>
    private Game? _gameToEdit;

    /// <summary>
    /// Текст для фільтрації ігор у бібліотеці. Коли цей текст змінюється, колекція відфільтрованих ігор оновлюється автоматично.
    /// </summary>
    private string? _searchText;

    private object _currentPage;
    public object CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;
            this.OnPropertyChanged(nameof(CurrentPage));
        }
    }

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

    /// <summary>
    /// Налаштування додатка.
    /// </summary>
    private Settings _settings = new();

    public Settings Settings
    {
        get => _settings;
        set { _settings = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Колекція ігор, відфільтрована за текстом у SearchText. Якщо SearchText порожній, повертає всі ігри.
    /// </summary>
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

    /// <summary>
    /// Список жанрів для вибору при додаванні/редагуванні гри.
    /// </summary>
    public List<string> GenreList { get; } = new() { "Action", "Adventure", "Battle Royale", "RPG",
                                                    "MOBA", "Strategy", "Simulator", "Survival",
                                                    "Sports", "Platformer", "Puzzle", "Racing",
                                                    "Roguelike", "Horror", "Another" };

    public ObservableCollection<GenreStat> GenreStats { get; set; } = new();

    /// <summary>
    /// Список оцінок для вибору при додаванні/редагуванні гри.
    /// </summary>
    public List<string> RateList { get; } = new() { "1/5", "2/5", "3/5", "4/5", "5/5" };


    private DateTime _startTime;
    private string? _sessionTime;
    private DispatcherTimer _timer;
    public string SessionTime
    {
        get => _sessionTime!;
        set { _sessionTime = value; OnPropertyChanged(nameof(SessionTime)); }
    }
    public MainViewModel()
    {
        _startTime = DateTime.Now;
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick!;
        _timer.Start();
        SessionTime = "00:00:00";

        Settings = JsonStorage.LoadSettings();

        Games = new ObservableCollection<Game>(JsonStorage.Load());

        ChangeLanguage(Settings.Language == "ua" ? "Ukrainian" : "English");

        foreach (var game in Games)
        {
            game.PropertyChanged += (s, e) => OnGameDataChanged();
        }

        Games.CollectionChanged += (s, e) =>
        {
            if (e.NewItems != null)
            {
                foreach (Game newGame in e.NewItems)
                {
                    newGame.PropertyChanged += (s, e) => OnGameDataChanged();
                }
            }
            OnGameDataChanged();
        };
        UpdateGenreStatistics();
    }

    private void OnGameDataChanged()
    {
        JsonStorage.Save(Games);
        UpdateGenreStatistics();
        OnPropertyChanged(nameof(FilteredGames));
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        TimeSpan elapsed = DateTime.Now - _startTime;
        SessionTime = elapsed.ToString(@"hh\:mm\:ss");
    }
    public ushort TotalGamesCount => (ushort)Games.Count;
    public ushort FavouriteGamesCount => (ushort)Games.Count(g => g.Rate == "5/5");
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



    /// <summary>
    /// Оновлює статистику ігор, викликаючи події зміни властивостей для TotalGamesCount, FavouriteGamesCount та LaunchSortedGames.
    /// </summary>
    public void UpdateStats()
    {
        OnPropertyChanged(nameof(TotalGamesCount));
        OnPropertyChanged(nameof(FavouriteGamesCount));
        OnPropertyChanged(nameof(LaunchSortedGames));
    }

    /// <summary>
    /// Застосовує всі налаштування. Викликається при завантаженні програми для застосування збережених налаштувань.
    /// </summary>
    public void ApplyAllSettings()
    {
        if (MainWindow.Instance == null) return;

        MainWindow.Instance.Opacity = WindowOpacity;
        MainWindow.Instance.WindowState = IsFullscreen ? WindowState.FullScreen : WindowState.Normal;
        ApplyResolution(SelectedResolutionIndex);
    }

    /// <summary>
    /// >Застосовує вибрану роздільну здатність до вікна.
    /// </summary>
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

    /// <summary>
    /// Змінює мову інтерфейсу програми.
    /// </summary>
    public void ChangeLanguage(string langCode)
    {
        var currentDict = App.Current!.Resources.MergedDictionaries;
        currentDict.Clear();
        var uri = new Uri($"avares://GameLibraryManager/Assets/Languages/{langCode}.axaml");
        var resourceInclude = new ResourceInclude(uri) { Source = uri };
        currentDict.Add(resourceInclude);
    }

    public void UpdateGenreStatistics()
    {
        if (Games == null || !Games.Any()) return;

        int totalGames = Games.Count;

        var stats = Games
            .GroupBy(g => g.Genre)
            .Select(group => new GenreStat
            {
                Name = group.Key ?? "Unknown",
                Count = group.Count(),
                Percent = (double)group.Count() / totalGames * 100
            })
            .OrderByDescending(x => x.Count)
            .ToList();

        GenreStats.Clear();
        foreach (var item in stats)
        {
            GenreStats.Add(item);
        }
    }
}
