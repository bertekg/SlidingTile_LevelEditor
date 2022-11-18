using SlidingTile_LevelEditor.Class;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace SlidingTile_LevelEditor.Windows
{
    public partial class CheckLevelWindow : Window
    {
        List<FloorTile> _floorTiles;

        List<string> _passedTests = new List<string>();
        List<string> _failedTests = new List<string>();
        public CheckLevelWindow(List<FloorTile> floorTiles)
        {
            InitializeComponent();
            _floorTiles = floorTiles;
            CheckStartFloorTile();
            CheckFinishFloorTile();
            CheckMinimumNumberTiles();
            CheckFloorTileRepeted();
            textBox_totalNumberTests.Text = (_failedTests.Count + _passedTests.Count).ToString();
            textBox_failNumberTests.Text = _failedTests.Count.ToString();
            listView_passedTest.ItemsSource = _passedTests;
            textBox_passNumberTests.Text = _passedTests.Count.ToString();
            listView_failedTest.ItemsSource = _failedTests;
        }
        public int GetNumberFailedTests()
        {
            return _failedTests.Count;
        }
        private void CheckMinimumNumberTiles()
        {
            const string testStartPart = "MINIMUM NUMBER TILES: ";
            if (_floorTiles.Count >= 2)
            {
                _passedTests.Add(testStartPart + "Number of tiles are at least 2.");
            }
            else
            {
                _failedTests.Add(testStartPart + "Number of tiles less then 2, must be Start and minimum 1 Finish tile!");
            }
        }

        private void CheckFloorTileRepeted()
        {
            const string testStartPart = "FLOOR TILE REPETED: ";
            for (int i = 0; i < _floorTiles.Count; i++)
            {
                List<FloorTile> resultFloorTiles = _floorTiles.FindAll(item => item.PosX == _floorTiles[i].PosX && item.PosY == _floorTiles[i].PosY);
                if (resultFloorTiles.Count == 1)
                {
                    _passedTests.Add(testStartPart + "Tile in position [" + _floorTiles[i].PosX.ToString() + "," +
                            _floorTiles[i].PosY.ToString() + "] is unique (not exist more tile on same position).");
                }
                else
                {
                    _failedTests.Add(testStartPart + "Tile in position [" + _floorTiles[i].PosX.ToString() + "," +
                            _floorTiles[i].PosY.ToString() + "] is not unique, found: " + _floorTiles.Count.ToString() + " times.");
                }
            }
        }
        private void CheckFinishFloorTile()
        {
            const string testStartPart = "FINISH FLOOR TILE: ";
            List<FloorTile> resultFloorTiles = _floorTiles.FindAll(item => item.Type == FloorTileType.Finish);
            if (resultFloorTiles.Count > 0)
            {
                _passedTests.Add(testStartPart + "Minimum one Finish tile exist.");
                for (int i = 0; i < resultFloorTiles.Count; i++)
                {
                    if (resultFloorTiles[i].Number == 0)
                    {
                        _passedTests.Add(testStartPart + "Finish tile in position [" + resultFloorTiles[i].PosX.ToString() + "," +
                            resultFloorTiles[i].PosY.ToString() + "] have correct Number (equal 0).");
                    }
                    else
                    {
                        _failedTests.Add(testStartPart + "Finish tile in position [" + resultFloorTiles[i].PosX.ToString() + "," +
                            resultFloorTiles[i].PosY.ToString() + "] have wrong Number (not equal 0).");
                    }

                    if (resultFloorTiles[i].PosX != 0 || resultFloorTiles[i].PosY != 0)
                    {
                        _passedTests.Add(testStartPart + "Finish tile in position [" + resultFloorTiles[i].PosX.ToString() + "," +
                            resultFloorTiles[i].PosY.ToString() + "] is not on Start position.");
                    }
                    else
                    {
                        _failedTests.Add(testStartPart + "Finish tile in position [" + resultFloorTiles[i].PosX.ToString() + "," +
                            resultFloorTiles[i].PosY.ToString() + "] is on Start position.");
                    }
                }
            }
            else
            {
                _failedTests.Add(testStartPart + "Missing Finish tile in level.");
            }
        }
        private void CheckStartFloorTile()
        {
            const string testStartPart = "START FLOOR TILE: ";
            List<FloorTile> resultFloorTiles = _floorTiles.FindAll(item => item.PosX == 0 && item.PosY == 0);
            if (resultFloorTiles.Count == 1)
            {
                _passedTests.Add(testStartPart + "Tile exist on the list.");

                if (resultFloorTiles[0].Type == FloorTileType.Normal || 
                    resultFloorTiles[0].Type == FloorTileType.Ice ||
                    resultFloorTiles[0].Type == FloorTileType.Solid)
                {
                    _passedTests.Add(testStartPart + "Correct type.");
                }
                else
                {
                    _failedTests.Add(testStartPart + "Wrong type, must be Normal, Ice, Solid.");
                }

                if (resultFloorTiles[0].Number > 0 && 
                    (resultFloorTiles[0].Type == FloorTileType.Normal ||
                    resultFloorTiles[0].Type == FloorTileType.Ice))
                {
                    _passedTests.Add(testStartPart + "Number value is over 0 for current type: " + resultFloorTiles[0].Type.ToString() + ".");
                }
                else if (resultFloorTiles[0].Type == FloorTileType.Solid)
                {
                    _passedTests.Add(testStartPart + "Number value can be 0 for Solid.");
                }
                else
                {
                    _failedTests.Add(testStartPart + "Number value is no over 0.");
                }
            }
            else if (resultFloorTiles.Count > 1)
            {
                _failedTests.Add(testStartPart + "More then 1 tile on start position.");
            }
            else
            {
                _failedTests.Add(testStartPart + "Missing tile on the list on start position.");
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
