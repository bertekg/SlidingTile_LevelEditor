using System.Windows;
using System.Windows.Input;

namespace SlidingTile_LevelEditor.Windows;

public partial class OptionsWindow : Window
{
    public OptionsWindow()
    {
        InitializeComponent();
    }
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return || e.Key == Key.Escape || e.Key == Key.Enter || e.Key == Key.Back)
        {
            Close();
        }
    }
    private void ButtonClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
