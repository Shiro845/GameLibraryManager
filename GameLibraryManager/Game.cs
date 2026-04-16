using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace GameLibraryManager
{
    public class Game : INotifyPropertyChanged
    {
        private string? _name;
        private string? _genre;
        private string? _rate;
        private string? _filePath;
        public string Name
        {
            get => _name!;
            set { _name = value; OnPropertyChanged(); }
        }
        public string Genre
            {
            get => _genre!;
            set { _genre = value; OnPropertyChanged(); }
        }
        public string Rate
            {
            get => _rate!;
            set { _rate = value; OnPropertyChanged(); }
        }
        public string FilePath
            {
            get => _filePath!;
            set { _filePath = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
