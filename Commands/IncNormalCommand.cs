using SlidingTile_LevelEditor.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SlidingTile_LevelEditor.Commands
{
    class IncNormalCommand : Command
    {
        private List<Command> _commands;
        private Point _point;
        private int _commandIndex;
        public bool _isCurrentCommand;
        private List<FloorTile> _floorTiles;
        private List<FloorTile> _beforChange;
        private List<FloorTile> _afterChange;
        public IncNormalCommand(List<Command> commands, List<FloorTile> floorTiles, Point point, int commandIndex)
        {
            _commands = commands;
            _point = point;
            _commandIndex = commandIndex;
            _isCurrentCommand = true;
            _beforChange = new List<FloorTile>();
            _afterChange = new List<FloorTile>();
            _floorTiles = floorTiles;
            Execute();
        }

        public override void Execute()
        {
            _commands.Add(this);
            int index = FindFloor();
            if (index >= 0)
            {
                _floorTiles[index].Type = FloorTileType.Normal;
                _floorTiles[index].Number++;
            }
            else 
            {
                FloorTile floorTile = new FloorTile()
                {
                    PosX = (int)_point.X,
                    PosY = (int)_point.Y,
                    Type = FloorTileType.Normal,
                    Number = 1
                };
                _floorTiles.Add(floorTile);
            }
        }
        private int FindFloor()
        {
            return _floorTiles.FindIndex(item => item.PosX == _point.X&& item.PosY == _point.Y);
        }

        public override void Redo()
        {
            throw new NotImplementedException();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return _isCurrentCommand.ToString() + ">" + _commandIndex.ToString() + ":"  + _point.X.ToString() + "," + _point.Y.ToString();
        }
    }
}
