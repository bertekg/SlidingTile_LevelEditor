using SlidingTile_LevelEditor.Class;
using System.Collections.Generic;
using System.Windows;

namespace SlidingTile_LevelEditor.Commands
{
    class RemoveCommand : Command
    {
        private List<Command> _commands;
        private Point _point;
        private int _commandIndex;
        private List<FloorTile> _floorTiles;
        private FloorTile _beforChange;
        private int _floorTileIndex;
        public RemoveCommand(List<Command> commands, List<FloorTile> floorTiles, Point point, int commandIndex, int floorTileIndex)
        {
            _commands = commands;
            _point = point;
            _commandIndex = commandIndex;
            _beforChange = new FloorTile();
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
                Number = _floorTiles[_floorTileIndex].Number,
                Portal = _floorTiles[_floorTileIndex].Portal
            };
            _floorTiles.RemoveAt(_floorTileIndex);
        }
        public override void Undo()
        {
            FloorTile floorTileToInser = new FloorTile()
            {
                Type = _beforChange.Type,
                PosX = _beforChange.PosX,
                PosY = _beforChange.PosY,
                Number = _beforChange.Number,
                Portal = _beforChange.Portal
            };
            _floorTiles.Insert(_floorTileIndex, floorTileToInser);
        }
        public override void Redo()
        {
            _floorTiles.RemoveAt(_floorTileIndex);
        }
        public override string ToString()
        {
            return _commandIndex.ToString() + "; Remove [" + _point.X.ToString() + "," + _point.Y.ToString() +
                    "] Number: " + _beforChange.Number.ToString() + " -> null" +
                    ", Portal: " + _beforChange.Portal.ToString() + " -> null";
        }
    }
}
