﻿using SlidingTile_LevelEditor.Class;
using System;
using System.Collections.Generic;
using System.Windows;

namespace SlidingTile_LevelEditor.Commands
{
    class SpringDecCommand : Command
    {
        private List<Command> _commands;
        private Point _point;
        private int _commandIndex;
        private List<FloorTile> _floorTiles;
        private FloorTile _beforChange;
        private FloorTile _afterChange;
        private int _floorTileIndex;
        public SpringDecCommand(List<Command> commands, List<FloorTile> floorTiles, Point point, int commandIndex, int floorTileIndex)
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
                Spring = _floorTiles[_floorTileIndex].Spring
            };
            if (_floorTiles[_floorTileIndex].Number > 1)
            {
                _floorTiles[_floorTileIndex].Type = FloorTileType.Spring;
                _floorTiles[_floorTileIndex].Number--;
                _floorTiles[_floorTileIndex].Portal = 0;
                _afterChange = new FloorTile
                {
                    PosX = _floorTiles[_floorTileIndex].PosX,
                    PosY = _floorTiles[_floorTileIndex].PosY,
                    Type = _floorTiles[_floorTileIndex].Type,
                    Number = _floorTiles[_floorTileIndex].Number,
                    Portal = _floorTiles[_floorTileIndex].Portal,
                    Spring = _floorTiles[_floorTileIndex].Spring
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
                _floorTiles[_floorTileIndex].Portal = _beforChange.Portal;
                _floorTiles[_floorTileIndex].Spring = _beforChange.Spring;
            }
            else
            {
                FloorTile floorTileToInser = new FloorTile()
                { 
                    PosX = _beforChange.PosX,
                    PosY = _beforChange.PosY,
                    Type = _beforChange.Type,
                    Number = _beforChange.Number,
                    Portal = _beforChange.Portal,
                    Spring = _beforChange.Spring
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
                    PosX = _afterChange.PosX,
                    PosY = _afterChange.PosY,
                    Type = _afterChange.Type,
                    Number = _afterChange.Number,
                    Portal = _afterChange.Portal,
                    Spring = _afterChange.Spring
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
                returnText = $"{_commandIndex}; Spring Dec [{_point.X},{_point.Y}] Number: {_beforChange.Number} -> {_afterChange.Number}" +
                    $", Portal: {_beforChange.Portal} -> {_afterChange.Portal}, Spring: {_beforChange.Spring} -> {_afterChange.Spring}";
            }
            else
            {
                returnText = $"{_commandIndex}; Spring Dec [{_point.X},{_point.Y}] Number: {_beforChange.Number} -> null" +
                    $", Portal: {_beforChange.Portal} -> null, Spring: {_beforChange.Spring} -> null";
            }
            return returnText;
        }
    }
}
