using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace csharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow instance = null;

        private Map map = null;
        private Tank myTank = null;
        private List<Tank> enemyTanks = null;

        private static Random rand = new Random();

        public MainWindow()
        {
            instance = this;
            InitializeComponent();

            this.Loaded += WindowLoaded;
            this.KeyDown += WindowKeyDown;
        }

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    myTank.onMove(map, MoveDirection.Left);
                    break;
                case Key.Up:
                    myTank.onMove(map, MoveDirection.Up);
                    break;
                case Key.Right:
                    myTank.onMove(map, MoveDirection.Right);
                    break;
                case Key.Down:
                    myTank.onMove(map, MoveDirection.Down);
                    break;
                case Key.Space:
                    // todo: shot
                    myTank.shot(map);
                    break;
                case Key.Escape:
                    this.Close();
                    break;
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.Width = Constants.WindowWidth;
            this.Height = Constants.WindowHeight;

            controlBox.Width = Constants.ControlBoxWidth;

            map = new Map();
            map.initMap(gridMain, Constants.Rows, Constants.Columns);

            enemyTanks = new List<Tank>();
            for (int i = 0; i < 5; i++)
            {
                int x = rand.Next(1, Constants.Columns - 2);
                int y = rand.Next(1, Constants.Rows - 2);
                Tank tank = new Tank(map, new System.Drawing.Point(x, y), 0, true);

                enemyTanks.Add(tank);
            }

            myTank = new Tank(map, new System.Drawing.Point(1, 1), 1, false);
        }

        public void GameFinish()
        {
            MessageBox.Show("End");
            this.Close();
        }


        public List<Tank> EnemyTanks
        {
            get => enemyTanks;
            set => enemyTanks = value;
        }
    }
}
