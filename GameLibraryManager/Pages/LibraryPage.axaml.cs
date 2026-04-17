using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using GameLibraryManager.Model;
using GameLibraryManager.ViewModel;

namespace GameLibraryManager.Pages
{
    public partial class LibraryPage : UserControl
    {
        public LibraryPage()
        {
            InitializeComponent();
        }

        private void AddGameButton(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance?.ShowOverlay();
        }
    }
}
