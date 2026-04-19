using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using GameLibraryManager.Model;
using GameLibraryManager.ViewModel;

namespace GameLibraryManager;

/// <summary>
/// >Контрол гри для відображення інформації про неї та надання можливості запуску, редагування або видалення з бібліотеки.
/// </summary>
public partial class GameUserControl : UserControl
{
    public GameUserControl()
    {
        InitializeComponent();
    }

    private void LaunchButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is Game gameData)
        {
            if (!File.Exists(gameData.FilePath))
            {
                if (MainWindow.Instance?.DataContext is MainViewModel vm)
                {
                    vm.ErrorMessage = App.GetText("GameFileNotFound");
                    vm.IsErrorVisible = true;
                }
                return;
            }
            gameData.LaunchData = DateTime.Now.ToString("G");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = gameData.FilePath,
                UseShellExecute = true
            });
        }
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is Game gameData)
        {
            if (MainWindow.Instance?.DataContext is MainViewModel vm)
            {
                vm.GameToEdit = gameData;
                vm.IsOverlayVisible = true;
            }
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is Game gameToDelete)
        {
            if (MainWindow.Instance?.DataContext is MainViewModel vm)
            {
                vm.Games.Remove(gameToDelete);
            }
        }
    }
}