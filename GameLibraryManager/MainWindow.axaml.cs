using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using GameLibraryManager.Model;
using GameLibraryManager.Pages;
using GameLibraryManager.ViewModel;

namespace GameLibraryManager;

/// <summary>
/// Головне вікно програми, що відповідає за навігацію та керування основним інтерфейсом.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Отримує статичний екземпляр поточного вікна для доступу з інших частин програми.
    /// </summary>
    public static MainWindow? Instance { get; private set; }

    /// <summary>
    /// Сторінка домашнього екрана.
    /// </summary>
    private readonly HomePage homePage;

    /// <summary>
    /// Сторінка бібліотеки ігор.
    /// </summary>
    private readonly LibraryPage libraryPage;

    /// <summary>
    /// Сторінка налаштувань.
    /// </summary>
    private readonly SettingsPage settingsPage;

    public MainWindow()
    {
        InitializeComponent();
        CanResize = false;
        Instance = this;
        this.DataContext = new MainViewModel();

        homePage = new HomePage();
        libraryPage = new LibraryPage();
        settingsPage = new SettingsPage();

        if (this.DataContext is MainViewModel vm)
        {
            vm.ApplyAllSettings();
        }

        MainContentArea.Content = libraryPage;
    }

    /// <summary>
    /// Показує домашню сторінку.
    /// </summary>
    public void ShowHome(object sender, RoutedEventArgs e)
    {
        MainContentArea.Content = homePage;
    }

    /// <summary>
    /// Показує сторінку бібліотеки ігор.
    /// </summary>
    public void ShowLibrary(object sender, RoutedEventArgs e)
    {
        MainContentArea.Content = libraryPage;
    }

    /// <summary>
    /// Показує сторінку налаштувань.
    /// </summary>
    public void ShowSettings(object sender, RoutedEventArgs e)
    {
        MainContentArea.Content = settingsPage;
    }

    /// <summary>
    /// Виходить з програми.
    /// </summary>
    public void ExitButton(object sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }

    /// <summary>
    /// Показує накладку для додавання нової гри.
    /// </summary>
    public void ShowOverlay()
    {
        if (this.DataContext is MainViewModel vm)
        {
            vm.GameToEdit = new Game();
            vm.IsOverlayVisible = true;
        }
    }

    /// <summary>
    /// Підтвердження додавання/редагування гри.
    /// </summary>
    public async void ConfirmAddGame(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is MainViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.GameToEdit?.Name) ||
            string.IsNullOrWhiteSpace(vm.GameToEdit?.Genre) ||
            string.IsNullOrWhiteSpace(vm.GameToEdit?.Rate) ||
            string.IsNullOrWhiteSpace(vm.GameToEdit?.FilePath))
            {
                vm.ErrorMessage = App.GetText("FillAllFields");
                vm.IsErrorVisible = true;
                return;
            }
            if (vm.Games.Any(g => g.Name == vm.GameToEdit.Name && g != vm.GameToEdit))
            {
                vm.ErrorMessage = App.GetText("NameExists");
                vm.IsErrorVisible = true;
                return;
            }
        }

        AddGameToList();
    }

    /// <summary>
    /// Скасовує додавання/редагування гри.
    /// </summary>
    public void CancelAddGame(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is MainViewModel vm)
        {
            vm.IsOverlayVisible = false;
            vm.GameToEdit = null;
        }
    }

    /// <summary>
    /// Додавання нової гри до списку або оновлення існуючої після редагування.
    /// </summary>
    public void AddGameToList()
    {
        if (this.DataContext is MainViewModel vm)
        {
            if (!vm.Games.Contains(vm.GameToEdit!))
            {
                vm.Games.Add(vm.GameToEdit!);
            }

            vm.IsOverlayVisible = false;
            vm.GameToEdit = null;
        }
    }

    /// <summary>
    /// Видаляє гру зі списку.
    /// </summary>
    public void DeleteGame(Game game)
    {
        if (this.DataContext is ViewModel.MainViewModel vm)
        {
            vm.Games.Remove(game);
        }
    }

    /// <summary>
    /// Закриває повідомлення про помилку.
    /// </summary>
    public void CloseError(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is ViewModel.MainViewModel vm)
        {
            vm.IsErrorVisible = false;
        }
    }

    /// <summary>
    /// Відкриває діалог вибору файлу для вибору виконуваного файлу гри та встановлює його шлях у відповідне поле.
    /// </summary>
    public async void BrowseGameDirectory(object sender, RoutedEventArgs e)
    {
        var storage = TopLevel.GetTopLevel(this)?.StorageProvider;

        var files = await storage!.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = App.GetText("SelectGameFile"),
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType(App.GetText("ExecutableFiles")) { Patterns = new[] { "*.exe*"} },
                new FilePickerFileType(App.GetText("AllFiles")) { Patterns = new[] { "*.*" } }
            }
        });

        if (files.Count > 0 && DataContext is MainViewModel vm)
        {
            if (vm.GameToEdit == null) vm.GameToEdit = new Game();
            vm.GameToEdit.FilePath = files[0].Path.LocalPath;
        }
    }
}