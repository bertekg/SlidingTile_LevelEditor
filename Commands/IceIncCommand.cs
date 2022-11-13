using SlidingTile_LevelEditor.Class;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SlidingTile_LevelEditor.Commands
{
    class IceIncCommand : Command
    {
        private List<Command> _commands;
        private Point _point;
        private int _commandIndex;
        private List<FloorTile> _floorTiles;
        private FloorTile _beforChange;
        private FloorTile _afterChange;
        private int _floorTileIndex;
        public IceIncCommand(List<Command> commands, List<FloorTile> floorTiles, Point point, int commandIndex)
        {
            _commands = commands;
            _point = point;
            _commandIndex = commandIndex;
            _beforChange = new FloorTile();
            _afterChange = new FloorTile();
            _floorTiles = floorTiles;
            Execute();
        }
        public override void Execute()
        {
            _commands.Add(this);
            _floorTileIndex = FindFloor();
            if (_floorTileIndex >= 0)
            {
                _beforChange = new FloorTile
                {
                    Type = _floorTiles[_floorTileIndex].Type,
                    PosX = _floorTiles[_floorTileIndex].PosX,
                    PosY = _floorTiles[_floorTileIndex].PosY,
                    Number = _floorTiles[_floorTileIndex].Number
                };
                _floorTiles[_floorTileIndex].Type = FloorTileType.Ice;
                _floorTiles[_floorTileIndex].Number++;
                _afterChange = new FloorTile
                {
                    Type = _floorTiles[_floorTileIndex].Type,
                    PosX = _floorTiles[_floorTileIndex].PosX,
                    PosY = _floorTiles[_floorTileIndex].PosY,
                    Number = _floorTiles[_floorTileIndex].Number
                };
            }
            else
            {
                _beforChange = null;
                FloorTile floorTile = new FloorTile()
                {
                    PosX = (int)_point.X,
                    PosY = (int)_point.Y,
                    Type = FloorTileType.Ice,
                    Number = 1
                };
                _floorTiles.Add(floorTile);
                _afterChange = new FloorTile()
                {
                    Type = floorTile.Type,
                    PosX = floorTile.PosX,
                    PosY = floorTile.PosY,
                    Number = floorTile.Number
                };
                _floorTileIndex = _floorTiles.Count - 1;
            }
        }
        private int FindFloor()
        {
            return _floorTiles.FindIndex(item => item.PosX == _point.X && item.PosY == _point.Y);
        }
        public override void Redo()
        {
            if (_floorTiles.ElementAtOrDefault(_floorTileIndex) != null)
            {
                _floorTiles[_floorTileIndex] = new FloorTile()
                {
                    Type = _afterChange.Type,
                    PosX = _afterChange.PosX,
                    PosY = _afterChange.PosY,
                    Number = _afterChange.Number
                };
            }
            else
            {
                FloorTile floorTileToInser = new FloorTile()
                {
                    Type = _afterChange.Type,
                    PosX = _afterChange.PosX,
                    PosY = _afterChange.PosY,
                    Number = _afterChange.Number
                };
                _floorTiles.Insert(_floorTileIndex, floorTileToInser);
            }
        }
        public override void Undo()
        {
            if (_beforChange != null)
            {
                _floorTiles[_floorTileIndex].Type = _beforChange.Type;
                _floorTiles[_floorTileIndex].Number = _beforChange.Number;
            }
            else
            {
                _floorTiles.RemoveAt(_floorTileIndex);
            }
        }
        public override string ToString()
        {
            string returnText;
            if (_beforChange != null)
            {
                returnText = _commandIndex.ToString() + "; Ice INC [" + _point.X.ToString() + "," + _point.Y.ToString() +
                    "] Number: " + _beforChange.Number.ToString() + " -> " + _afterChange.Number.ToString();
            }
            else
            {
                returnText = _commandIndex.ToString() + "; Ice INC [" + _point.X.ToString() + "," + _point.Y.ToString() +
                    "] Number: null -> " + _afterChange.Number.ToString();
            }
            return returnText;
        }
    }
}
