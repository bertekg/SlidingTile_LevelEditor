﻿using Microsoft.Win32;
using SlidingTile_LevelEditor.Class;
using SlidingTile_LevelEditor.Commands;
using SlidingTile_LevelEditor.Windows;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using System.Xml;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace SlidingTile_LevelEditor
{
    public partial class MainWindow : Window
    {
        private List<FloorTile> _floorTiles = new List<FloorTile>();
        private List<Command> _commands = new List<Command>();
        private int _indexCommand = -1;
        private string _projectName = string.Empty, _projectPath = string.Empty;
        private bool _updateControl;
        public MainWindow()
        {
            SetCultureInfo("en-EN");
            InitializeComponent();
            lvFloorTiles.ItemsSource = _floorTiles;
            lvCommans.ItemsSource = _commands;
            NewLevel();
        }
        private void NewLevel()
        {
            _updateControl = false;
            _floorTiles.Clear();
            _floorTiles.Add(new FloorTile() { PosX = 0, PosY = 0, Number = 1, Type = FloorTileType.Normal });
            lvFloorTiles.Items.Refresh();
            tbFloorTileCount.Text = _floorTiles.Count.ToString();
            _commands.Clear();
            lvCommans.Items.Refresh();
            _indexCommand = _commands.Count - 1;
            tbCommandsCount.Text = _commands.Count.ToString();
            tbCommandsIndex.Text = _indexCommand.ToString();
            Title = GetProjectNameInLang() + " (Empty Project)";
            CalcLevelSize();
            UpdateMainGridView();
            _updateControl = true;
        }
        private void UpdateMainGridView()
        {
            gMainPlaceGrid.Children.Clear();
            gMainPlaceGrid.RowDefinitions.Clear();
            gMainPlaceGrid.ColumnDefinitions.Clear();

            int viewMinX = iudAreaViewDimMinX.Value.Value;
            int viewMaxX = iudAreaViewDimMaxX.Value.Value;
            int viewMinY = iudAreaViewDimMinY.Value.Value;
            int viewMaxY = iudAreaViewDimMaxY.Value.Value;
            int iColNo = 1 + viewMaxX - viewMinX;
            int iRowNo = 1 + viewMaxY - viewMinY;

            for (int i = 0; i < iColNo; i++)
            {
                gMainPlaceGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < iRowNo; i++)
            {
                gMainPlaceGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < iColNo; i++)
            {
                for (int j = 0; j < iRowNo; j++)
                {
                    Button button = new Button();
                    Point newPoint = new Point(viewMinX + i, viewMinY + j);
                    button.Width = 60; button.Height = 60; button.ToolTip = newPoint;
                    Grid.SetColumn(button, i);
                    Grid.SetRow(button, iRowNo - j - 1);
                    button.Click += Button_Click;
                    FloorTile? cResult = _floorTiles.Find(x => x.PosX == newPoint.X && x.PosY == newPoint.Y);
                    if (cResult != null)
                    {
                        if (cResult.Type == FloorTileType.Normal)
                        {
                            Grid gr = new Grid();
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"/Graphics/Tiles/Normal.png", UriKind.Relative));
                            TextBlock label = new TextBlock();
                            label.HorizontalAlignment = HorizontalAlignment.Center;
                            label.VerticalAlignment = VerticalAlignment.Center;
                            label.TextAlignment = TextAlignment.Center;
                            if (newPoint == new Point(0, 0))
                            {
                                label.Text = "Start\n" + cResult.Number.ToString();
                                label.FontSize = 18;
                                label.Foreground = new SolidColorBrush(Colors.Green);
                            }
                            else
                            {
                                label.Text = cResult.Number.ToString();
                                label.FontSize = 30;
                            }
                            gr.Children.Add(img);
                            gr.Children.Add(label);
                            button.Content = gr;
                        }
                        else if (cResult.Type == FloorTileType.Ice)
                        {
                            Grid gr = new Grid();
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"/Graphics/Tiles/Ice.png", UriKind.Relative));
                            TextBlock lab = new TextBlock();
                            lab.HorizontalAlignment = HorizontalAlignment.Center;
                            lab.VerticalAlignment = VerticalAlignment.Center;
                            lab.TextAlignment = TextAlignment.Center;
                            if (newPoint == new Point(0, 0))
                            {
                                lab.Text = "Start\n" + cResult.Number.ToString();
                                lab.FontSize = 18;
                                lab.Foreground = new SolidColorBrush(Colors.Green);
                            }
                            else
                            {
                                lab.Text = cResult.Number.ToString();
                                lab.FontSize = 30;
                            }
                            gr.Children.Add(img);
                            gr.Children.Add(lab);
                            button.Content = gr;
                        }
                        else
                        {
                            Grid gr = new Grid();
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"/Graphics/Tiles/Normal.png", UriKind.Relative));
                            TextBlock lab = new TextBlock();
                            lab.HorizontalAlignment = HorizontalAlignment.Center;
                            lab.VerticalAlignment = VerticalAlignment.Center;
                            lab.TextAlignment = TextAlignment.Center;
                            lab.Text = "FIN";
                            lab.FontSize = 18;
                            lab.Foreground = new SolidColorBrush(Colors.Red);
                            gr.Children.Add(img);
                            gr.Children.Add(lab);
                            button.Content = gr;
                        }
                    }
                    //bTemp.MouseEnter += Button_MouseEnter;
                    gMainPlaceGrid.Children.Add(button);
                }
            }
        }
        private void CalcLevelSize()
        {
            tbInfoMinX.Text = _floorTiles.Min(c => c.PosX).ToString();
            tbInfoMaxX.Text = _floorTiles.Max(c => c.PosX).ToString();
            tbInfoMinY.Text = _floorTiles.Min(c => c.PosY).ToString();
            tbInfoMaxY.Text = _floorTiles.Max(c => c.PosY).ToString();
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
            NewLevel();
            MessageBox.Show("New project created!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
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
            for (int i = _commands.Count - 1; i > _indexCommand; i--)
            {
                _commands.RemoveAt(i);
            }
            Button? button = sender as Button;
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
            _commands[_indexCommand].Undo();
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
            lvFloorTiles.Items.Refresh();
        }

        private void commandBinding_Redo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _indexCommand < (_commands.Count - 1);
        }

        private void commandBinding_Redo_Executed(object sender, ExecutedRoutedEventArgs e)
        {            
            _indexCommand++;
            _commands[_indexCommand].Redo();
            PostUndoRedoCommandsList();
        }

        private void commandBinding_Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void commandBinding_Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openDialogOpen = new OpenFileDialog();
            openDialogOpen.Filter = "Game Level | *.xml";
            bool? result = openDialogOpen.ShowDialog();
            if (result.HasValue && result.Value)
            {
                _floorTiles.Clear();
                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(openDialogOpen.FileName);
                    string xmlString = xmlDocument.OuterXml;
                    using (StringReader read = new StringReader(xmlString))
                    {
                        Type outType = typeof(List<FloorTile>);

                        XmlSerializer serializer = new XmlSerializer(outType);
                        using (XmlReader reader = new XmlTextReader(read))
                        {
                            _floorTiles = (List<FloorTile>)serializer.Deserialize(reader);
                            lvFloorTiles.ItemsSource = _floorTiles;
                            reader.Close();
                        }
                        read.Close();
                    }
                    _projectName = Path.GetFileNameWithoutExtension(openDialogOpen.FileName);
                    _projectPath = Path.GetDirectoryName(openDialogOpen.FileName);
                    Title = GetProjectNameInLang() + " [" + _projectName + "]";
                    lvFloorTiles.Items.Refresh();
                    tbFloorTileCount.Text = _floorTiles.Count.ToString();
                    _commands.Clear();
                    lvCommans.Items.Refresh();
                    _indexCommand = _commands.Count - 1;
                    tbCommandsCount.Text = _commands.Count.ToString();
                    tbCommandsIndex.Text = _indexCommand.ToString();
                    CalcLevelSize();
                    UpdateMainGridView();
                    MessageBox.Show("InfoOpenLevelConfirmationMessage", "InfoOpenLevelConfirmationTittle", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                catch (Exception)
                {
                    MessageBox.Show("ErrorOpenLevelMessage", "ErrorOpenLevelTittle", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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
                bool bRetAfterSave = SaveProject(_floorTiles, Path.GetFileNameWithoutExtension(textDialogSave.FileName), Path.GetDirectoryName(textDialogSave.FileName));
                if (bRetAfterSave == true)
                {
                    AssigneProjectNameAndPath();
                }
                else
                {
                    MessageBox.Show("ErrorSaveLevelMessage", "ErrorSaveLevelTittle", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void AssigneProjectNameAndPath()
        {
            this.Title = GetProjectNameInLang() + " [" + _projectName + "]";
            sbiProjectPath.Text = _projectPath;
            MessageBox.Show("InfoSaveLevelMessage", "InfoSaveLevelTittle", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        private void iudAreaViewDim_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (_updateControl)
                UpdateMainGridView();
        }
        private void IncViewRange(int minX, int maxX, int minY, int maxY)
        {
            _updateControl = false;
            iudAreaViewDimMinX.Value += minX;
            iudAreaViewDimMaxX.Value += maxX;
            iudAreaViewDimMinY.Value += minY;
            iudAreaViewDimMaxY.Value += maxY;
            _updateControl = true;
            UpdateMainGridView();
        }
        private void commandBinding_MoveViewUp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void commandBinding_MoveViewUp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(0, 0, -1, -1);
        }
        private void commandBinding_MoveViewDown_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void commandBinding_MoveViewDown_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(0, 0, 1, 1);

        }
        private void commandBinding_MoveViewLeft_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void commandBinding_MoveViewLeft_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(1, 1, 0, 0);
        }
        private void commandBinding_MoveViewRight_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void commandBinding_MoveViewRight_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(-1, -1, 0, 0);
        }
        private void commandBinding_MoveViewLeftUp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void commandBinding_MoveViewLeftUp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(1, 1, -1, -1);
        }
        private void commandBinding_MoveViewRightUp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void commandBinding_MoveViewRightUp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(-1, -1, -1, -1);
        }
        private void commandBinding_MoveViewLeftDown_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void commandBinding_MoveViewLeftDown_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(1, 1, 1, 1);
        }
        private void commandBinding_MoveViewRightDown_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void commandBinding_MoveViewRightDown_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(-1, -1, 1, 1);
        }
        private void commandBinding_AdjustViewProject_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void commandBinding_AdjustViewProject_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _updateControl = false;
            iudAreaViewDimMinX.Value = _floorTiles.Min(item => item.PosX);
            iudAreaViewDimMaxX.Value = _floorTiles.Max(item => item.PosX);
            iudAreaViewDimMinY.Value = _floorTiles.Min(item => item.PosY);
            iudAreaViewDimMaxY.Value = _floorTiles.Max(item => item.PosY);
            _updateControl = true;
            UpdateMainGridView();
        }
        private void commandBinding_ZoomOutView_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void commandBinding_ZoomOutView_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(-1, 1, -1, 1);
        }
        private void commandBinding_ZoomInView_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void commandBinding_ZoomInView_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(1, -1, 1, -1);
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
                    string fileLoc = Path.Combine(pPath, pName + ".xml");
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
}
