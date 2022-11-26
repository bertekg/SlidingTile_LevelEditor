using System.Windows;
using System.Windows.Input;

namespace SlidingTile_LevelEditor.Windows
{
    /// <summary>
    /// Interaction logic for OptionWindow.xaml
    /// </summary>
    public partial class OptionWindow : Window
    {
        public OptionWindow()
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
        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
