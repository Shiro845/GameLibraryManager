using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using GameLibraryManager.Model;

namespace GameLibraryManager.Services;

/// <summary>
/// Сервіс для збереження та завантаження даних у форматі JSON. Включає методи для роботи з бібліотекою ігор та налаштуваннями програми.
/// </summary>
public static class JsonStorage
{
    private static readonly string BasePath = AppContext.BaseDirectory;
    private static readonly string FolderPath = Path.Combine(BasePath, "Data");
    private static readonly string FilePath = Path.Combine(FolderPath, "library.json");
    private static readonly string SettingsPath = Path.Combine(FolderPath, "settings.json");

    /// <summary>
    /// Зберігає колекцію ігор у файл JSON. Якщо папка не існує, вона буде створена.
    /// </summary>
    public static void Save(ObservableCollection<Game> games)
    {
        if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(games, options);
        File.WriteAllText(FilePath, json);
    }

    /// <summary>
    /// Завантажує колекцію ігор з файлу JSON. Якщо файл не існує, повертається порожній список.
    /// </summary>
    /// <returns>Список ігор.</returns>
    public static List<Game> Load()
    {
        if (!File.Exists(FilePath)) return new List<Game>();
        string json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<Game>>(json) ?? new List<Game>();
    }

    /// <summary>
    /// Зберігає налаштування програми у файл JSON. Якщо папка не існує, вона буде створена.
    /// </summary>
    public static void SaveSettings(AppSettings settings)
    {
        if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(settings, options);
        File.WriteAllText(SettingsPath, json);
    }

    /// <summary>
    /// >Завантажує налаштування програми з файлу JSON. Якщо файл не існує, повертається новий екземпляр AppSettings з дефолтними значеннями.
    /// </summary>
    /// <returns>Налаштування програми.</returns>
    public static AppSettings LoadSettings()
    {
        if (!File.Exists(SettingsPath)) return new AppSettings();
        string json = File.ReadAllText(SettingsPath);
        return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
    }
}