using System.ComponentModel;
using System.Windows.Input;

namespace Declaraties.ViewModels;

public class SettingsViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public IList<string> Themes { get; } = new List<string> { "Light", "Dark" };

    string _selectedTheme;
    public string SelectedTheme
    {
        get => _selectedTheme;
        set
        {
            if (_selectedTheme == value) return;
            _selectedTheme = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTheme)));
        }
    }

    public ICommand SaveCommand { get; }

    public SettingsViewModel()
    {
        var saved = Preferences.Get("AppTheme", "Light");
        SelectedTheme = saved;
        SaveCommand = new Command(Save);
    }

    void Save()
    {
        if (Application.Current is App app)
            app.ApplyTheme(SelectedTheme);
    }
}

