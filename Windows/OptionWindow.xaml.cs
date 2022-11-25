using SlidingTile_LevelEditor.Properties;
using System.Windows;
using System.Windows.Input;

namespace SlidingTile_LevelEditor.Windows
{
    /// <summary>
    /// Interaction logic for OptionWindow.xaml
    /// </summary>
    public partial class OptionWindow : Window
    {
        private bool afterInitial;
        public OptionWindow()
        {
            InitializeComponent();
            afterInitial = false;
            checkBox_UseNumpad.IsChecked = Settings.Default.navigationNumPad;
            afterInitial = true;
        }
        private void checkBox_UseNumpad_Common(object sender, RoutedEventArgs e)
        {
            if (afterInitial)
            {
                Settings.Default.navigationNumPad = checkBox_UseNumpad.IsChecked ?? default;
                Settings.Default.Save();
            }
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
