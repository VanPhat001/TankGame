using System;
using System.Drawing;
using System.Windows.Threading;

namespace csharp
{
    public class Bullet
    {
        private Point locate;
        private MoveDirection moveDirection;
        private int group = -1;
        private bool isOutOfRange;
        private event EventHandler outOfRange;


        public Bullet(Map map, Point bulletPoint, MoveDirection moveDirection, int group)
        {
            locate = new Point(bulletPoint.X, bulletPoint.Y);
            this.moveDirection = moveDirection;
            this.group = group;
            IsOutOfRange = false;

            if (Map.checkIn(locate))
            {
                if (impactProcess(map)) return;

                print(map);
                System.Threading.Thread.Sleep(Constants.BulletSpeed);

                DispatcherTimer timer = new DispatcherTimer();
                timer.Tag = map;
                timer.Interval = TimeSpan.FromMilliseconds(Constants.BulletSpeed);
                timer.Tick += bulletMove;
                timer.Start();
            }
            else
            {
                isOutOfRange = true;
            }
        }


        /// <summary>
        /// check impact between bullet and object
        /// </summary>
        /// <param name="map"></param>
        /// <param name="point"></param>
        /// <returns>null if no impact, -1 if impact wall, 1 if impact tank </returns>
        private int? checkImpact(Map map, Point point)
        {
            int group = map.getTankGroup(point);

            if (group != -1 && this.group != group)
            {
                return 1;
            }

            if (map.isWallAt(point))
            {
                return -1;
            }

            return null;
        }

        private void bulletMove(object sender, EventArgs e)
        {
            if (!onMove((sender as DispatcherTimer).Tag as Map))
            {
                (sender as DispatcherTimer).Stop();
            }
        }

        /// <summary>
        /// move bullet 
        /// </summary>
        /// <param name="map"></param>
        /// <returns>true if can move, false if out of range "remove bullet"</returns>
        public bool onMove(Map map)
        {
            clear(map);

            switch (moveDirection)
            {
                case MoveDirection.Up:
                    locate.Y--;
                    break;
                case MoveDirection.Left:
                    locate.X--;
                    break;
                case MoveDirection.Right:
                    locate.X++;
                    break;
                case MoveDirection.Down:
                    locate.Y++;
                    break;
            }

            if (!Map.checkIn(locate))
            {
                IsOutOfRange = true;
                return false;
            }

            if (impactProcess(map))
            {
                return false;
            }

            print(map);
            return true;
        }

        public bool impactProcess(Map map)
        {
            int? isImpact = checkImpact(map, locate);
            if (isImpact == 1)
            {
                bool isEnemyTankImpact = false;

                // find enemy tank, which impact with bullet
                for (int i = 0; i < MainWindow.instance.EnemyTanks.Count; i++)
                {
                    if (MainWindow.instance.EnemyTanks[i].checkIn(locate))
                    {
                        MainWindow.instance.EnemyTanks[i].Stop();
                        MainWindow.instance.EnemyTanks[i].clear(map);
                        MainWindow.instance.EnemyTanks.RemoveAt(i);
                        if (MainWindow.instance.EnemyTanks.Count == 0)
                        {
                            MainWindow.instance.GameFinish();
                        }
                        isEnemyTankImpact = true;
                        break;
                    }
                }

                // no enemy tank impact
                // ==> mytank impact ==> endgame
                if (!isEnemyTankImpact)
                {

                    for (int i = 0; i < MainWindow.instance.EnemyTanks.Count; i++)
                    {
                        MainWindow.instance.EnemyTanks[i].Stop();
                    }
                    MainWindow.instance.GameFinish();
                }

                isOutOfRange = true;
                return true;
            }
            else if (isImpact == -1)
            {
                map.setEmptyAt(locate);

                isOutOfRange = true;
                return true;
            }

            return false;
        }

        public void clear(Map map)
        {
            map.setEmptyAt(locate);
        }

        public void print(Map map)
        {
            map.setBulletAt(locate, group);
        }

        public bool IsOutOfRange
        {
            get => isOutOfRange;
            set
            {
                isOutOfRange = value;
                if (value == true)
                {
                    onOutOfRange();
                }
            }
        }

        public event EventHandler OutOfRange
        {
            add
            {
                outOfRange += value;
            }
            remove
            {
                outOfRange -= value;
            }
        }

        private void onOutOfRange()
        {
            if (outOfRange != null)
            {
                outOfRange(this, new EventArgs());
            }
        }

    }
}