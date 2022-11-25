using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Reflection;

namespace SlidingTile_LevelEditor.Windows
{
    /// <summary>
    /// Interaction logic for AboutProgram.xaml
    /// </summary>
    public partial class AboutProgramWindow : Window
    {
        public AboutProgramWindow()
        {
            InitializeComponent();
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(assembly.Location);
            DateTime lastModified = fileInfo.LastWriteTime;
            tbVersionNo.Text = $"Version number: {Assembly.GetExecutingAssembly().GetName().Version}";
            tbReleaseDate.Text = $"Release date: {lastModified.Date:yyyy.MM.dd}";
            tbCopyright.Text = fvi.LegalCopyright;
        }
        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Escape || e.Key == Key.Enter || e.Key == Key.Back)
            {
                Close();
            }
        }
    }
}
