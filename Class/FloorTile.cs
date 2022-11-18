namespace SlidingTile_LevelEditor.Class
{
    public enum FloorTileType { None, Finish, Normal, Ice, Solid };
    public class FloorTile
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public FloorTileType Type { get; set; }
        public int Number { get; set; }
        public override string ToString()
        {
            return "[" + PosX.ToString() + "," + PosY.ToString() + "], Number: " + Number.ToString() + ", Type: " + Type.ToString();
        }
    }
}
