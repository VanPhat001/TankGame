using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;

namespace csharp
{
    public enum ObjectDigits
    {
        EmptyCell, TankGroup0, TankGroup1, BulletGroup0, BulletGroup1, WallCell
    }

    public class Map
    {
        private TextBlock[,] textBlocks = null;
        public Map()
        {
            textBlocks = null;
        }

        public void initWall()
        {
            Random rand = new Random();

            int n = 5 + rand.Next(6);
            for (int i = 0; i < n; i++)
            {
                int numberWall = 8 + rand.Next(8);
                bool isRight = rand.Next(2) == 1;

                Point point = new Point(rand.Next(Constants.Columns), rand.Next(Constants.Rows));

                for (int j = 0; j < numberWall; j++)
                {
                    if (checkIn(point))
                    {
                        setWallAt(point);
                    }

                    if (isRight)
                    {
                        point.X++;
                    }
                    else
                    {
                        point.Y++;
                    }
                }

            }
        }

        public void initMap(Grid gridMain, int rows, int columns)
        {
            for (int i = 0; i < rows; i++)
            {
                gridMain.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < columns; i++)
            {
                gridMain.ColumnDefinitions.Add(new ColumnDefinition());
            }

            textBlocks = new TextBlock[rows, columns];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    TextBlock txbl = new TextBlock();

                    Grid.SetRow(txbl, r);
                    Grid.SetColumn(txbl, c);

                    gridMain.Children.Add(txbl);
                    textBlocks[r, c] = txbl;

                    setEmptyAt(new Point(c, r));
                }
            }

            initWall();
        }

        public static bool checkIn(Point point)
        {
            return 0 <= point.X && point.X < Constants.Columns && 0 <= point.Y && point.Y < Constants.Rows;
        }

        public void setEmptyAt(Point point)
        {
            textBlocks[point.Y, point.X].Background = Constants.EmptyColor;
            textBlocks[point.Y, point.X].Tag = ObjectDigits.EmptyCell;
        }

        public void setTankBody(Point point, int group)
        {
            TextBlock txbk = textBlocks[point.Y, point.X];
            txbk.Background = Constants.TankGroupColor[group];

            switch (group)
            {
                case 0:
                    txbk.Tag = ObjectDigits.TankGroup0;
                    break;
                case 1:
                    txbk.Tag = ObjectDigits.TankGroup1;
                    break;
            }
        }

        public void clearTankBody(Point point)
        {
            // textBlocks[point.Y, point.X].Background = Constants.EmptyColor;
            setEmptyAt(point);
        }

        public void setBulletAt(Point point, int group)
        {
            TextBlock txbk = textBlocks[point.Y, point.X];
            txbk.Background = Constants.TankGroupColor[group];

            switch (group)
            {
                case 0:
                    txbk.Tag = ObjectDigits.BulletGroup0;
                    break;
                case 1:
                    txbk.Tag = ObjectDigits.BulletGroup1;
                    break;
            }
        }

        public bool isTankAt(Point point)
        {
            ObjectDigits digits = (ObjectDigits)textBlocks[point.Y, point.X].Tag;
            return digits == ObjectDigits.TankGroup0 || digits == ObjectDigits.TankGroup1;
        }

        public bool checkTankMoveIn(Point topLeftPoint, MoveDirection moveDirection)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Tank.tankBody[(int)moveDirection, i, j] == 1 && isWallAt(new Point(topLeftPoint.X + j, topLeftPoint.Y + i)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public int getTankGroup(Point point)
        {
            ObjectDigits digits = (ObjectDigits)textBlocks[point.Y, point.X].Tag;
            switch (digits)
            {
                case ObjectDigits.TankGroup0:
                    return 0;
                case ObjectDigits.TankGroup1:
                    return 1;
            }
            return -1;
        }

        public void setWallAt(Point point)
        {
            textBlocks[point.Y, point.X].Background = Constants.WallColor;
            textBlocks[point.Y, point.X].Tag = ObjectDigits.WallCell;
        }

        public bool isWallAt(Point point)
        {
            ObjectDigits digits = (ObjectDigits)textBlocks[point.Y, point.X].Tag;
            return digits == ObjectDigits.WallCell;
        }
    }
}