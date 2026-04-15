using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Declaraties.ViewModels;

public class SettingsViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public List<string> Themes { get; } = new() { "Light", "Dark" };

    private string _selectedTheme;
    public string SelectedTheme
    {
        get => _selectedTheme;
        set
        {
            if (_selectedTheme != value)
            {
                _selectedTheme = value;
                OnPropertyChanged();

                // ⭐ APPLY THE THEME IMMEDIATELY
                (Application.Current as App)?.ApplyTheme(value);
            }
        }
    }

    public ICommand SaveCommand { get; }

    public SettingsViewModel()
    {
        // Load saved theme
        SelectedTheme = Preferences.Get("AppTheme", "Light");

        SaveCommand = new Command(() =>
        {
            (Application.Current as App)?.ApplyTheme(SelectedTheme);
        });
    }
}
