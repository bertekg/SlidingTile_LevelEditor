using System.Windows.Input;

namespace SlidingTile_LevelEditor
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Exit = new RoutedUICommand
            ("Exit", "Exit", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Q, ModifierKeys.Alt)
            });

        public static readonly RoutedUICommand AboutProgram = new RoutedUICommand
            ("About Program", "AboutProgram", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F1, ModifierKeys.Alt)
            });

        public static readonly RoutedUICommand MoveViewUp = new RoutedUICommand
            ("Move View Up", "MoveViewUp", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad8)
            });

        public static readonly RoutedUICommand MoveViewDown = new RoutedUICommand
            ("Move View Down", "MoveViewDown", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad2)
            });

        public static readonly RoutedUICommand MoveViewLeft = new RoutedUICommand
            ("Move View Left", "MoveViewLeft", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad4)
            });

        public static readonly RoutedUICommand MoveViewRight = new RoutedUICommand
            ("Move View Right", "MoveViewRight", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad6)
            });

        public static readonly RoutedUICommand MoveViewLeftUp = new RoutedUICommand
            ("Move View Left Up", "MoveViewLeftUp", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad7)
            });

        public static readonly RoutedUICommand MoveViewRightUp = new RoutedUICommand
            ("Move View Right Up", "MoveViewRightUp", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad9)
            });

        public static readonly RoutedUICommand MoveViewLeftDown = new RoutedUICommand
            ("Move View Left Down", "MoveViewLeftDown", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad1)
            });

        public static readonly RoutedUICommand MoveViewRightDown = new RoutedUICommand
            ("Move View Right Down", "MoveViewRightDown", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad3)
            });

        public static readonly RoutedUICommand AdjustViewProject = new RoutedUICommand
           ("Adjust View Project", "AjustViewProject", typeof(CustomCommands),
           new InputGestureCollection()
           {
                new KeyGesture(Key.NumPad5)
           });

        public static readonly RoutedUICommand ZoomOutView = new RoutedUICommand
           ("Zoom Out View", "ZoomOutView", typeof(CustomCommands),
           new InputGestureCollection()
           {
                new KeyGesture(Key.Subtract)
           });

        public static readonly RoutedUICommand ZoomInView = new RoutedUICommand
          ("Zoom In View", "ZoomInView", typeof(CustomCommands),
          new InputGestureCollection()
          {
                new KeyGesture(Key.Add)
          });
        public static readonly RoutedUICommand EditModeNormalInc = new RoutedUICommand
            ("Edit Mode Normal Inc", "EditModeNormalInc", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D1, ModifierKeys.Alt)
            });
        public static readonly RoutedUICommand EditModeNormalDec = new RoutedUICommand
            ("Edit Mode Normal Dec", "EditModeNormalDec", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D2, ModifierKeys.Alt)
            });
    }
}
