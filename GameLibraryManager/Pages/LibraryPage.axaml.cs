using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace GameLibraryManager.Pages
{
    public partial class LibraryPage : UserControl, INotifyPropertyChanged
    {
        public static LibraryPage? Instance { get; private set; }
        
        public new event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public LibraryPage()
        {
            InitializeComponent();
            Instance = this;
            DataContext = this;
            
            if (MainWindow.Instance != null)
            {
                MainWindow.Instance.Games.CollectionChanged += (s, e) => UpdateGamesList();
            }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    UpdateGamesList();
                }
            }
        }

        public ObservableCollection<Game> FilteredGames { get; } = new();

        public void UpdateGamesList()
        {
            var allGames = MainWindow.Instance?.Games;
            if (allGames == null) return;

            var query = SearchText?.ToLower() ?? "";
            var results = allGames.Where(g =>
                string.IsNullOrWhiteSpace(query) ||
                (g.Name?.ToLower().Contains(query) ?? false) ||
                (g.Genre?.ToLower().Contains(query) ?? false)).ToList();

            FilteredGames.Clear();
            foreach (var game in results)
            {
                FilteredGames.Add(game);
            }
            OnPropertyChanged(nameof(FilteredGames));
        }

        private void AddGameButton(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance?.ShowOverlay();
        }
    }
}
