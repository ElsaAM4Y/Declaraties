using System.ComponentModel;
using System.Windows.Input;
using Declaraties.Services;

namespace Declaraties.ViewModels;

public class SettingsViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public event Action ThemeChanged;

    public IList<string> Themes { get; } = new List<string> { "Light", "Dark" };

    private string _selectedTheme;
    public string SelectedTheme
    {
        get => _selectedTheme;
        set
        {
            if (_selectedTheme == value)
                return;

            _selectedTheme = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTheme)));
        }
    }

    public ICommand SaveCommand { get; }

    public SettingsViewModel()
    {
        SelectedTheme = Preferences.Get("AppTheme", "Light");
        SaveCommand = new Command(Save);
    }

    private void Save()
    {
        ThemeService.SetTheme(SelectedTheme);
    }
}
