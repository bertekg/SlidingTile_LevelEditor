﻿namespace SlidingTile_LevelEditor.Class
{
    public enum FloorTileType { None, Finish, Normal, Ice, Static, Portal, Spring };
    public enum SpringDirection { Up, Left, Down, Right }
    public class FloorTile
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public FloorTileType Type { get; set; }
        public int Number { get; set; }
        public int Portal { get; set; }
        public SpringDirection Spring { get; set; }
        public override string ToString()
        {
            return $"[{PosX},{PosY}], Type: {Type}, Number: {Number}, Portal: {Portal}, Spring: {Spring}";
        }
    }
}
