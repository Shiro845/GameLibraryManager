using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
using GameLibraryManager.Model;

namespace GameLibraryManager.Services;

public static class JsonStorage
{
    private static readonly string BasePath = AppContext.BaseDirectory;
    private static readonly string FolderPath = Path.Combine(BasePath, "Data");
    private static readonly string FilePath = Path.Combine(FolderPath, "library.json");
    private static readonly string SettingsPath = Path.Combine(FolderPath, "settings.json");

    public static void Save(ObservableCollection<Game> games)
    {
        if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(games, options);
        File.WriteAllText(FilePath, json);
    }

    public static List<Game> Load()
    {
        if (!File.Exists(FilePath)) return new List<Game>();
        string json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<Game>>(json) ?? new List<Game>();
    }

    public static void SaveSettings(AppSettings settings)
    {
        if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(settings, options);
        File.WriteAllText(SettingsPath, json);
    }

    public static AppSettings LoadSettings()
    {
        if (!File.Exists(SettingsPath)) return new AppSettings();
        string json = File.ReadAllText(SettingsPath);
        return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
    }
}