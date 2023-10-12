using SlidingTile_LevelEditor.Class;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SlidingTile_LevelEditor.Commands;

public class StaticCommand : Command
{
    private readonly List<Command> _commands;
    private Point _point;
    private readonly int _commandIndex;
    private readonly List<FloorTile> _floorTiles;
    private FloorTile? _beforChange;
    private FloorTile _afterChange;
    private int _floorTileIndex;
    public StaticCommand(List<Command> commands, List<FloorTile> floorTiles, Point point, int commandIndex, int floorTileIndex)
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
        if (_floorTileIndex >= 0)
        {
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
            _floorTiles[_floorTileIndex].Type = FloorTileType.Static;
            _floorTiles[_floorTileIndex].Number = 0;
            _floorTiles[_floorTileIndex].Portal = 0;
            _floorTiles[_floorTileIndex].Spring = SpringDirection.Up;
            _floorTiles[_floorTileIndex].Bomb = 0;
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
        else
        {
            _beforChange = null;
            FloorTile floorTile = new()
            {
                PosX = (int)_point.X,
                PosY = (int)_point.Y,
                Type = FloorTileType.Static,
                Number = 0,
                Portal = 0,
                Spring = SpringDirection.Up,
                Bomb = 0
            };
            _floorTiles.Add(floorTile);
            _afterChange = new FloorTile
            {
                PosX = floorTile.PosX,
                PosY = floorTile.PosY,
                Type = floorTile.Type,
                Number = floorTile.Number,
                Portal = floorTile.Portal,
                Spring = floorTile.Spring,
                Bomb = floorTile.Bomb
            };
            _floorTileIndex = _floorTiles.Count - 1;
        }
    }
    public override void Undo()
    {
        if (_beforChange != null)
        {
            _floorTiles[_floorTileIndex].Type = _beforChange.Type;
            _floorTiles[_floorTileIndex].Number = _beforChange.Number;
            _floorTiles[_floorTileIndex].Portal = _beforChange.Portal;
            _floorTiles[_floorTileIndex].Spring = _beforChange.Spring;
            _floorTiles[_floorTileIndex].Bomb = _beforChange.Bomb;
        }
        else
        {
            _floorTiles.RemoveAt(_floorTileIndex);
        }
    }
    public override void Redo()
    {
        if (_floorTiles.ElementAtOrDefault(_floorTileIndex) != null)
        {
            _floorTiles[_floorTileIndex] = new FloorTile
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
        else
        {
            FloorTile floorTileToInser = new()
            {
                PosX = _afterChange.PosX,
                PosY = _afterChange.PosY,
                Type = _afterChange.Type,
                Number = _afterChange.Number,
                Portal = _afterChange.Portal,
                Spring = _afterChange.Spring,
                Bomb = _afterChange.Bomb
            };
            _floorTiles.Insert(_floorTileIndex, floorTileToInser);
        }
    }
    public override string ToString()
    {
        string returnText;
        if (_beforChange != null)
        {
            returnText = $"{_commandIndex}; Static [{_point.X},{_point.Y}] Number: {_beforChange.Number} -> {_afterChange.Number}, " +
                 $"Portal: {_beforChange.Portal} -> {_afterChange.Portal}, Spring: {_beforChange.Spring} -> {_afterChange.Spring}, " +
                 $"Bomb: {_beforChange.Bomb} -> {_afterChange.Bomb}";
        }
        else
        {
            returnText = $"{_commandIndex}; Static [{_point.X},{_point.Y}] Number: null -> {_afterChange.Number}, " +
                $"Portal: null -> {_afterChange.Portal}, Spring: null -> {_afterChange.Spring}, " +
                $"Bomb: null -> {_afterChange.Bomb}";
        }
        return returnText;
    }
}
