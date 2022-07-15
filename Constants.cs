using System.Windows.Media;

namespace csharp
{
    public static class Constants
    {
        public static int WindowHeight { get; } = 600;
        public static int WindowWidth { get; } = 900;


        // public static int ControlBoxHeight {get; } = 500;
        public static int ControlBoxWidth { get; } = 0;


        public static int Rows { get; } = 45;
        public static int Columns { get; } = 75;


        public static int BotMoveSpeed { get; } = 850;
        public static int BulletSpeed { get; } = 50;


        public static Brush EmptyColor { get; } = Brushes.Gainsboro;
        public static Brush WallColor { get; } = Brushes.Gray;
        public static Brush MyTankColor { get; } = Brushes.Green;
        public static Brush EnemyTankColor { get; } = Brushes.Blue;
        public static Brush[] TankGroupColor { get; } = new Brush[] { Brushes.Green, Brushes.Blue };
    }
}