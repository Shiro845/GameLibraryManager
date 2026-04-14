using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;

namespace GameLibraryManager.Pages
{
    public partial class SettingsPage : UserControl
    {
        public SettingsPage()
        {
            InitializeComponent();
        }
        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageComboBox == null) return;

            if (LanguageComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string langCode = selectedItem.Tag?.ToString() ?? "Ukrainian";

                ChangeLanguage(langCode);
            }
        }
        private void ChangeLanguage(string langCode)
        {
            var currentDict = App.Current!.Resources.MergedDictionaries;
            currentDict.Clear();

            var newDict = new ResourceInclude(new Uri("avares://GameLibraryManager/Assets/Languages/" + langCode + ".axaml"))
            {
                Source = new Uri("avares://GameLibraryManager/Assets/Languages/" + langCode + ".axaml")
            };

            currentDict.Add(newDict);
        }

        private void OpacitySlider_ValueChanged(object sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (OpacitySlider == null) return;

            if (OpacitySlider.Value is double opacity)
            {
                MainWindow.Instance!.Opacity = opacity;
            }
        }
        private void Resolution_Changed(object? sender, SelectionChangedEventArgs e)
        {
            if (ResolutionComboBox?.SelectedItem is ComboBoxItem item && item.Tag != null)
            {
                var res = item.Tag.ToString()!.Split(',');
                int width = int.Parse(res[0]);
                int height = int.Parse(res[1]);

                if (MainWindow.Instance != null)
                {
                    MainWindow.Instance.Width = width;
                    MainWindow.Instance.Height = height;
                    MainWindow.Instance.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
            }
        }
        private void FullscreenCheckBox_IsCheckedChanged(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (FullscreenCheckBox == null) return;
            if (FullscreenCheckBox.IsChecked == true)
            {
                if (MainWindow.Instance != null) { MainWindow.Instance.WindowState = WindowState.FullScreen; }
            }
            else
            {
                if (MainWindow.Instance != null) { MainWindow.Instance.WindowState = WindowState.Normal; }
            }
        }
    }
}
