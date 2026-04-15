using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;    
using GameLibraryManager.Pages;
using static GameLibraryManager.GameUserControl;

namespace GameLibraryManager
{

    public partial class MainWindow : Window
    {
        public static MainWindow? Instance { get; private set; }
        public ObservableCollection<Game> Games { get; set; } = new ObservableCollection<GameUserControl.Game>();
        public Game? GameToEdit { get; set; }

        public GameLibraryManager.Pages.HomePage HomePage;
        public GameLibraryManager.Pages.LibraryPage LibraryPage;
        public GameLibraryManager.Pages.SettingsPage SettingsPage;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;

            HomePage = new HomePage();
            LibraryPage = new LibraryPage();
            LibraryPage.DataContext = this;
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
            AddGameOverlay.IsVisible = true;
        }
        public async void ConfirmAddGame(object sender, RoutedEventArgs e)
        {
            foreach (var game in Games)
            {
                if (game.Name == NameTextBox.Text && game != GameToEdit)
                {
                    ErrorText.Text = App.GetText("NameExists");
                    ErrorPopup.IsVisible = true;
                    return;
                }
            }
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(GenreComboBox.Text) || string.IsNullOrWhiteSpace(RateComboBox.Text) || string.IsNullOrWhiteSpace(GameDirectoryTextBox.Text))
            {
                ErrorText.Text = App.GetText("FillAllFields");
                ErrorPopup.IsVisible = true;
                return;
            }
            AddGameToList();
            AddGameOverlay.IsVisible = false;
        }
        public void CancelAddGame(object sender, RoutedEventArgs e)
        {
            AddGameOverlay.IsVisible = false;
        }
        public void AddGameToList()
        {
            if (GameToEdit != null)
            {
                GameToEdit.Name = NameTextBox.Text!;
                GameToEdit.Genre = GenreComboBox.Text!;
                GameToEdit.Rate = RateComboBox.Text!;
                GameToEdit.FilePath = GameDirectoryTextBox.Text!;

                GameToEdit = null;
            }
            else
            {
                Games.Add(new GameUserControl.Game {Name = NameTextBox.Text!, Genre = GenreComboBox.Text!, Rate = RateComboBox.Text! + "/5", FilePath = GameDirectoryTextBox.Text!});
            }
            LibraryPage.Instance?.UpdateGamesList();
        }
        public void DeleteGame(Game game)
        {
            Games.Remove(game);
        }
        public void CloseError(object sender, RoutedEventArgs e)
        {
            ErrorPopup.IsVisible = false;
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
                 GameDirectoryTextBox.Text = files[0].Path.LocalPath;
            }
        }
    }
}