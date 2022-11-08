using SlidingTile_LevelEditor.Commands;
using SlidingTile_LevelEditor.Windows;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SlidingTile_LevelEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Command> _commands = new List<Command>();
        private int _indexCommand = -1;
        public MainWindow()
        {
            SetCultureInfo("en-EN");
            InitializeComponent();
            lvCommans.ItemsSource = _commands;
        }
        private static void SetCultureInfo(string cultureInfoToSet)
        {
            CultureInfo ci = new CultureInfo(cultureInfoToSet);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
        private void commandBinding_New_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void commandBinding_New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("New project command", "TODO", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void commandBinding_Exit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void commandBinding_Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
        private void commandBinding_AboutProgram_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void commandBinding_AboutProgram_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AboutProgram apWindow = new AboutProgram();
            apWindow.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new IncNormalCommand(_commands, new Point(0, 0), _indexCommand);
            PostAddCommandUpdate();
        }

        private void PostAddCommandUpdate()
        {
            _indexCommand = _commands.Count - 1;
            foreach (IncNormalCommand item in _commands)
            {
                item._isCurrentCommand = false;
            }
            ((IncNormalCommand)_commands[^1])._isCurrentCommand = true;
            lvCommans.Items.Refresh();
            tbCommandsCount.Text = _commands.Count.ToString();
            tbCommandsIndex.Text = _indexCommand.ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new IncNormalCommand(_commands, new Point(0, 1), _indexCommand);
            PostAddCommandUpdate();
        }

        private void commandBinding_Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _indexCommand >= 0;
        }

        private void commandBinding_Undo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _indexCommand--;
            PostUndoRedoCommandsList();
        }

        private void PostUndoRedoCommandsList()
        {
            for (int i = 0; i < _commands.Count; i++)
            {
                if (_indexCommand != i)
                {
                    ((IncNormalCommand)_commands[i])._isCurrentCommand = false;
                }
                else
                {
                    ((IncNormalCommand)_commands[i])._isCurrentCommand = true;
                }
            }
            tbCommandsIndex.Text = _indexCommand.ToString();
            lvCommans.Items.Refresh();
        }

        private void commandBinding_Redo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _indexCommand < (_commands.Count - 1);
        }

        private void commandBinding_Redo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _indexCommand++;
            PostUndoRedoCommandsList();
        }
    }
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Exit = new RoutedUICommand
            ("Exit", "Exit", typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.Q, ModifierKeys.Alt)
                }
            );
        public static readonly RoutedUICommand AboutProgram = new RoutedUICommand
            ("About Program", "About Program", typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F1, ModifierKeys.Alt)
                }
            );
    }
}
