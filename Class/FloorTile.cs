﻿namespace SlidingTile_LevelEditor.Class
{
    public enum FloorTileType { None, Finish, Normal, Ice, Static, Portal };
    public class FloorTile
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public FloorTileType Type { get; set; }
        public int Number { get; set; }
        public int Portal { get; set; }
        public override string ToString()
        {
            return "[" + PosX.ToString() + "," + PosY.ToString() + ", Type: " + Type.ToString() +
                "], Number: " + Number.ToString() + "], Portal: " + Portal.ToString();
        }
    }
}
