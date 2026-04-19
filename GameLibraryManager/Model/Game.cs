using GameLibraryManager.ViewModel;

namespace GameLibraryManager.Model;

/// <summary>
/// Модель даних для представлення інформації про гру, включаючи назву, жанр, рейтинг, шлях до файлу та дату запуску.
/// </summary>
public class Game : ViewModelBase
{
    /// <summary>
    /// Назва гри.
    /// </summary>
    private string? _name;

    /// <summary>
    /// Жанр гри.
    /// </summary>
    private string? _genre;

    /// <summary>
    /// Рейтинг гри.
    /// </summary>
    private string? _rate;

    /// <summary>
    /// Шлях до файлу гри.
    /// </summary>
    private string? _filePath;

    /// <summary>
    /// Дата запуску гри.
    /// </summary>
    private string? _launchData;

    public string Name
    {
        get => _name!;
        set { _name = value; OnPropertyChanged(); }
    }

    public string Genre
        {
        get => _genre!;
        set { _genre = value; OnPropertyChanged(); }
    }

    public string Rate
        {
        get => _rate!;
        set { _rate = value; OnPropertyChanged(); }
    }

    public string FilePath
        {
        get => _filePath!;
        set { _filePath = value; OnPropertyChanged(); }
    }

    public string LaunchData
        {
        get => _launchData!;
        set { _launchData = value; OnPropertyChanged(); }
    }
}
