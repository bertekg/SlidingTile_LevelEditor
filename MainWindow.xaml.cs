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

namespace SlidingTile_LevelEditor;

public partial class MainWindow : Window
{
    private const EditMode EDIT_MODE_NONE = EditMode.None;
    private List<FloorTile> _floorTiles = new();
    private readonly List<Command> _commands = new();
    private List<Button>? _viewButtons;
    private int _indexCommand = -1;
    private string? _projectName = string.Empty, _projectPath = string.Empty;
    private bool _updateControl;
    public static EditMode _editMode = EDIT_MODE_NONE;
    readonly LinearGradientBrush _borderSelected = new(Colors.GreenYellow, Colors.Cyan, 45.0);
    readonly SolidColorBrush _borderNotSelected = new(Colors.White);
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
                Button button = new();
                Point newPoint = new(viewMinX + i, viewMinY + j);
                button.Width = 60; button.Height = 60; button.ToolTip = newPoint;
                Grid.SetColumn(button, i);
                Grid.SetRow(button, iRowNo - j - 1);
                button.Click += Button_Click;
                FloorTile? cResult = _floorTiles.Find(x => x.PosX == newPoint.X && x.PosY == newPoint.Y);
                if (cResult != null)
                {
                    if (cResult.Type == FloorTileType.Normal)
                    {
                        Grid gr = new();
                        Image img = new()
                        {
                            Source = new BitmapImage(new Uri(@"/Graphics/Tiles/Normal.png", UriKind.Relative))
                        };
                        TextBlock textBlovk = new()
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            TextAlignment = TextAlignment.Center
                        };
                        if (newPoint == new Point(0, 0))
                        {
                            textBlovk.Text = "Start\n" + cResult.Number.ToString();
                            textBlovk.FontSize = 18;
                            textBlovk.Foreground = new SolidColorBrush(Colors.Black);
                        }
                        else
                        {
                            textBlovk.Text = cResult.Number.ToString();
                            textBlovk.FontSize = 30;
                        }
                        gr.Children.Add(img);
                        gr.Children.Add(textBlovk);
                        button.Content = gr;
                    }
                    else if (cResult.Type == FloorTileType.Ice)
                    {
                        Grid grid = new();
                        Image image = new()
                        {
                            Source = new BitmapImage(new Uri(@"/Graphics/Tiles/Ice.png", UriKind.Relative))
                        };
                        TextBlock textBlovk = new()
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            TextAlignment = TextAlignment.Center
                        };
                        if (newPoint == new Point(0, 0))
                        {
                            textBlovk.Text = "Start\n" + cResult.Number.ToString();
                            textBlovk.FontSize = 18;
                            textBlovk.Foreground = new SolidColorBrush(Colors.Black);
                        }
                        else
                        {
                            textBlovk.Text = cResult.Number.ToString();
                            textBlovk.FontSize = 30;
                        }
                        grid.Children.Add(image);
                        grid.Children.Add(textBlovk);
                        button.Content = grid;
                    }
                    else if (cResult.Type == FloorTileType.Static)
                    {
                        Grid grid = new();
                        Image image = new()
                        {
                            Source = new BitmapImage(new Uri(@"/Graphics/Tiles/Static.png", UriKind.Relative))
                        };
                        grid.Children.Add(image);
                        button.Content = grid;
                    }
                    else if (cResult.Type == FloorTileType.Portal)
                    {
                        Grid grid = new();
                        Image image = new()
                        {
                            Source = new BitmapImage(new Uri(@"/Graphics/Tiles/Portal.png", UriKind.Relative))
                        };
                        TextBlock textBlock = new()
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            TextAlignment = TextAlignment.Center,
                            Text = cResult.Portal.ToString(),
                            FontSize = 30
                        };
                        grid.Children.Add(image);
                        grid.Children.Add(textBlock);
                        button.Content = grid;
                    }
                    else if (cResult.Type == FloorTileType.Spring)
                    {
                        Grid grid = new();
                        Image image = new()
                        {
                            Source = new BitmapImage(new Uri(@"/Graphics/Tiles/Spring.png", UriKind.Relative))
                        };
                        TextBlock textBlock = new()
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            TextAlignment = TextAlignment.Center
                        };
                        switch (cResult.Spring)
                        {
                            case SpringDirection.Up:
                                textBlock.Text = $"⮝{cResult.Number}";
                                break;
                            case SpringDirection.Left:
                                textBlock.Text = $"⮜{cResult.Number}";
                                break;
                            case SpringDirection.Down:
                                textBlock.Text = $"⮟{cResult.Number}";

                                break;
                            case SpringDirection.Right:
                                textBlock.Text = $"⮞{cResult.Number}";
                                break;
                            default:
                                break;
                        }
                        textBlock.FontSize = 24;
                        grid.Children.Add(image);
                        grid.Children.Add(textBlock);
                        button.Content = grid;
                    }
                    else if (cResult.Type == FloorTileType.BombInit)
                    {
                        Grid grid = new();
                        Image image = new()
                        {
                            Source = new BitmapImage(new Uri(@"/Graphics/Tiles/Bomb.png", UriKind.Relative))
                        };
                        TextBlock textBlock = new()
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            TextAlignment = TextAlignment.Center,
                            Text = $"Init\n{cResult.Bomb}",
                            FontSize = 18
                        };
                        grid.Children.Add(image);
                        grid.Children.Add(textBlock);
                        button.Content = grid;
                    }
                    else if (cResult.Type == FloorTileType.BombMod)
                    {
                        Grid grid = new();
                        Image image = new()
                        {
                            Source = new BitmapImage(new Uri(@"/Graphics/Tiles/BombMod.png", UriKind.Relative))
                        };
                        TextBlock textBlock = new()
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            TextAlignment = TextAlignment.Center,
                            Text = $"Mod\n{cResult.Bomb}",
                            FontSize = 18
                        };
                        grid.Children.Add(image);
                        grid.Children.Add(textBlock);
                        button.Content = grid;
                    }
                    else if (cResult.Type == FloorTileType.Finish)
                    {
                        Grid grid = new();
                        Image image = new()
                        {
                            Source = new BitmapImage(new Uri(@"/Graphics/Tiles/Finish.png", UriKind.Relative))
                        };
                        TextBlock textBlock = new()
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            TextAlignment = TextAlignment.Center,
                            Text = "F",
                            FontSize = 30,
                            Foreground = new SolidColorBrush(Colors.Red)
                        };
                        grid.Children.Add(image);
                        grid.Children.Add(textBlock);
                        button.Content = grid;
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
        CultureInfo ci = new(cultureInfoToSet);
        Thread.CurrentThread.CurrentCulture = ci;
        Thread.CurrentThread.CurrentUICulture = ci;
    }
    private void CommandBinding_New_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void CommandBinding_New_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        NewLevel();
        MessageBox.Show("New project created!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
    }
    private void CommandBinding_Exit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void CommandBinding_Exit_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        Close();
    }
    private void CommandBinding_AboutProgram_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void CommandBinding_AboutProgram_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        AboutProgramWindow apWindow = new();
        apWindow.ShowDialog();
    }
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        for (int i = _commands.Count - 1; i > _indexCommand; i--)
        {
            _commands.RemoveAt(i);
        }
        Button? button = sender as Button;
        if (button?.ToolTip == null) return; 
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
                _ = new NormalIncCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                PostCommandUpdate();
                break;
            case EditMode.NormalDec:
                if (floorTileIndex >= 0)
                {
                    FloorTile floorTile = _floorTiles[floorTileIndex];
                    if (floorTile.Type == FloorTileType.Finish)
                    {
                        MessageBox.Show("Cannot decrees Finish floor Tile.", "Wrong Selection",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (point == new Point(0, 0) && floorTile.Number < 2)
                    {
                        MessageBox.Show("Cannot decrees Start floor Tile below number 1.", "Wrong Selection",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        _ = new NormalDecCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                        PostCommandUpdate();
                    }
                }
                break;
            case EditMode.IceInc:
                _ = new IceIncCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                PostCommandUpdate();
                break;
            case EditMode.IceDec:
                floorTileIndex = FindFloor(point);
                if (floorTileIndex >= 0)
                {
                    FloorTile floorTile = _floorTiles[floorTileIndex];
                    if (floorTile.Type == FloorTileType.Finish)
                    {
                        MessageBox.Show("Cannot decrees Finish floor Tile.", "Wrong Selection",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (point == new Point(0, 0) && floorTile.Number < 2)
                    {
                        MessageBox.Show("Cannot decrees Start floor Tile below number 1.", "Wrong Selection",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        _ = new IceDecCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                        PostCommandUpdate();
                    }
                }
                break;
            case EditMode.Static:
                floorTileIndex = FindFloor(point);
                if (floorTileIndex < 0)
                {
                    _ = new StaticCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                    PostCommandUpdate();
                }
                else
                {
                    FloorTile floorTile = _floorTiles[floorTileIndex];
                    if (floorTile.Type != FloorTileType.Static)
                    {
                        _ = new StaticCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                        PostCommandUpdate();
                    }
                }
                break;
            case EditMode.PortalInc:
                if (point == new Point(0, 0))
                {
                    MessageBox.Show("Cannot change Start floor tile to Portal.", "Wrong Selection",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    _ = new PortalIncCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                    PostCommandUpdate();
                }
                break;
            case EditMode.PortalDec:
                if (point == new Point(0, 0))
                {
                    MessageBox.Show("Cannot change Start floor tile to Portal.", "Wrong Selection",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    floorTileIndex = FindFloor(point);
                    if (floorTileIndex >= 0)
                    {
                        FloorTile floorTile = _floorTiles[floorTileIndex];
                        if (floorTile.Type == FloorTileType.Finish)
                        {
                            MessageBox.Show("Cannot decrees Finish floor Tile.", "Wrong Selection",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            _ = new PortalDecCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                            PostCommandUpdate();
                        }
                    }
                }
                break;
            case EditMode.SpringInc:
                if (point == new Point(0, 0))
                {
                    MessageBox.Show("Cannot change Start floor tile to Spring.", "Wrong Selection",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    _ = new SpringIncCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                    PostCommandUpdate();
                }
                break;
            case EditMode.SpringDec:
                if (point == new Point(0, 0))
                {
                    MessageBox.Show("Cannot change Start floor tile to Spring.", "Wrong Selection",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    floorTileIndex = FindFloor(point);
                    if (floorTileIndex >= 0)
                    {
                        FloorTile floorTile = _floorTiles[floorTileIndex];
                        if (floorTile.Type == FloorTileType.Finish)
                        {
                            MessageBox.Show("Cannot decrees Finish floor Tile.", "Wrong Selection",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            _ = new SpringDecCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                            PostCommandUpdate();
                        }
                    }
                }
                break;
            case EditMode.SpringLeft:
                if (point == new Point(0, 0))
                {
                    MessageBox.Show("Cannot change Start floor tile to Spring.", "Wrong Selection",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    floorTileIndex = FindFloor(point);
                    if (floorTileIndex >= 0)
                    {
                        FloorTile floorTile = _floorTiles[floorTileIndex];
                        if (floorTile.Type == FloorTileType.Spring)
                        {
                            _ = new SpringLeftCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                            PostCommandUpdate();
                        }
                        else
                        {
                            MessageBox.Show("Cannot change direction of this floor tile. Only existing Spring tile can be modified",
                                "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                break;
            case EditMode.SpringRight:
                if (point == new Point(0, 0))
                {
                    MessageBox.Show("Cannot change Start floor tile to Spring.", "Wrong Selection",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    floorTileIndex = FindFloor(point);
                    if (floorTileIndex >= 0)
                    {
                        FloorTile floorTile = _floorTiles[floorTileIndex];
                        if (floorTile.Type == FloorTileType.Spring)
                        {
                            _ = new SpringRightCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                            PostCommandUpdate();
                        }
                        else
                        {
                            MessageBox.Show("Cannot change direction of this floor tile. Only existing Spring tile can be modified",
                                "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                break;
            case EditMode.BombInitalInc:
                if (point == new Point(0, 0))
                {
                    MessageBox.Show("Cannot change Start floor tile to Bomb Init.", "Wrong Selection",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    _ = new BombInitIncCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                    PostCommandUpdate();
                }
                break;
            case EditMode.BombInitialDec:
                if (point == new Point(0, 0))
                {
                    MessageBox.Show("Cannot change Start floor tile to Bomb Init.", "Wrong Selection",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (floorTileIndex >= 0)
                {
                    _ = new BombInitDecCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                    PostCommandUpdate();
                }
                break;
            case EditMode.BombModInc:
                if (point == new Point(0, 0))
                {
                    MessageBox.Show("Cannot change Start floor tile to Bomb Mod.", "Wrong Selection",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    _ = new BombModIncCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                    PostCommandUpdate();
                }
                break;
            case EditMode.BombModDec:
                if (point == new Point(0, 0))
                {
                    MessageBox.Show("Cannot change Start floor tile to Bomb Mod.", "Wrong Selection",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    _ = new BombModDecCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                    PostCommandUpdate();
                }
                break;
            case EditMode.FinishTile:
                if (point == new Point(0, 0))
                {
                    MessageBox.Show("Cannot change Start floor tile to Finish.", "Wrong Selection",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    floorTileIndex = FindFloor(point);
                    
                    if (floorTileIndex < 0)
                    {
                        _ = new FinishCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
                        PostCommandUpdate();
                    }
                    else
                    {
                        FloorTile floorTile = _floorTiles[floorTileIndex];
                        if (floorTile.Type != FloorTileType.Finish)
                        {
                            _ = new FinishCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
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
                        MessageBox.Show("Cannot remove Start floor tile.", "Wrong Selection",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        _ = new RemoveCommand(_commands, _floorTiles, point, _indexCommand + 1, floorTileIndex);
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
    private void CommandBinding_Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = _indexCommand >= 0;
    }

    private void CommandBinding_Undo_Executed(object sender, ExecutedRoutedEventArgs e)
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

    private void CommandBinding_Redo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = _indexCommand < (_commands.Count - 1);
    }

    private void CommandBinding_Redo_Executed(object sender, ExecutedRoutedEventArgs e)
    {            
        _indexCommand++;
        _commands[_indexCommand].Redo();
        PostUndoRedoCommandsList();
        UpdateMainGridViewOnlyButtons();
    }

    private void CommandBinding_Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void CommandBinding_Open_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        OpenFileDialog openDialogOpen = new()
        {
            Filter = "Game Level | *.xml"
        };
        bool? result = openDialogOpen.ShowDialog();
        if (result.HasValue && result.Value)
        {
            _floorTiles.Clear();
            try
            {
                XmlDocument xmlDocument = new();
                xmlDocument.Load(openDialogOpen.FileName);
                string xmlString = xmlDocument.OuterXml;
                using (StringReader read = new(xmlString))
                {
                    Type outType = typeof(List<FloorTile>);

                    XmlSerializer serializer = new(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        _floorTiles = serializer.Deserialize(reader) as List<FloorTile>;
                        lvFloorTiles.ItemsSource = _floorTiles;
                        reader.Close();
                    }
                    read.Close();
                }
                _projectName = Path.GetFileNameWithoutExtension(openDialogOpen.FileName);
                _projectPath = Path.GetDirectoryName(openDialogOpen?.FileName);
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
                MessageBox.Show("Game level was opened correct!", "New level confirmation",
                    MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (Exception)
            {
                MessageBox.Show("Game level cannot be opened!\nFile is corrupted.",
                    "Open level error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void CommandBinding_Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void CommandBinding_Save_Executed(object sender, ExecutedRoutedEventArgs e)
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
                MessageBox.Show("Game level was saved correct!", "Save level confirmation",
                    MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                MessageBox.Show("Game level was not saved correct!\nSome unexpected error occur.",
                    "Error during save level", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void CommandBinding_SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (!ShouldPerformSaveAfterCheck()) return;

        SaveAs();
    }

    private void CommandBinding_SaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private static string GetProjectNameInLang()
    {
        return "Sliding Tile - PC Level Editor";
    }
    private void SaveAs()
    {
        SaveFileDialog textDialogSave = new()
        {
            Filter = "Game level | *.xml"
        };
        bool? result = textDialogSave.ShowDialog();
        if (result.HasValue && result.Value)
        {
            bool bRetAfterSave = SaveProject(_floorTiles, Path.GetFileNameWithoutExtension(textDialogSave.FileName),
                Path.GetDirectoryName(textDialogSave.FileName));
            if (bRetAfterSave == true)
            {
                AssigneProjectNameAndPath();
            }
            else
            {
                MessageBox.Show("Game level was not saved correct!\nSome unexpected error occur.",
                    "Error during save level", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    private void AssigneProjectNameAndPath()
    {
        this.Title = GetProjectNameInLang() + " [" + _projectName + "]";
        sbiProjectPath.Text = _projectPath;
        MessageBox.Show("Game level was saved correct!", "Save level confirmation",
            MessageBoxButton.OK, MessageBoxImage.Asterisk);
    }
    private void IntegerUpDown_AreaViewDim_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
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
    private void CommandBinding_MoveViewCommon_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void CommandBinding_MoveViewUp_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        IncViewRange(0, 0, -1, -1);
    }
    private void CommandBinding_MoveViewDown_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        IncViewRange(0, 0, 1, 1);
    }
    private void CommandBinding_MoveViewLeft_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        IncViewRange(1, 1, 0, 0);
    }
    private void CommandBinding_MoveViewRight_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        IncViewRange(-1, -1, 0, 0);
    }
    private void CommandBinding_MoveViewLeftUp_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        IncViewRange(1, 1, -1, -1);
    }
    private void CommandBinding_MoveViewRightUp_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        IncViewRange(-1, -1, -1, -1);
    }
    private void CommandBinding_MoveViewLeftDown_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        IncViewRange(1, 1, 1, 1);
    }
    private void CommandBinding_MoveViewRightDown_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        IncViewRange(-1, -1, 1, 1);
    }
    private void CommandBinding_FullLevelView_Executed(object sender, ExecutedRoutedEventArgs e)
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
    private void CommandBinding_ZoomOutView_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        IncViewRange(-1, 1, -1, 1);
    }
    private void CommandBinding_ZoomInView_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = CheckPossibleZoomIn();
    }
    private void CommandBinding_ZoomInView_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        IncViewRange(1, -1, 1, -1);
    }
    private void ViewPlaceAllCells_MouseWheel(object sender, MouseWheelEventArgs e)
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
    private void CommandBinding_EditModeCommon_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void CommandBinding_EditModeNormalInc_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        _editMode = EditMode.NormalInc;
        UpdateEditBorders();
    }
    private void CommandBinding_EditModeNormalDec_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        _editMode = EditMode.NormalDec;
        UpdateEditBorders();
    }
    private void CommandBinding_EditModeIceInc_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        _editMode = EditMode.IceInc;
        UpdateEditBorders();
    }
    private void CommandBinding_EditModeIceDec_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        _editMode = EditMode.IceDec;
        UpdateEditBorders();
    }
    private void CommandBinding_EditModeStatic_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        _editMode = EditMode.Static;
        UpdateEditBorders();
    }
    private void CommandBinding_EditModePortalInc_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        _editMode = EditMode.PortalInc;
        UpdateEditBorders();
    }
    private void CommandBinding_EditModePortalDec_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        _editMode = EditMode.PortalDec;
        UpdateEditBorders();
    }
    private void CommandBinding_EditModeSpringInc_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        _editMode = EditMode.SpringInc;
        UpdateEditBorders();
    }
    private void CommandBinding_EditModeSpringDec_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        _editMode = EditMode.SpringDec;
        UpdateEditBorders();
    }
    private void CommandBinding_EditModeSpringLeft_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        _editMode = EditMode.SpringLeft;
        UpdateEditBorders();
    }
    private void CommandBinding_EditModeSpringRight_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        _editMode = EditMode.SpringRight;
        UpdateEditBorders();
    }
    private void CommandBinding_EditModeBombInitialInc_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        _editMode = EditMode.BombInitalInc;
        UpdateEditBorders();
    }
    private void CommandBinding_EditModeBombInitialDec_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        _editMode = EditMode.BombInitialDec;
        UpdateEditBorders();
    }
    private void CommandBinding_EditModeBombModInc_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        _editMode = EditMode.BombModInc;
        UpdateEditBorders();
    }
    private void CommandBinding_EditModeBombModDec_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        _editMode = EditMode.BombModDec;
        UpdateEditBorders();
    }
    private void CommandBinding_EditModeFinish_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        _editMode = EditMode.FinishTile;
        UpdateEditBorders();
    }
    private void CommandBinding_EditModeDeleteTile_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        _editMode = EditMode.DeleteTile;
        UpdateEditBorders();
    }
    private void Button_MouseEnter(object sender, MouseEventArgs e)
    {
        Button bTemo = (Button)sender;
        tbCurrentGrid.Text = bTemo.ToolTip.ToString();
    }
    private void CommandBinding_DeleteAllTiles_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        if (_floorTiles != null)
        {
            if (_floorTiles.Count > 1)
            {
                e.CanExecute = true;
            } 
        }
    }
    private void CommandBinding_DeleteAllTiles_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        _ = new ClearAllCommand(_commands, _floorTiles, _indexCommand + 1);
        PostCommandUpdate();
        UpdateMainGridViewOnlyButtons();
    }
    private void CommandBinding_Check_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void CommandBinding_Check_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        CheckLevelWindow checkLevelWindow = new(_floorTiles);
        checkLevelWindow.ShowDialog();
    }
    private bool ShouldPerformSaveAfterCheck()
    {
        bool retrunVal;
        CheckLevelWindow checkLevelWindow = new(_floorTiles);
        int failsNumber = checkLevelWindow.GetNumberFailedTests();
        if (failsNumber > 0)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show($"In current project detected some problems.\nNumber of problem: {failsNumber}\nDo you want save anymore?",
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
    private void CommandBinding_Options_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void CommandBinding_Options_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        OptionsWindow optionsWindow = new();
        optionsWindow.ShowDialog();
    }

    private void CommandBinding_ProjectResults_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void CommandBinding_ProjectResults_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        ProjectResultsWindow projectResultsWindow = new();
        projectResultsWindow.ShowDialog();
    }

    private bool SaveProject(List<FloorTile> saveObject, string pName, string pPath)
    {
        bool correctSave = false;
        if (pName == "")
        {
            MessageBox.Show("Please select a correct level name.", "Incorrect level name",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else if (pPath == "")
        {
            MessageBox.Show("Please select a correct level path.", "Incorrect level path",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else if (!Directory.Exists(pPath))
        {
            MessageBox.Show("The level path does not exist.", "No exist level path",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            try
            {
                XmlDocument xmlDocument = new();
                XmlSerializer serializer = new(saveObject.GetType());
                string fileLoc = Path.Combine(pPath, pName + ".xml");
                using (MemoryStream stream = new())
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
