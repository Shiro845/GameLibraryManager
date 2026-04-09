using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using GameLibraryManager.Pages;

namespace GameLibraryManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainContentArea.Content = new HomePage();
        }

        public void ShowHome(object sender, RoutedEventArgs e)
        {
            MainContentArea.Content = new HomePage();
        }
        public void ShowLibrary(object sender, RoutedEventArgs e)
        {
            MainContentArea.Content = new LibraryPage();
        }
        public void ShowSettings(object sender, RoutedEventArgs e)
        {
            MainContentArea.Content = new SettingsPage();
        }
        public void ExitButton(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}