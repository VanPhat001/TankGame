using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Threading;

namespace csharp
{
    public enum MoveDirection
    {
        Up, Left, Right, Down
    };

    public class Tank
    {
        private MoveDirection moveDirection;
        private Point centerPoint;
        private int group = -1;
        private List<Bullet> bullets = null;
        DispatcherTimer timer = null;
        private Random rand = new Random();


        public static int[,,] tankBody = new int[4, 3, 3]
        {
            {
                {0, 1, 0},
                {1, 1, 1},
                {1, 0, 1}
            },
            {
                {0, 1, 1},
                {1, 1, 0},
                {0, 1, 1}
            },
            {
                {1, 1, 0},
                {0, 1, 1},
                {1, 1, 0}
            },
            {
                {1, 0, 1},
                {1, 1, 1},
                {0, 1, 0}
            }
        };


        public Tank(Map map, Point tankPoint, int groupNumber, bool isBot)
        {
            moveDirection = MoveDirection.Right;
            centerPoint = new Point(tankPoint.X, tankPoint.Y);
            group = groupNumber;
            bullets = new List<Bullet>();

            print(map);
            if (isBot)
            {
                timer = new DispatcherTimer();
                timer.Tag = map;
                timer.Interval = TimeSpan.FromMilliseconds(Constants.BotMoveSpeed);
                timer.Tick += botAction;
                timer.Start();
            }
        }

        public void Stop()
        {
            if (timer != null)
            {
                timer.Stop();
            }
        }

        public void shot(Map map)
        {
            Bullet bullet = null;
            switch (moveDirection)
            {
                case MoveDirection.Up:
                    bullet = new Bullet(map, new Point(centerPoint.X, centerPoint.Y - 2), moveDirection, group);
                    break;
                case MoveDirection.Left:
                    bullet = new Bullet(map, new Point(centerPoint.X - 2, centerPoint.Y), moveDirection, group);
                    break;
                case MoveDirection.Right:
                    bullet = new Bullet(map, new Point(centerPoint.X + 2, centerPoint.Y), moveDirection, group);
                    break;
                case MoveDirection.Down:
                    bullet = new Bullet(map, new Point(centerPoint.X, centerPoint.Y + 2), moveDirection, group);
                    break;
            }

            if (bullet != null)
            {
                bullets.Add(bullet);
                bullet.OutOfRange += DestroyBullet;
            }
        }

        private void DestroyBullet(object sender, EventArgs e)
        {
            Bullet bullet = sender as Bullet;
            bullets.Remove(bullet);
        }

        public void onMove(Map map)
        {
            onMove(map, moveDirection);
        }

        public void onMove(Map map, MoveDirection moveDirection)
        {
            clear(map);

            Point oddPoint = new Point(centerPoint.X, centerPoint.Y);

            setMoveDirection(moveDirection);

            switch (moveDirection)
            {
                case MoveDirection.Up:
                    centerPoint.Y--;
                    break;
                case MoveDirection.Left:
                    centerPoint.X--;
                    break;
                case MoveDirection.Right:
                    centerPoint.X++;
                    break;
                case MoveDirection.Down:
                    centerPoint.Y++;
                    break;
            }

            if (centerPoint.X < 1) centerPoint.X = 1;
            if (centerPoint.Y < 1) centerPoint.Y = 1;
            if (centerPoint.X >= Constants.Columns - 2) centerPoint.X = Constants.Columns - 2;
            if (centerPoint.Y >= Constants.Rows - 2) centerPoint.Y = Constants.Rows - 2;

            if (!map.checkTankMoveIn(convertCenterToLeftTop(centerPoint), moveDirection))
            {
                centerPoint = oddPoint;
            }

            print(map);
        }

        private void botAction(object sender, EventArgs e)
        {
            // move
            if (rand.Next(4) == 1)
            {
                onMove((sender as DispatcherTimer).Tag as Map, (MoveDirection)rand.Next(4));
            }
            else
            {
                onMove((sender as DispatcherTimer).Tag as Map);
            }

            // shot
            if (rand.Next(10) == 1)
            {
                shot((sender as DispatcherTimer).Tag as Map);
            }
        }

        public Point convertCenterToLeftTop(Point center)
        {
            return new Point(center.X - 1, center.Y - 1);
        }

        public Point convertLeftTopToCenter(Point leftTop)
        {
            return new Point(leftTop.X + 1, leftTop.Y + 1);
        }

        public void printAt(Map map, Point point)
        {
            point = convertCenterToLeftTop(point);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (tankBody[(int)moveDirection, i, j] == 1)
                    {
                        map.setTankBody(new Point(point.X + j, point.Y + i), group);
                    }
                }
            }
        }

        public void clearAt(Map map, Point point)
        {
            point = convertCenterToLeftTop(point);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (tankBody[(int)moveDirection, i, j] == 1)
                    {
                        map.clearTankBody(new Point(point.X + j, point.Y + i));
                    }
                }
            }
        }

        public void print(Map map)
        {
            printAt(map, centerPoint);
        }

        public void clear(Map map)
        {
            clearAt(map, centerPoint);
        }

        public void setMoveDirection(MoveDirection moveDirection)
        {
            this.moveDirection = moveDirection;
        }

        public bool checkIn(Point point)
        {
            Point topLeft = convertCenterToLeftTop(centerPoint);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (tankBody[(int)moveDirection, i, j] == 1 && topLeft.X + j == point.X && topLeft.Y + i == point.Y)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}