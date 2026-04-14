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
        public void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
    }
}
