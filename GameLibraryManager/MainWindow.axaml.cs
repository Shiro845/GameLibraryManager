using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using GameLibraryManager.Model;
using GameLibraryManager.Pages;
using GameLibraryManager.ViewModel;

namespace GameLibraryManager;

public partial class MainWindow : Window
{
    public static MainWindow? Instance { get; private set; }

    public HomePage HomePage;
    public LibraryPage LibraryPage;
    public SettingsPage SettingsPage;

    public MainWindow()
    {
        InitializeComponent();
        Instance = this;
        this.DataContext = new MainViewModel();

        HomePage = new HomePage();
        LibraryPage = new LibraryPage();
        SettingsPage = new SettingsPage();
        
        if (this.DataContext is MainViewModel vm)
        {
            vm.ApplyAllSettings();
        }

        MainContentArea.Content = LibraryPage;
    }

    public void ShowHome(object sender, RoutedEventArgs e)
    {
        MainContentArea.Content = HomePage;
    }
    public void ShowLibrary(object sender, RoutedEventArgs e)
    {
        MainContentArea.Content = LibraryPage;
    }
    public void ShowSettings(object sender, RoutedEventArgs e)
    {
        MainContentArea.Content = SettingsPage;
    }
    public void ExitButton(object sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }
    public void ShowOverlay()
    {
        if (this.DataContext is MainViewModel vm)
        {
            vm.GameToEdit = new Game();
            vm.IsOverlayVisible = true;
        }
    }
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
    public void CancelAddGame(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is MainViewModel vm)
        {
            vm.IsOverlayVisible = false;
            vm.GameToEdit = null;
        }
    }
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
    public void DeleteGame(Game game)
    {
        if (this.DataContext is ViewModel.MainViewModel vm)
        {
            vm.Games.Remove(game);
        }
    }
    public void CloseError(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is ViewModel.MainViewModel vm)
        {
            vm.IsErrorVisible = false;
        }
    }

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