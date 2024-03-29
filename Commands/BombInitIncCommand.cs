﻿using SlidingTile_LevelEditor.Class;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SlidingTile_LevelEditor.Commands;

public class BombInitIncCommand : Command
{
    private readonly List<Command> _commands;
    private Point _point;
    private readonly int _commandIndex;
    private readonly List<FloorTile> _floorTiles;
    private FloorTile? _beforChange;
    private FloorTile? _afterChange;
    private int _floorTileIndex;
    public BombInitIncCommand(List<Command> commands, List<FloorTile> floorTiles, Point point, int commandIndex, int floorTileIndex)
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
            if (_floorTiles[_floorTileIndex].Bomb != -1)
            {
                _floorTiles[_floorTileIndex].Type = FloorTileType.BombInit;
                _floorTiles[_floorTileIndex].Number = 1;
                _floorTiles[_floorTileIndex].Portal = 0;
                _floorTiles[_floorTileIndex].Spring = SpringDirection.Up;
                _floorTiles[_floorTileIndex].Bomb++;
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
                _floorTiles.RemoveAt(_floorTileIndex);
                _afterChange = null;
            }
        }
        else
        {
            _beforChange = null;
            FloorTile? floorTile = new()
            {
                PosX = (int)_point.X,
                PosY = (int)_point.Y,
                Type = FloorTileType.BombInit,
                Number = 1,
                Portal = 0,
                Spring = SpringDirection.Up,
                Bomb = 1
            };
            _floorTiles.Add(floorTile);
            _afterChange = new FloorTile()
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
            if (_floorTiles.ElementAtOrDefault(_floorTileIndex) != null)
            {
                _floorTiles[_floorTileIndex] = new FloorTile
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
            else
            {
                FloorTile? floorTileToInser = new()
                {
                    PosX = _beforChange.PosX,
                    PosY = _beforChange.PosY,
                    Type = _beforChange.Type,
                    Number = _beforChange.Number,
                    Portal = _beforChange.Portal,
                    Spring = _beforChange.Spring,
                    Bomb = _beforChange.Bomb
                };
                _floorTiles.Insert(_floorTileIndex, floorTileToInser);
            }
        }
        else
        {
            _floorTiles.RemoveAt(_floorTileIndex);
        }
    }
    public override void Redo()
    {
        if (_afterChange != null)
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
                FloorTile? floorTileToInser = new()
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
        else
        {
            _floorTiles.RemoveAt(_floorTileIndex);
        }
    }
    public override string ToString()
    {
        string returnText = string.Empty;
        if (_beforChange != null && _afterChange != null)
        {
            returnText = $"{_commandIndex}; Bomb Init Inc [{_point.X},{_point.Y}] Number: {_beforChange.Number} -> {_afterChange.Number}, " +
                 $"Portal: {_beforChange.Portal} -> {_afterChange.Portal}, Spring: {_beforChange.Spring} -> {_afterChange.Spring}, " +
                 $"Bomb: {_beforChange.Bomb} -> {_afterChange.Bomb}";
        }
        else if(_beforChange == null && _afterChange != null)
        {
            returnText = $"{_commandIndex}; Bomb Init Inc [{_point.X},{_point.Y}] Number: null -> {_afterChange.Number}, " +
                $"Portal: null -> {_afterChange.Portal}, Spring: null -> {_afterChange.Spring}, Bomb: null -> {_afterChange.Bomb}";
        }
        else if (_beforChange != null && _afterChange == null)
        {
            returnText = $"{_commandIndex}; Bomb Init Inc [{_point.X},{_point.Y}] Number: {_beforChange.Number} -> null, " +
               $"Portal: {_beforChange.Portal} -> null, Spring: {_beforChange.Spring} -> null, Bomb: {_beforChange.Bomb} -> null";
        }
        return returnText;
    }
}
