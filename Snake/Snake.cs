using System.Collections.Generic;
using System.Drawing;

namespace Snake
{
    class Snake
    {
        public List<Point> Points { get; set; } = new List<Point>();
        public Movement NextMovement { get; set; } = Movement.Right;
        public Movement PreviousMovement { get; set; }


        public const int StartLength = 4;

        public void GenerateSnake()
        {
            for (int i = 0; i < StartLength; i++)
            {
                Points.Add(new Point(StartLength - 1 - i, 0));
            }
        }  
    }
}
