using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;    
using GameLibraryManager.Pages;

namespace GameLibraryManager
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        private bool _isOverlayVisible;
        private string _errorMessage = "";
        private bool _isErrorVisible;
        private Game? _gameToEdit;
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
#pragma warning disable CS0108
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore CS0108

        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public static MainWindow? Instance { get; private set; }

        public ObservableCollection<Game> Games { get; set; } = new ObservableCollection<Game>();

        public GameLibraryManager.Pages.HomePage HomePage;
        public GameLibraryManager.Pages.LibraryPage LibraryPage;
        public GameLibraryManager.Pages.SettingsPage SettingsPage;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            this.DataContext = this;
            IsOverlayVisible = false;
            IsErrorVisible = false;

            HomePage = new HomePage();
            LibraryPage = new LibraryPage();
            LibraryPage.UpdateGamesList();
            SettingsPage = new SettingsPage();

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
            GameToEdit = new Game();
            IsOverlayVisible = true;
        }
        public async void ConfirmAddGame(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GameToEdit?.Name) ||
                string.IsNullOrWhiteSpace(GameToEdit?.Genre) ||
                string.IsNullOrWhiteSpace(GameToEdit?.Rate) ||
                string.IsNullOrWhiteSpace(GameToEdit?.FilePath))
            {
                ErrorMessage = App.GetText("FillAllFields");
                IsErrorVisible = true;
                return;
            }
            if (Games.Any(g => g.Name == GameToEdit.Name && g != GameToEdit))
            {
                ErrorMessage = App.GetText("NameExists");
                IsErrorVisible = true;
                return;
            }
            AddGameToList();
        }
        public void CancelAddGame(object sender, RoutedEventArgs e)
        {
            IsOverlayVisible = false;
            GameToEdit = null;
        }
        public void AddGameToList()
        {
            if (!Games.Contains(GameToEdit!))
            {
                Games.Add(GameToEdit!);
            }
            IsOverlayVisible = false;
            GameToEdit = null;
        }
        public void DeleteGame(Game game)
        {
            Games.Remove(game);
        }
        public void CloseError(object sender, RoutedEventArgs e)
        {
            IsErrorVisible = false;
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

            if (files.Count > 0)
            {
                if (GameToEdit == null) GameToEdit = new Game();
                GameToEdit.FilePath = files[0].Path.LocalPath;
            }
        }
    }
}