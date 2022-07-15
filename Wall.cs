using System.Drawing;

namespace csharp
{
    public class Wall
    {
        private Point locate;
        public Wall(Map map, Point wallPoint)
        {
            locate = new Point(wallPoint.X, wallPoint.Y);
            map.setWallAt(locate);
        }        
    }
}