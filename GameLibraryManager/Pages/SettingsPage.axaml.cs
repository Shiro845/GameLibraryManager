using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;

namespace GameLibraryManager.Pages
{
    public partial class SettingsPage : UserControl
    {
        public new event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        public SettingsPage()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        private double _windowOpacity = 1.0;
        public double WindowOpacity
        {
            get => _windowOpacity;
            set
            {
                _windowOpacity = value;
                if (MainWindow.Instance != null) MainWindow.Instance.Opacity = value;
                OnPropertyChanged();
            }
        }

        private bool _isFullscreen;
        public bool IsFullscreen
        {
            get => _isFullscreen;
            set
            {
                _isFullscreen = value;
                if (MainWindow.Instance != null)
                    MainWindow.Instance.WindowState = value ? WindowState.FullScreen : WindowState.Normal;
                OnPropertyChanged();
            }
        }

        private int _selectedLanguageIndex;
        public int SelectedLanguageIndex
        {
            get => _selectedLanguageIndex;
            set
            {
                _selectedLanguageIndex = value;
                string langCode = (value == 1) ? "Ukrainian" : "English";
                ChangeLanguage(langCode);
                OnPropertyChanged();
            }
        }

        private int _selectedResolutionIndex;
        public int SelectedResolutionIndex
        {
            get => _selectedResolutionIndex;
            set
            {
                _selectedResolutionIndex = value;
                ApplyResolution(value);
                OnPropertyChanged();
            }
        }
        private void ApplyResolution(int index)
        {
            if (MainWindow.Instance == null) return;

            (int width, int height) = index switch
            {
                0 => (1280, 720),
                1 => (1366, 768),
                2 => (1600, 900),
                3 => (1920, 1080),
                4 => (2560, 1440),
                5 => (3840, 2160),
                _ => (1280, 720)
            };
            MainWindow.Instance.Width = width;
            MainWindow.Instance.Height = height;
            MainWindow.Instance.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }   
        private void ChangeLanguage(string langCode)
        {
            var currentDict = App.Current!.Resources.MergedDictionaries;
            currentDict.Clear();
            var uri = new Uri($"avares://GameLibraryManager/Assets/Languages/{langCode}.axaml");
            var resourceInclude = new ResourceInclude(uri) { Source = uri };
            currentDict.Add(resourceInclude);
        }
    }
}
