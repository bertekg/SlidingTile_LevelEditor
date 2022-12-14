using Microsoft.Win32;
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
        private List<Button> _viewButtons;
        private int _indexCommand = -1;
        private string _projectName = string.Empty, _projectPath = string.Empty;
        private bool _updateControl;
        public static EditMode _editMode = EditMode.None;
        LinearGradientBrush _borderSelected = new LinearGradientBrush(Colors.GreenYellow, Colors.Cyan, 45.0);
        SolidColorBrush _borderNotSelected = new SolidColorBrush(Colors.White);
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
            _updateControl = false;
            iudAreaViewDimMinX.Value = -1;
            iudAreaViewDimMaxX.Value = 1;
            iudAreaViewDimMinY.Value = -1;
            iudAreaViewDimMaxY.Value = 1;
            _updateControl = true;
            UpdateMainGridViewFull();
            UpdateEditBorders();
            _updateControl = true;
        }
        private void UpdateEditBorders()
        {
            borderNormalInc.BorderBrush = _borderNotSelected;
            borderNormalDec.BorderBrush = _borderNotSelected;
            borderIceInc.BorderBrush = _borderNotSelected;
            borderIceDec.BorderBrush = _borderNotSelected;
            borderSolid.BorderBrush = _borderNotSelected;
            borderPortalInc.BorderBrush = _borderNotSelected;
            borderPortalDec.BorderBrush = _borderNotSelected;
            borderSpringInc.BorderBrush = _borderNotSelected;
            borderSpringDec.BorderBrush = _borderNotSelected;
            borderSpringLeft.BorderBrush = _borderNotSelected;
            borderSpringRight.BorderBrush = _borderNotSelected;
            borderBombInitInc.BorderBrush = _borderNotSelected;
            borderBombInitDec.BorderBrush = _borderNotSelected;
            borderBombModInc.BorderBrush = _borderNotSelected;
            borderBombModDec.BorderBrush = _borderNotSelected;
            borderFinish.BorderBrush = _borderNotSelected;
            borderDelete.BorderBrush = _borderNotSelected;
            switch (_editMode)
            {
                case EditMode.None:
                    break;
                case EditMode.NormalInc:
                    borderNormalInc.BorderBrush = _borderSelected;
                    break;
                case EditMode.NormalDec:
                    borderNormalDec.BorderBrush = _borderSelected;
                    break;
                case EditMode.IceInc:
                    borderIceInc.BorderBrush = _borderSelected;
                    break;
                case EditMode.IceDec:
                    borderIceDec.BorderBrush = _borderSelected;
                    break;
                case EditMode.Static:
                    borderSolid.BorderBrush = _borderSelected;
                    break;
                case EditMode.PortalInc:
                    borderPortalInc.BorderBrush = _borderSelected;
                    break;
                case EditMode.PortalDec:
                    borderPortalDec.BorderBrush = _borderSelected;
                    break;
                case EditMode.SpringInc:
                    borderSpringInc.BorderBrush = _borderSelected;
                    break;
                case EditMode.SpringDec:
                    borderSpringDec.BorderBrush = _borderSelected;
                    break;
                case EditMode.SpringLeft:
                    borderSpringLeft.BorderBrush = _borderSelected;
                    break;
                case EditMode.SpringRight:
                    borderSpringRight.BorderBrush = _borderSelected;
                    break;
                case EditMode.FinishTile:
                    borderFinish.BorderBrush = _borderSelected;
                    break;
                case EditMode.DeleteTile:
                    borderDelete.BorderBrush = _borderSelected;
                    break;
                case EditMode.BombInitalInc:
                    borderBombInitInc.BorderBrush = _borderSelected;
                    break;
                case EditMode.BombInitialDec:
                    borderBombInitDec.BorderBrush = _borderSelected;
                    break;
                case EditMode.BombModInc:
                    borderBombModInc.BorderBrush = _borderSelected;
                    break;
                case EditMode.BombModDec:
                    borderBombModDec.BorderBrush = _borderSelected;
                    break;
                default:
                    break;
            }
        }
        private void UpdateMainGridViewFull()
        {
            gMainPlaceGrid.RowDefinitions.Clear();
            gMainPlaceGrid.ColumnDefinitions.Clear();

            int viewMinX = iudAreaViewDimMinX.Value ?? default;
            int viewMaxX = iudAreaViewDimMaxX.Value ?? default;
            int viewMinY = iudAreaViewDimMinY.Value ?? default;
            int viewMaxY = iudAreaViewDimMaxY.Value ?? default;
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
            UpdateMainGridViewOnlyButtons(viewMinX, viewMinY, iColNo, iRowNo);
        }
        private void UpdateMainGridViewOnlyButtons()
        {
            int viewMinX = iudAreaViewDimMinX.Value ?? default;
            int viewMaxX = iudAreaViewDimMaxX.Value ?? default;
            int viewMinY = iudAreaViewDimMinY.Value ?? default;
            int viewMaxY = iudAreaViewDimMaxY.Value ?? default;
            int iColNo = 1 + viewMaxX - viewMinX;
            int iRowNo = 1 + viewMaxY - viewMinY;

            UpdateMainGridViewOnlyButtons(viewMinX, viewMinY, iColNo, iRowNo);
        }
        private void UpdateMainGridViewOnlyButtons(int viewMinX, int viewMinY, int iColNo, int iRowNo)
        {
            gMainPlaceGrid.Children.Clear();
            for (int i = 0; i < iColNo; i++)
            {
                for (int j = 0; j < iRowNo; j++)
                {
                    _viewButtons = new List<Button>();
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
                                label.Foreground = new SolidColorBrush(Colors.Black);
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
                                lab.Foreground = new SolidColorBrush(Colors.Black);
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
                        else if (cResult.Type == FloorTileType.Static)
                        {
                            Grid gr = new Grid();
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"/Graphics/Tiles/Static.png", UriKind.Relative));
                            gr.Children.Add(img);
                            button.Content = gr;
                        }
                        else if (cResult.Type == FloorTileType.Portal)
                        {
                            Grid gr = new Grid();
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"/Graphics/Tiles/Portal.png", UriKind.Relative));
                            TextBlock label = new TextBlock();
                            label.HorizontalAlignment = HorizontalAlignment.Center;
                            label.VerticalAlignment = VerticalAlignment.Center;
                            label.TextAlignment = TextAlignment.Center;
                            label.Text = cResult.Portal.ToString();
                            label.FontSize = 30;
                            gr.Children.Add(img);
                            gr.Children.Add(label);
                            button.Content = gr;
                        }
                        else if (cResult.Type == FloorTileType.Spring)
                        {
                            Grid gr = new Grid();
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"/Graphics/Tiles/Spring.png", UriKind.Relative));
                            TextBlock label = new TextBlock();
                            label.HorizontalAlignment = HorizontalAlignment.Center;
                            label.VerticalAlignment = VerticalAlignment.Center;
                            label.TextAlignment = TextAlignment.Center;
                            switch (cResult.Spring)
                            {
                                case SpringDirection.Up:
                                    label.Text = $"⮝{cResult.Number}";
                                    break;
                                case SpringDirection.Left:
                                    label.Text = $"⮜{cResult.Number}";
                                    break;
                                case SpringDirection.Down:
                                    label.Text = $"⮟{cResult.Number}";

                                    break;
                                case SpringDirection.Right:
                                    label.Text = $"⮞{cResult.Number}";
                                    break;
                                default:
                                    break;
                            }
                            label.FontSize = 24;
                            gr.Children.Add(img);
                            gr.Children.Add(label);
                            button.Content = gr;
                        }
                        else if (cResult.Type == FloorTileType.BombInit)
                        {
                            Grid gr = new Grid();
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"/Graphics/Tiles/Bomb.png", UriKind.Relative));
                            TextBlock label = new TextBlock();
                            label.HorizontalAlignment = HorizontalAlignment.Center;
                            label.VerticalAlignment = VerticalAlignment.Center;
                            label.TextAlignment = TextAlignment.Center;
                            label.Text = $"Init\n{cResult.Bomb}";
                            label.FontSize = 18;
                            gr.Children.Add(img);
                            gr.Children.Add(label);
                            button.Content = gr;
                        }
                        else if (cResult.Type == FloorTileType.BombMod)
                        {
                            Grid gr = new Grid();
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"/Graphics/Tiles/BombMod.png", UriKind.Relative));
                            TextBlock label = new TextBlock();
                            label.HorizontalAlignment = HorizontalAlignment.Center;
                            label.VerticalAlignment = VerticalAlignment.Center;
                            label.TextAlignment = TextAlignment.Center;
                            label.Text = $"Mod\n{cResult.Bomb}";
                            label.FontSize = 18;
                            gr.Children.Add(img);
                            gr.Children.Add(label);
                            button.Content = gr;
                        }
                        else if (cResult.Type == FloorTileType.Finish)
                        {
                            Grid gr = new Grid();
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"/Graphics/Tiles/Finish.png", UriKind.Relative));
                            TextBlock lab = new TextBlock();
                            lab.HorizontalAlignment = HorizontalAlignment.Center;
                            lab.VerticalAlignment = VerticalAlignment.Center;
                            lab.TextAlignment = TextAlignment.Center;
                            lab.Text = "F";
                            lab.FontSize = 30;
                            lab.Foreground = new SolidColorBrush(Colors.Red);
                            gr.Children.Add(img);
                            gr.Children.Add(lab);
                            button.Content = gr;
                        }
                    }
                    button.MouseEnter += Button_MouseEnter;
                    gMainPlaceGrid.Children.Add(button);
                    _viewButtons.Add(button);
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
            AboutProgramWindow apWindow = new AboutProgramWindow();
            apWindow.ShowDialog();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = _commands.Count - 1; i > _indexCommand; i--)
            {
                _commands.RemoveAt(i);
            }
            Button? button = sender as Button;
            if (button.ToolTip == null) return; 
            Point point = (Point)button.ToolTip;
            if (_editMode == EditMode.None)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Please select edit mode.\nDo you want change to \"Normal Inc\" edit mode?",
                        "Missing selection of Edit Mode", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    _editMode = EditMode.NormalInc;
                    UpdateEditBorders();
                }
            }
            int floorTileIndex = FindFloor(point);
            switch (_editMode)
            {
                case EditMode.NormalInc:
                    new NormalIncCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                    PostCommandUpdate();
                    break;
                case EditMode.NormalDec:
                    if (floorTileIndex >= 0)
                    {
                        FloorTile floorTile = _floorTiles[floorTileIndex];
                        if (floorTile.Type == FloorTileType.Finish)
                        {
                            MessageBox.Show("Cannot decrees Finish floor Tile.", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (point == new Point(0, 0) && floorTile.Number < 2)
                        {
                            MessageBox.Show("Cannot decrees Start floor Tile below number 1.", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            new NormalDecCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                            PostCommandUpdate();
                        }
                    }
                    break;
                case EditMode.IceInc:
                    new IceIncCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                    PostCommandUpdate();
                    break;
                case EditMode.IceDec:
                    floorTileIndex = FindFloor(point);
                    if (floorTileIndex >= 0)
                    {
                        FloorTile floorTile = _floorTiles[floorTileIndex];
                        if (floorTile.Type == FloorTileType.Finish)
                        {
                            MessageBox.Show("Cannot decrees Finish floor Tile.", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (point == new Point(0, 0) && floorTile.Number < 2)
                        {
                            MessageBox.Show("Cannot decrees Start floor Tile below number 1.", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            new IceDecCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                            PostCommandUpdate();
                        }
                    }
                    break;
                case EditMode.Static:
                    floorTileIndex = FindFloor(point);
                    if (floorTileIndex < 0)
                    {
                        new StaticCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                        PostCommandUpdate();
                    }
                    else
                    {
                        FloorTile floorTile = _floorTiles[floorTileIndex];
                        if (floorTile.Type != FloorTileType.Static)
                        {
                            new StaticCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                            PostCommandUpdate();
                        }
                    }
                    break;
                case EditMode.PortalInc:
                    if (point == new Point(0, 0))
                    {
                        MessageBox.Show("Cannot change Start floor tile to Portal.", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        new PortalIncCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                        PostCommandUpdate();
                    }
                    break;
                case EditMode.PortalDec:
                    if (point == new Point(0, 0))
                    {
                        MessageBox.Show("Cannot change Start floor tile to Portal.", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        floorTileIndex = FindFloor(point);
                        if (floorTileIndex >= 0)
                        {
                            FloorTile floorTile = _floorTiles[floorTileIndex];
                            if (floorTile.Type == FloorTileType.Finish)
                            {
                                MessageBox.Show("Cannot decrees Finish floor Tile.", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else
                            {
                                new PortalDecCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                                PostCommandUpdate();
                            }
                        }
                    }
                    break;
                case EditMode.SpringInc:
                    if (point == new Point(0, 0))
                    {
                        MessageBox.Show("Cannot change Start floor tile to Spring.", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        new SpringIncCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                        PostCommandUpdate();
                    }
                    break;
                case EditMode.SpringDec:
                    if (point == new Point(0, 0))
                    {
                        MessageBox.Show("Cannot change Start floor tile to Spring.", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        floorTileIndex = FindFloor(point);
                        if (floorTileIndex >= 0)
                        {
                            FloorTile floorTile = _floorTiles[floorTileIndex];
                            if (floorTile.Type == FloorTileType.Finish)
                            {
                                MessageBox.Show("Cannot decrees Finish floor Tile.", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else
                            {
                                new SpringDecCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                                PostCommandUpdate();
                            }
                        }
                    }
                    break;
                case EditMode.SpringLeft:
                    if (point == new Point(0, 0))
                    {
                        MessageBox.Show("Cannot change Start floor tile to Spring.", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        floorTileIndex = FindFloor(point);
                        if (floorTileIndex >= 0)
                        {
                            FloorTile floorTile = _floorTiles[floorTileIndex];
                            if (floorTile.Type == FloorTileType.Spring)
                            {
                                new SpringLeftCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                                PostCommandUpdate();
                            }
                            else
                            {
                                MessageBox.Show("Cannot change direction of this floor tile. Only existing Spring tile can be modified", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    break;
                case EditMode.SpringRight:
                    if (point == new Point(0, 0))
                    {
                        MessageBox.Show("Cannot change Start floor tile to Spring.", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        floorTileIndex = FindFloor(point);
                        if (floorTileIndex >= 0)
                        {
                            FloorTile floorTile = _floorTiles[floorTileIndex];
                            if (floorTile.Type == FloorTileType.Spring)
                            {
                                new SpringRightCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                                PostCommandUpdate();
                            }
                            else
                            {
                                MessageBox.Show("Cannot change direction of this floor tile. Only existing Spring tile can be modified", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    break;
                case EditMode.BombInitalInc:
                    if (point == new Point(0, 0))
                    {
                        MessageBox.Show("Cannot change Start floor tile to Bomb Init.", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        new BombInitIncCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                        PostCommandUpdate();
                    }
                    break;
                case EditMode.BombInitialDec:
                    if (point == new Point(0, 0))
                    {
                        MessageBox.Show("Cannot change Start floor tile to Bomb Init.", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (floorTileIndex >= 0)
                    {
                        new BombInitDecCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                        PostCommandUpdate();
                    }
                    break;
                case EditMode.BombModInc:
                    if (point == new Point(0, 0))
                    {
                        MessageBox.Show("Cannot change Start floor tile to Bomb Mod.", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        new BombModIncCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                        PostCommandUpdate();
                    }
                    break;
                case EditMode.BombModDec:
                    if (point == new Point(0, 0))
                    {
                        MessageBox.Show("Cannot change Start floor tile to Bomb Mod.", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        new BombModDecCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                        PostCommandUpdate();
                    }
                    break;
                case EditMode.FinishTile:
                    if (point == new Point(0, 0))
                    {
                        MessageBox.Show("Cannot change Start floor tile to Finish.", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        floorTileIndex = FindFloor(point);
                        
                        if (floorTileIndex < 0)
                        {
                            new FinishCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                            PostCommandUpdate();
                        }
                        else
                        {
                            FloorTile floorTile = _floorTiles[floorTileIndex];
                            if (floorTile.Type != FloorTileType.Finish)
                            {
                                new FinishCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                                PostCommandUpdate();
                            }
                        }
                    }
                    break;
                case EditMode.DeleteTile:
                    floorTileIndex = FindFloor(point);
                    if (floorTileIndex >= 0)
                    {
                        if (point == new Point(0, 0))
                        {
                            MessageBox.Show("Cannot remove Start floor tile.", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            new RemoveCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                            PostCommandUpdate();
                        }
                    }
                    break;
            }
            UpdateMainGridViewOnlyButtons();
        }
        private int FindFloor(Point point)
        {
            return _floorTiles.FindIndex(item => item.PosX == point.X && item.PosY == point.Y);
        }
        private void UpdateButton(Button button, Point point)
        {
            FloorTile floorTile = _floorTiles.Find(tile => tile.PosX == point.X && tile.PosY == point.Y);
            if (floorTile != null)
            {
                if (floorTile.Type == FloorTileType.Normal)
                {
                    Grid gr = new Grid();
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(@"/Graphics/Tiles/Normal.png", UriKind.Relative));
                    TextBlock label = new TextBlock();
                    label.HorizontalAlignment = HorizontalAlignment.Center;
                    label.VerticalAlignment = VerticalAlignment.Center;
                    label.TextAlignment = TextAlignment.Center;
                    if (point == new Point(0, 0))
                    {
                        label.Text = "Start\n" + floorTile.Number.ToString();
                        label.FontSize = 18;
                        label.Foreground = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        label.Text = floorTile.Number.ToString();
                        label.FontSize = 30;
                    }
                    gr.Children.Add(img);
                    gr.Children.Add(label);
                    button.Content = gr;
                }
                else if (floorTile.Type == FloorTileType.Ice)
                {
                    Grid gr = new Grid();
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(@"/Graphics/Tiles/Ice.png", UriKind.Relative));
                    TextBlock lab = new TextBlock();
                    lab.HorizontalAlignment = HorizontalAlignment.Center;
                    lab.VerticalAlignment = VerticalAlignment.Center;
                    lab.TextAlignment = TextAlignment.Center;
                    if (point == new Point(0, 0))
                    {
                        lab.Text = "Start\n" + floorTile.Number.ToString();
                        lab.FontSize = 18;
                        lab.Foreground = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        lab.Text = floorTile.Number.ToString();
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
        }
        private void PostCommandUpdate()
        {
            _indexCommand = _commands.Count - 1;
            lvCommans.Items.Refresh();
            tbCommandsCount.Text = _commands.Count.ToString();
            tbCommandsIndex.Text = _indexCommand.ToString();
            lvFloorTiles.Items.Refresh();
            tbFloorTileCount.Text = _floorTiles.Count.ToString();
            CalcLevelSize();
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
            UpdateMainGridViewOnlyButtons();
        }

        private void PostUndoRedoCommandsList()
        {
            tbCommandsIndex.Text = _indexCommand.ToString();
            lvCommans.Items.Refresh();
            lvFloorTiles.Items.Refresh();
            tbFloorTileCount.Text = _floorTiles.Count.ToString();
            CalcLevelSize();
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
            UpdateMainGridViewOnlyButtons();
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
                    tbFloorTileCount.Text = _floorTiles?.Count.ToString();
                    _commands.Clear();
                    lvCommans.Items.Refresh();
                    _indexCommand = _commands.Count - 1;
                    tbCommandsCount.Text = _commands.Count.ToString();
                    tbCommandsIndex.Text = _indexCommand.ToString();
                    CalcLevelSize();
                    AdjustViewProject();
                    MessageBox.Show("Game level was opened correct!", "New level confirmation", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                catch (Exception)
                {
                    MessageBox.Show("Game level cannot be opened!\nFile is corrupted.", "Open level error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void commandBinding_Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void commandBinding_Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!ShouldPerformSaveAfterCheck()) return;
            
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
                    MessageBox.Show("Game level was saved correct!", "Save level confirmation", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else
                {
                    MessageBox.Show("Game level was not saved correct!\nSome unexpected error occur.", "Error during save level", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void commandBinding_SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!ShouldPerformSaveAfterCheck()) return;

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
                    MessageBox.Show("Game level was not saved correct!\nSome unexpected error occur.", "Error during save level", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void AssigneProjectNameAndPath()
        {
            this.Title = GetProjectNameInLang() + " [" + _projectName + "]";
            sbiProjectPath.Text = _projectPath;
            MessageBox.Show("Game level was saved correct!", "Save level confirmation", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        private void iudAreaViewDim_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (_updateControl)
                UpdateMainGridViewFull();
        }
        private void IncViewRange(int minX, int maxX, int minY, int maxY)
        {
            _updateControl = false;
            iudAreaViewDimMinX.Value += minX;
            iudAreaViewDimMaxX.Value += maxX;
            iudAreaViewDimMinY.Value += minY;
            iudAreaViewDimMaxY.Value += maxY;
            _updateControl = true;
            UpdateMainGridViewFull();
        }
        private void commandBinding_MoveViewCommon_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void commandBinding_MoveViewUp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(0, 0, -1, -1);
        }
        private void commandBinding_MoveViewDown_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(0, 0, 1, 1);
        }
        private void commandBinding_MoveViewLeft_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(1, 1, 0, 0);
        }
        private void commandBinding_MoveViewRight_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(-1, -1, 0, 0);
        }
        private void commandBinding_MoveViewLeftUp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(1, 1, -1, -1);
        }
        private void commandBinding_MoveViewRightUp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(-1, -1, -1, -1);
        }
        private void commandBinding_MoveViewLeftDown_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(1, 1, 1, 1);
        }
        private void commandBinding_MoveViewRightDown_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(-1, -1, 1, 1);
        }
        private void commandBinding_FullLevelView_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AdjustViewProject();
        }
        private void AdjustViewProject()
        {
            _updateControl = false;
            iudAreaViewDimMinX.Value = _floorTiles.Min(item => item.PosX);
            iudAreaViewDimMaxX.Value = _floorTiles.Max(item => item.PosX);
            iudAreaViewDimMinY.Value = _floorTiles.Min(item => item.PosY);
            iudAreaViewDimMaxY.Value = _floorTiles.Max(item => item.PosY);
            _updateControl = true;
            UpdateMainGridViewFull();
        }
        private void commandBinding_ZoomOutView_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(-1, 1, -1, 1);
        }
        private void commandBinding_ZoomInView_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CheckPossibleZoomIn();
        }
        private void commandBinding_ZoomInView_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IncViewRange(1, -1, 1, -1);
        }
        private void vPlaceAllCells_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                ZoomIn();
            }
            else if (e.Delta < 0)
                IncViewRange(-1, 1, -1, 1);
        }
        private void ZoomIn()
        {
            if (CheckPossibleZoomIn())
            {
                IncViewRange(1, -1, 1, -1);
            }
        }
        private bool CheckPossibleZoomIn()
        {
            bool isPossible = false;
            int xDelta = (iudAreaViewDimMaxX.Value ?? default) - (iudAreaViewDimMinX.Value ?? default);
            int yDelta = (iudAreaViewDimMaxY.Value ?? default) - (iudAreaViewDimMinY.Value ?? default);
            if (xDelta >= 2 && yDelta >= 2)
            {
                isPossible = true;
            }
            return isPossible;
        }
        private void commandBinding_EditModeCommon_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void commandBinding_EditModeNormalInc_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _editMode = EditMode.NormalInc;
            UpdateEditBorders();
        }
        private void commandBinding_EditModeNormalDec_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _editMode = EditMode.NormalDec;
            UpdateEditBorders();
        }
        private void commandBinding_EditModeIceInc_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _editMode = EditMode.IceInc;
            UpdateEditBorders();
        }
        private void commandBinding_EditModeIceDec_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _editMode = EditMode.IceDec;
            UpdateEditBorders();
        }
        private void commandBinding_EditModeStatic_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _editMode = EditMode.Static;
            UpdateEditBorders();
        }
        private void commandBinding_EditModePortalInc_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _editMode = EditMode.PortalInc;
            UpdateEditBorders();
        }
        private void commandBinding_EditModePortalDec_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _editMode = EditMode.PortalDec;
            UpdateEditBorders();
        }
        private void commandBinding_EditModeSpringInc_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _editMode = EditMode.SpringInc;
            UpdateEditBorders();
        }
        private void commandBinding_EditModeSpringDec_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _editMode = EditMode.SpringDec;
            UpdateEditBorders();
        }
        private void commandBinding_EditModeSpringLeft_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _editMode = EditMode.SpringLeft;
            UpdateEditBorders();
        }
        private void commandBinding_EditModeSpringRight_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _editMode = EditMode.SpringRight;
            UpdateEditBorders();
        }
        private void commandBinding_EditModeBombInitialInc_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _editMode = EditMode.BombInitalInc;
            UpdateEditBorders();
        }
        private void commandBinding_EditModeBombInitialDec_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _editMode = EditMode.BombInitialDec;
            UpdateEditBorders();
        }
        private void commandBinding_EditModeBombModInc_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _editMode = EditMode.BombModInc;
            UpdateEditBorders();
        }
        private void commandBinding_EditModeBombModDec_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _editMode = EditMode.BombModDec;
            UpdateEditBorders();
        }
        private void commandBinding_EditModeFinish_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _editMode = EditMode.FinishTile;
            UpdateEditBorders();
        }
        private void commandBinding_EditModeDeleteTile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _editMode = EditMode.DeleteTile;
            UpdateEditBorders();
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button bTemo = (Button)sender;
            tbCurrentGrid.Text = bTemo.ToolTip.ToString();
        }
        private void commandBinding_DeleteAllTiles_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_floorTiles != null)
            {
                if (_floorTiles.Count > 1)
                {
                    e.CanExecute = true;
                } 
            }
        }
        private void commandBinding_DeleteAllTiles_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            new ClearAllCommand(_commands, _floorTiles, _indexCommand + 1);
            PostCommandUpdate();
            UpdateMainGridViewOnlyButtons();
        }
        private void commandBinding_Check_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void commandBinding_Check_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CheckLevelWindow checkLevelWindow = new CheckLevelWindow(_floorTiles);
            checkLevelWindow.ShowDialog();
        }
        private bool ShouldPerformSaveAfterCheck()
        {
            bool retrunVal;
            CheckLevelWindow checkLevelWindow = new CheckLevelWindow(_floorTiles);
            int failsNumber = checkLevelWindow.GetNumberFailedTests();
            if (failsNumber > 0)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("In current project detected some problems.\nNumber of problem: " + failsNumber.ToString() + "\nDo you want save anymore?",
                    "Project contain error", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    retrunVal = true;
                }
                else if (messageBoxResult == MessageBoxResult.No)
                {
                    retrunVal = false;
                    checkLevelWindow.ShowDialog();
                }
                else
                {
                    retrunVal = false;
                }
            }
            else
            {
                retrunVal = true;
            }
            checkLevelWindow.Close();
            return retrunVal;
        }
        private void commandBinding_Options_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void commandBinding_Options_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OptionsWindow optionsWindow = new OptionsWindow();
            optionsWindow.ShowDialog();
        }
        private bool SaveProject(List<FloorTile> saveObject, string pName, string pPath)
        {
            bool correctSave = false;
            if (pName == "")
            {
                MessageBox.Show("Please select a correct level name.", "Incorrect level name", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (pPath == "")
            {
                MessageBox.Show("Please select a correct level path.", "Incorrect level path", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!Directory.Exists(pPath))
            {
                MessageBox.Show("The level path does not exist.", "No exist level path", MessageBoxButton.OK, MessageBoxImage.Error);
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
    public enum EditMode {None, NormalInc, NormalDec, IceInc, IceDec, Static, PortalInc, PortalDec,
        SpringInc, SpringDec, SpringLeft, SpringRight,
        BombInitalInc, BombInitialDec, BombModInc, BombModDec, FinishTile, DeleteTile}
}
