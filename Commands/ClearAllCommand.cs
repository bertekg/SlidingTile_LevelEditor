using SlidingTile_LevelEditor.Class;
using System.Collections.Generic;

namespace SlidingTile_LevelEditor.Commands
{
    class ClearAllCommand : Command
    {
        private List<Command> _commands;
        private int _commandIndex;
        private List<FloorTile> _floorTiles;
        private List<FloorTile> _beforChange;
        private List<FloorTile> _afterChange;
        public ClearAllCommand(List<Command> commands, List<FloorTile> floorTiles, int commandIndex)
        {
            _commands = commands;
            _commandIndex = commandIndex;
            _beforChange = new List<FloorTile>();
            _afterChange = new List<FloorTile>();
            _floorTiles = floorTiles;
            Execute();
        }

        public override void Execute()
        {
            _commands.Add(this);
            foreach (FloorTile tile in _floorTiles)
            {
                FloorTile newFloorTile = new FloorTile()
                {
                    PosX = tile.PosX,
                    PosY = tile.PosY,
                    Number = tile.Number,
                    Type = tile.Type
                };
                _beforChange.Add(newFloorTile);
            }
            _floorTiles.Clear();
            _floorTiles.Add(new FloorTile() { PosX = 0, PosY = 0, Type = FloorTileType.Normal, Number = 1 });
            _afterChange.Add(new FloorTile() { PosX = 0, PosY = 0, Type = FloorTileType.Normal, Number = 1 });
        }
        public override void Undo()
        {
            _floorTiles.Clear();
            foreach (FloorTile tile in _beforChange)
            {
                FloorTile newFloorTile = new FloorTile()
                {
                    PosX = tile.PosX,
                    PosY = tile.PosY,
                    Number = tile.Number,
                    Type = tile.Type
                };
                _floorTiles.Add(newFloorTile);
            }
        }
        public override void Redo()
        {
            _floorTiles.Clear();
            foreach (FloorTile tile in _afterChange)
            {
                FloorTile newFloorTile = new FloorTile()
                {
                    PosX = tile.PosX,
                    PosY = tile.PosY,
                    Number = tile.Number,
                    Type = tile.Type
                };
                _floorTiles.Add(newFloorTile);
            }
        }
        public override string ToString()
        {
            return _commandIndex.ToString() + "; Delete all tiles, before count: " + _beforChange.Count.ToString();
        }
    }
}
