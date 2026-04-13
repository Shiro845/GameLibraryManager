using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;    
using GameLibraryManager.Pages;
using static GameLibraryManager.GameUserControl;

namespace GameLibraryManager
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<GameUserControl.Game> Games { get; set; } = new ObservableCollection<GameUserControl.Game>();
        public UserControl HomePage;
        public UserControl LibraryPage;
        public UserControl SettingsPage;

        public MainWindow()
        {
            InitializeComponent();

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
        public void ConfirmAddGame(object sender, RoutedEventArgs e)
        {
            AddGameToList();
            AddGameOverlay.IsVisible = false;
        }
        public void CancelAddGame(object sender, RoutedEventArgs e)
        {
            AddGameOverlay.IsVisible = false;
        }
        public void AddGameToList()
        {
            Games.Add(new GameUserControl.Game {Name = NameTextBox.Text, Genre = GenreTextBox.Text, Rate = RateTextBox.Text, FilePath = GameDirectoryTextBox.Text});
        }
        public void DeleteGame(Game game)
        {
            Games.Remove(game);
        }

        public async void BrowseGameDirectory(object sender, RoutedEventArgs e) 
        {
            var storage = TopLevel.GetTopLevel(this)?.StorageProvider;

            var files = await storage!.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Select game file",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Executable files") { Patterns = new[] { "*.exe"} },
                    new FilePickerFileType("All files") { Patterns = new[] { "*.*" } }
                }
            });

            if (files.Count > 0)
            {
                 GameDirectoryTextBox.Text = files[0].Path.LocalPath;
            }
        }
    }
}