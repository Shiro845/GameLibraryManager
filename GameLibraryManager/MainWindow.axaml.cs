using System;
using System.Linq;
using Avalonia.Controls;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using GameLibraryManager.Pages;
using Avalonia.Platform.Storage;    

namespace GameLibraryManager
{
    public partial class MainWindow : Window
    {
        public UserControl HomePage => new HomePage();
        public UserControl LibraryPage => new LibraryPage();
        public UserControl SettingsPage => new SettingsPage();

        public MainWindow()
        {
            InitializeComponent();
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
            AddGameOverlay.IsVisible = false;
        }
        public void CancelAddGame(object sender, RoutedEventArgs e)
        {
            AddGameOverlay.IsVisible = false;
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