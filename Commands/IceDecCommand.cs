using SlidingTile_LevelEditor.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SlidingTile_LevelEditor.Commands
{
    class IceDecCommand : Command
    {
        private List<Command> _commands;
        private Point _point;
        private int _commandIndex;
        private List<FloorTile> _floorTiles;
        private FloorTile _beforChange;
        private FloorTile _afterChange;
        private int _floorTileIndex;
        public IceDecCommand(List<Command> commands, List<FloorTile> floorTiles, Point point, int commandIndex, int floorTileIndex) 
        {
            _commands = commands;
            _point = point;
            _commandIndex = commandIndex;
            _beforChange = new FloorTile();
            _afterChange = new FloorTile();
            _floorTiles = floorTiles;
            _floorTileIndex = floorTileIndex;
            Execute();
        }
        public override void Execute()
        {
            _commands.Add(this);
            _beforChange = new FloorTile
            {
                Type = _floorTiles[_floorTileIndex].Type,
                PosX = _floorTiles[_floorTileIndex].PosX,
                PosY = _floorTiles[_floorTileIndex].PosY,
                Number = _floorTiles[_floorTileIndex].Number
            };
            if (_floorTiles[_floorTileIndex].Number > 1)
            {
                _floorTiles[_floorTileIndex].Type = FloorTileType.Ice;
                _floorTiles[_floorTileIndex].Number--;
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
                _floorTiles.RemoveAt(_floorTileIndex);
                _afterChange = null;
            }
        }
        public override void Undo()
        {
            if (_afterChange != null)
            {
                _floorTiles[_floorTileIndex].Type = _beforChange.Type;
                _floorTiles[_floorTileIndex].Number = _beforChange.Number;
            }
            else
            {
                FloorTile floorTileToInser = new FloorTile()
                {
                    Type = _beforChange.Type,
                    PosX = _beforChange.PosX,
                    PosY = _beforChange.PosY,
                    Number = _beforChange.Number
                };
                _floorTiles.Insert(_floorTileIndex, floorTileToInser);
            }
        }
        public override void Redo()
        {
            if (_afterChange != null)
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
                _floorTiles.RemoveAt(_floorTileIndex);
            }
        }
        public override string ToString()
        {
            string returnText;
            if (_afterChange != null)
            {
                returnText = _commandIndex.ToString() + "; Ice DEC [" + _point.X.ToString() + "," + _point.Y.ToString() +
                    "] Number: " + _beforChange.Number.ToString() + " -> " + _afterChange.Number.ToString();
            }
            else
            {
                returnText = _commandIndex.ToString() + "; Ice DEC [" + _point.X.ToString() + "," + _point.Y.ToString() +
                    "] Number: " + _beforChange.Number.ToString() + " -> null";
            }
            return returnText;
        }
    }
}
