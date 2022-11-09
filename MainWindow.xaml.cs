using Microsoft.Win32;
using SlidingTile_LevelEditor.Class;
using SlidingTile_LevelEditor.Commands;
using SlidingTile_LevelEditor.Windows;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using System.Xml.Serialization;
using System.Xml;

namespace SlidingTile_LevelEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<FloorTile> _floorTiles = new List<FloorTile>();
        private List<Command> _commands = new List<Command>();
        private int _indexCommand = -1;
        private string _projectName, _projectPath;
        public MainWindow()
        {
            SetCultureInfo("en-EN");
            InitializeComponent();
            lvCommans.ItemsSource = _commands;
            _floorTiles.Add(new FloorTile() { PosX = 0, PosY = 0, Number = 1, Type = FloorTileType.Normal });
            lvFloorTiles.ItemsSource = _floorTiles;
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
            Button button = sender as Button;
            string cont = button.Content.ToString();
            new IncNormalCommand(_commands, _floorTiles, TEMP_DetectButtonLoc(cont), _indexCommand);
            PostAddCommandUpdate();
        }
        private Point TEMP_DetectButtonLoc(string content)
        {
            Point point = new Point(0, 0);
            if (content.Contains("Button"))
            {
                int openBrackIndex = content.IndexOf('[');
                int comaIndex = content.IndexOf(',');
                int closeBrackIndex = content.IndexOf(']');
                string strPosX = content.Substring(openBrackIndex + 1, comaIndex - openBrackIndex);
                string strPosY = content.Substring(comaIndex + 1, closeBrackIndex - comaIndex - 1);
                double posX = Convert.ToDouble(strPosX);
                double posY = Convert.ToDouble(strPosY);
                point = new Point(posX, posY);
            }
            return point;
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
            lvFloorTiles.Items.Refresh();
            tbFloorTileCount.Text = _floorTiles.Count.ToString();
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

        private void commandBinding_Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void commandBinding_Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Open function", "TODO", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void commandBinding_Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void commandBinding_Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_projectName == null || _projectName == "" || _projectPath == null || _projectPath == "")
            {
                SaveAs();
            }
            else
            {
                bool bRetAfterSave = SaveProject(_floorTiles, _projectName, _projectPath);
                if (bRetAfterSave == true)
                {
                    this.Title = GetProjectNameInLang() + " [" + _projectName + "]";
                    sbiProjectPath.Text = _projectPath;
                    MessageBox.Show("InfoSaveLevelMessage", "InfoSaveLevelTittle", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else
                {
                    MessageBox.Show("ErrorSaveLevelMessage", "ErrorSaveLevelTittle", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void commandBinding_SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveAs();
        }

        private void commandBinding_SaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private string GetProjectNameInLang()
        {
            return "Sliding Tile - PC Level Editor";
        }
        private void SaveAs()
        {
            SaveFileDialog textDialogSave = new SaveFileDialog();
            textDialogSave.Filter = "Game level | *.xml";
            bool? result = textDialogSave.ShowDialog();
            if (result.HasValue && result.Value)
            {
                bool bRetAfterSave = SaveProject(_floorTiles, System.IO.Path.GetFileNameWithoutExtension(textDialogSave.FileName), System.IO.Path.GetDirectoryName(textDialogSave.FileName));
                if (bRetAfterSave == true)
                {
                    this.Title = GetProjectNameInLang() + " [" + _projectName + "]";
                    sbiProjectPath.Text = _projectPath;
                    MessageBox.Show("InfoSaveLevelMessage", "InfoSaveLevelTittle", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else
                {
                    MessageBox.Show("ErrorSaveLevelMessage", "ErrorSaveLevelTittle", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private bool SaveProject(List<FloorTile> saveObject, string pName, string pPath)
        {
            bool correctSave = false;
            if (pName == "")
            {
                MessageBox.Show("ErrorIncorrectNameMessage", "ErrorIncorrectNameTittle", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (pPath == "")
            {
                MessageBox.Show("ErrorIncorrectPathMessage", "ErrorIncorrectPathTittle", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!Directory.Exists(pPath))
            {
                MessageBox.Show("ErrorPathNoExistMessage", "ErrorPathNoExistTittle", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    XmlSerializer serializer = new XmlSerializer(saveObject.GetType());
                    string fileLoc = System.IO.Path.Combine(pPath, pName + ".xml");
                    using (MemoryStream stream = new MemoryStream())
                    {
                        serializer.Serialize(stream, saveObject);
                        stream.Position = 0;
                        xmlDocument.Load(stream);
                        xmlDocument.Save(fileLoc);
                        stream.Close();
                    }
                    _projectName = pName;
                    _projectPath = pPath;
                    correctSave = true;
                }
                catch
                {
                    return correctSave;
                }
            }
            return correctSave;
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
