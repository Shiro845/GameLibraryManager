using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GameLibraryManager.ViewModel;

/// <summary>
/// Базовий клас для всіх ViewModel.
/// </summary>
public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
