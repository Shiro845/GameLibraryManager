using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace GameLibraryManager.Pages
{
    public partial class LibraryPage : UserControl
    {
        public LibraryPage()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public async void AddGameButton(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)TopLevel.GetTopLevel(this)!;
            mainWindow.ShowOverlay();
        }
        
    }
}
