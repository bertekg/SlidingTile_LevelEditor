using SlidingTile_LevelEditor.Class;
using System.Collections.Generic;

namespace SlidingTile_LevelEditor.Commands;

public class ClearAllCommand : Command
{
    private readonly List<Command> _commands;
    private readonly int _commandIndex;
    private readonly List<FloorTile> _floorTiles;
    private readonly List<FloorTile> _beforChange;
    private readonly List<FloorTile> _afterChange;
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
            FloorTile newFloorTile = new()
            {
                PosX = tile.PosX,
                PosY = tile.PosY,
                Type = tile.Type,
                Number = tile.Number,
                Portal = tile.Portal,
                Spring = tile.Spring,
                Bomb = tile.Bomb
            };
            _beforChange.Add(newFloorTile);
        }
        _floorTiles.Clear();
        _floorTiles.Add(new FloorTile()
        {
            PosX = 0, PosY = 0, Type = FloorTileType.Normal,
            Number = 1, Portal = 0, Spring = SpringDirection.Up,
            Bomb = 0
        });
        _afterChange.Add(new FloorTile() 
        { 
            PosX = 0, PosY = 0, Type = FloorTileType.Normal,
            Number = 1, Portal = 0, Spring = SpringDirection.Up,
            Bomb = 0
        });
    }
    public override void Undo()
    {
        _floorTiles.Clear();
        foreach (FloorTile tile in _beforChange)
        {
            FloorTile newFloorTile = new()
            {
                PosX = tile.PosX,
                PosY = tile.PosY,
                Type = tile.Type,
                Number = tile.Number,
                Portal = tile.Portal,
                Spring = tile.Spring,
                Bomb = tile.Bomb
            };
            _floorTiles.Add(newFloorTile);
        }
    }
    public override void Redo()
    {
        _floorTiles.Clear();
        foreach (FloorTile tile in _afterChange)
        {
            FloorTile newFloorTile = new()
            {
                PosX = tile.PosX,
                PosY = tile.PosY,
                Type = tile.Type,
                Number = tile.Number,
                Portal = tile.Portal,
                Spring = tile.Spring,
                Bomb = tile.Bomb
            };
            _floorTiles.Add(newFloorTile);
        }
    }
    public override string ToString()
    {
        return $"{_commandIndex}; Delete all tiles, before count: {_beforChange.Count}";
    }
}
