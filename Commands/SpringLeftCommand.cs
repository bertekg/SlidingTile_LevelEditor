using SlidingTile_LevelEditor.Class;
using System.Collections.Generic;
using System.Windows;

namespace SlidingTile_LevelEditor.Commands
{
    class SpringLeftCommand : Command
    {
        private List<Command> _commands;
        private Point _point;
        private int _commandIndex;
        private List<FloorTile> _floorTiles;
        private FloorTile _beforChange;
        private FloorTile _afterChange;
        private int _floorTileIndex;
        public SpringLeftCommand(List<Command> commands, List<FloorTile> floorTiles, Point point, int commandIndex, int floorTileIndex)
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
                PosX = _floorTiles[_floorTileIndex].PosX,
                PosY = _floorTiles[_floorTileIndex].PosY,
                Type = _floorTiles[_floorTileIndex].Type,
                Number = _floorTiles[_floorTileIndex].Number,
                Portal = _floorTiles[_floorTileIndex].Portal,
                Spring = _floorTiles[_floorTileIndex].Spring,
                Bomb = _floorTiles[_floorTileIndex].Bomb
            };
            _floorTiles[_floorTileIndex].Spring++;
            if ((int)_floorTiles[_floorTileIndex].Spring > (int)SpringDirection.Right)
            {
                _floorTiles[_floorTileIndex].Spring = SpringDirection.Up;
            }
            _afterChange = new FloorTile
            {
                PosX = _floorTiles[_floorTileIndex].PosX,
                PosY = _floorTiles[_floorTileIndex].PosY,
                Type = _floorTiles[_floorTileIndex].Type,
                Number = _floorTiles[_floorTileIndex].Number,
                Portal = _floorTiles[_floorTileIndex].Portal,
                Spring = _floorTiles[_floorTileIndex].Spring,
                Bomb = _floorTiles[_floorTileIndex].Bomb
            };
        }
        public override void Undo()
        {
            _floorTiles[_floorTileIndex] = new FloorTile()
            {
                PosX = _beforChange.PosX,
                PosY = _beforChange.PosY,
                Type = _beforChange.Type,
                Number = _beforChange.Number,
                Portal = _beforChange.Portal,
                Spring = _beforChange.Spring,
                Bomb = _beforChange.Bomb
            };
        }
        public override void Redo()
        {
            _floorTiles[_floorTileIndex] = new FloorTile()
            {
                PosX = _afterChange.PosX,
                PosY = _afterChange.PosY,
                Type = _afterChange.Type,
                Number = _afterChange.Number,
                Portal = _afterChange.Portal,
                Spring = _afterChange.Spring,
                Bomb = _afterChange.Bomb
            };
        }
        public override string ToString()
        {
            return $"{_commandIndex}; Spring Left [{_point.X},{_point.Y}] Number: {_beforChange.Number} -> {_afterChange.Number}, " +
                    $"Portal: {_beforChange.Portal} -> {_afterChange.Portal}, Spring: {_beforChange.Spring} -> {_afterChange.Spring}, " +
                    $"Bomb: {_beforChange.Bomb} -> {_afterChange.Bomb}"; ;
        }
    }
}
