using System.Windows.Input;

namespace SlidingTile_LevelEditor
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Exit = new RoutedUICommand
            ("Exit", "Exit", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Q, ModifierKeys.Control)
            });
        public static readonly RoutedUICommand DeleteAllTiles = new RoutedUICommand
           ("Delete All Tiles", "DeleteAllTiles", typeof(CustomCommands),
           new InputGestureCollection()
           {
                new KeyGesture(Key.Delete, ModifierKeys.Control)
           });
        public static readonly RoutedUICommand Check = new RoutedUICommand
           ("Check Level", "CheckLevel", typeof(CustomCommands),
           new InputGestureCollection()
           {
                new KeyGesture(Key.T, ModifierKeys.Control)
           });
        public static readonly RoutedUICommand ProjectResults = new RoutedUICommand
           ("Project Results", "ProjectResults", typeof(CustomCommands),
           new InputGestureCollection()
           {
                new KeyGesture(Key.R, ModifierKeys.Control)
           });
        public static readonly RoutedUICommand Options = new RoutedUICommand
           ("Options", "Options", typeof(CustomCommands),
           new InputGestureCollection()
           {
                new KeyGesture(Key.O, ModifierKeys.Alt)
           });
        public static readonly RoutedUICommand AboutProgram = new RoutedUICommand
            ("About Program", "AboutProgram", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F1)
            });
        public static readonly RoutedUICommand MoveViewUp = new RoutedUICommand
            ("Move View Up", "MoveViewUp", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad8, ModifierKeys.Control)
            });
        public static readonly RoutedUICommand MoveViewDown = new RoutedUICommand
            ("Move View Down", "MoveViewDown", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad2, ModifierKeys.Control)
            });
        public static readonly RoutedUICommand MoveViewLeft = new RoutedUICommand
            ("Move View Left", "MoveViewLeft", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad4, ModifierKeys.Control)
            });
        public static readonly RoutedUICommand MoveViewRight = new RoutedUICommand
            ("Move View Right", "MoveViewRight", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad6, ModifierKeys.Control)
            });
        public static readonly RoutedUICommand MoveViewLeftUp = new RoutedUICommand
            ("Move View Left Up", "MoveViewLeftUp", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad7, ModifierKeys.Control)
            });
        public static readonly RoutedUICommand MoveViewRightUp = new RoutedUICommand
            ("Move View Right Up", "MoveViewRightUp", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad9, ModifierKeys.Control)
            });
        public static readonly RoutedUICommand MoveViewLeftDown = new RoutedUICommand
            ("Move View Left Down", "MoveViewLeftDown", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad1, ModifierKeys.Control)
            });
        public static readonly RoutedUICommand MoveViewRightDown = new RoutedUICommand
            ("Move View Right Down", "MoveViewRightDown", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad3, ModifierKeys.Control)
            });
        public static readonly RoutedUICommand FullLevelView = new RoutedUICommand
            ("Full Level View", "FullLevelView", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F, ModifierKeys.Control)
            });
        public static readonly RoutedUICommand ZoomOutView = new RoutedUICommand
            ("Zoom Out View", "ZoomOutView", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Subtract, ModifierKeys.Control)
            });
        public static readonly RoutedUICommand ZoomInView = new RoutedUICommand
            ("Zoom In View", "ZoomInView", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Add, ModifierKeys.Control)
            });
        public static readonly RoutedUICommand EditModeNormalInc = new RoutedUICommand
            ("Edit Mode Normal Inc", "EditModeNormalInc", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D1, ModifierKeys.Control)
            });
        public static readonly RoutedUICommand EditModeNormalDec = new RoutedUICommand
            ("Edit Mode Normal Dec", "EditModeNormalDec", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D2, ModifierKeys.Control)
            });
        public static readonly RoutedUICommand EditModeIceInc = new RoutedUICommand
            ("Edit Mode Ice Inc", "EditModeIceInc", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D3, ModifierKeys.Control)
            });
        public static readonly RoutedUICommand EditModeIceDec = new RoutedUICommand
            ("Edit Mode Ice Dec", "EditModeIceDec", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D4, ModifierKeys.Control)
            });
        public static readonly RoutedUICommand EditModeStatic = new RoutedUICommand
            ("Edit Mode Static", "EditModeStatic", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D5, ModifierKeys.Control)
            });
        public static readonly RoutedUICommand EditModePortalInc = new RoutedUICommand
            ("Edit Mode Portal Inc", "EditModePortalInc", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D6, ModifierKeys.Control)
            });
        public static readonly RoutedUICommand EditModePortalDec = new RoutedUICommand
            ("Edit Mode Portal Dec", "EditModePortalDec", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D7, ModifierKeys.Control)
            });
        public static readonly RoutedUICommand EditModeSpringInc = new RoutedUICommand
           ("Edit Mode Spring Inc", "EditModeSpringInc", typeof(CustomCommands),
           new InputGestureCollection()
           {
                new KeyGesture(Key.D1, ModifierKeys.Alt)
           });
        public static readonly RoutedUICommand EditModeSpringDec = new RoutedUICommand
           ("Edit Mode Spring Dec", "EditModeSpringDec", typeof(CustomCommands),
           new InputGestureCollection()
           {
                new KeyGesture(Key.D2, ModifierKeys.Alt)
           });
        public static readonly RoutedUICommand EditModeSpringLeft = new RoutedUICommand
           ("Edit Mode Spring Left", "EditModeSpringLeft", typeof(CustomCommands),
           new InputGestureCollection()
           {
                new KeyGesture(Key.D3, ModifierKeys.Alt)
           });
        public static readonly RoutedUICommand EditModeSpringRight = new RoutedUICommand
           ("Edit Mode Spring Right", "EditModeSpringRight", typeof(CustomCommands),
           new InputGestureCollection()
           {
                new KeyGesture(Key.D4, ModifierKeys.Alt)
           });
        public static readonly RoutedUICommand EditModeBombInitialInc = new RoutedUICommand
          ("Edit Mode Bomb Initial Inc", "EditModeBombInitialInc", typeof(CustomCommands),
          new InputGestureCollection()
          {
                new KeyGesture(Key.D5, ModifierKeys.Alt)
          });
        public static readonly RoutedUICommand EditModeBombInitialDec = new RoutedUICommand
            ("Edit Mode Bomb Initial Dec", "EditModeBombInitialDec", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D6, ModifierKeys.Alt)
            });
        public static readonly RoutedUICommand EditModeBombModInc = new RoutedUICommand
            ("Edit Mode Bomb Mod Inc", "EditModeBombModInc", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D7, ModifierKeys.Alt)
            });
        public static readonly RoutedUICommand EditModeBombModDec = new RoutedUICommand
            ("Edit Mode Bomb Mod Dec", "EditModeBombModDec", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D8, ModifierKeys.Alt)
            });
        public static readonly RoutedUICommand EditModeFinish = new RoutedUICommand
            ("Edit Mode Finish", "EditModeFinish", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D9, ModifierKeys.Alt)
            });
        public static readonly RoutedUICommand EditModeDeleteTile = new RoutedUICommand
            ("Edit Mode Delete Tile", "EditModeDeleteTile", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D0, ModifierKeys.Alt)
            });
    }
}
