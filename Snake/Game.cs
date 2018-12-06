using System;
using System.Drawing;
using System.Linq;
using System.Timers;

namespace Snake
{
    class Game
    {
        private readonly Random _rnd;

        public Snake Snake { get; set; }
        public Point Food { get; set; }
        public Timer Timer { get; set; }

        public delegate void MethodDrawGame();
        public event MethodDrawGame OnDrawGame;

        public const int FieldWidth = 20;
        public const int FieldHeight = 15;

        public Game()
        {
            Snake = new Snake();
            _rnd = new Random();
        }

        public void Start()
        {
            GenerateSnake();
            GenerateFood();
            OnDrawGame();
            Timer = new Timer(1000);
            Timer.Elapsed += OnTimedMoveSnake;
            Timer.Start();
        }

        private void OnTimedMoveSnake(object sender, ElapsedEventArgs e)
        {
            MoveSnake(Snake.NextMovement);
            OnDrawGame();
        }

        private void GenerateSnake()
        {
            Snake.GenerateSnake();
        }

        private void GenerateFood()
        {
            do
            {
                Food = new Point(_rnd.Next(0, FieldWidth), _rnd.Next(0, FieldHeight));
            } while (!CheckGeneratePoint());
        }

        private bool CheckGeneratePoint()
        {
            foreach (var point in Snake.Points)
            {
                if (point == Food 
                    || new Point(point.X - 1, point.Y) == Food
                    || new Point(point.X, point.Y - 1) == Food
                    || new Point(point.X - 1, point.Y - 1) == Food
                    || new Point(point.X + 1, point.Y) == Food
                    || new Point(point.X, point.Y + 1) == Food
                    || new Point(point.X + 1, point.Y + 1) == Food)
                    return false;
            }
            return true;
        }

        public bool CheckFinishGame(Point newPoint)
        {
            if (newPoint.X < 0 || newPoint.X >= FieldWidth || newPoint.Y < 0 || newPoint.Y >= FieldHeight)
                return true;
            if (Snake.Points.Contains(newPoint))
                return true;
            return false;
        }
        
        public void MoveSnake(Movement move)
        {
            var points = Snake.Points;
            var firstPoint = points.First();
            var newPoint = new Point();
            switch (move)
            {
                case Movement.Left:
                    newPoint = new Point(firstPoint.X - 1, firstPoint.Y);
                    break;
                case Movement.Right:
                    newPoint = new Point(firstPoint.X + 1, firstPoint.Y);
                    break;
                case Movement.Up:
                    newPoint = new Point(firstPoint.X, firstPoint.Y - 1);
                    break;
                case Movement.Down:
                    newPoint = new Point(firstPoint.X, firstPoint.Y + 1);
                    break;
            }

            if (CheckFinishGame(newPoint))
            {
                Timer.Stop();
                Environment.Exit(0);
            }

            points.Insert(0, newPoint);
            if (!CheckFindingFood())
                Snake.Points.RemoveAt(points.Count - 1);
            Snake.PreviousMovement = move;
        }

        private bool CheckFindingFood()
        {
            if (Snake.Points.First() != Food)
                return false;
            GenerateFood();
            return true;
        }

        public bool CheckChanceMove(Movement movement)
        {
            switch (movement)
            {
                case Movement.Left:
                    if (Snake.PreviousMovement == Movement.Right)
                        return false;
                    break;
                case Movement.Right:
                    if (Snake.PreviousMovement == Movement.Left)
                        return false;
                    break;
                case Movement.Up:
                    if (Snake.PreviousMovement == Movement.Down)
                        return false;
                    break;
                case Movement.Down:
                    if (Snake.PreviousMovement == Movement.Up)
                        return false;
                    break;
            }
            return true;
        }
    }

    public enum Movement
    {
        Left, Right, Up, Down
    }
}
