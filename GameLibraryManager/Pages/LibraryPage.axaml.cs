using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace GameLibraryManager.Pages
{
    public partial class LibraryPage : UserControl
    {
        public UserControl AddGamePage = new AddGameUserControl();
        public string? filePath;
        public LibraryPage()
        {
            InitializeComponent();
        }

        public async void AddGameButton(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)TopLevel.GetTopLevel(this)!;
            mainWindow.ShowOverlay();
            //var storage = TopLevel.GetTopLevel(this).StorageProvider;

            //var files = await storage.OpenFilePickerAsync(new FilePickerOpenOptions
            //{
            //    Title = "Select game file",
            //    AllowMultiple = false,
            //    FileTypeFilter = new[]
            //    {
            //        new FilePickerFileType("Executable files") { Patterns = new[] { "*.exe"} },
            //        new FilePickerFileType("All files") { Patterns = new[] { "*.*" } }
            //    }
            //});

            //if (files.Count > 0)
            //{
            //    filePath = files[0].Path.LocalPath;
            //}
        }
    }
}
