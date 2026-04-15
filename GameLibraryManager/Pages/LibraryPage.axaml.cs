using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace GameLibraryManager.Pages
{
    public partial class LibraryPage : UserControl
    {
        public static LibraryPage? Instance { get; private set; }
        public LibraryPage()
        {
            InitializeComponent();
            Instance = this;
        }

        private async void AddGameButton(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)TopLevel.GetTopLevel(this)!;
            mainWindow.ShowOverlay();
        }
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = searchBox.Text?.ToLower() ?? "";
            var allGames = MainWindow.Instance?.Games;

            if (allGames == null) return;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                GamesControl.ItemsSource = allGames;
            }
            else
            {
                var filteredGames = allGames.Where(g =>
                g.Name!.ToLower().Contains(searchText)).ToList();
                GamesControl.ItemsSource = filteredGames;
            }
        }
        public void UpdateGamesList()
        {
            if (GamesControl != null)
            {
                var temp = GamesControl.ItemsSource;
                GamesControl.ItemsSource = null;
                GamesControl.ItemsSource = temp;
            }
        }

        private void searchBox_TextChanged(object? sender, TextChangedEventArgs e)
        {
        }
    }
}
