using System;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class FormSnake : Form
    {
        private readonly Game _game;
        private Bitmap _bitmap;
        private readonly Graphics _graphics;
        private volatile bool isStopped;

        public delegate void MethodShowResults(int scores);
        public event MethodShowResults OnShowResults;

        public const int SquareSide = 30;

        public FormSnake()
        {
            InitializeComponent();
            _bitmap = new Bitmap(pictureBoxGame.Width, pictureBoxGame.Height);
            _graphics = Graphics.FromImage(_bitmap);
            _game = new Game();
            _game.OnDrawGame += DrawGame;
            _game.OnFinishGame += FinishGame;
            _game.Start();
        }

        private void DrawGame()
        {
            if (isStopped)
                return;
            if (InvokeRequired)
            {
                Invoke((Action)DrawGame);
                return;
            }

            DrawField();
            DrawFood();
            DrawSnake();
            pictureBoxGame.Image = _bitmap;
        }

        private void DrawField()
        {
            _graphics.Clear(Color.Black);
            for (int i = 0; i < Game.FieldHeight; i++)
            {
                for (int j = 0; j < Game.FieldWidth; j++)
                {
                    _graphics.DrawRectangle(new Pen(Brushes.White), new Rectangle(SquareSide * j, SquareSide * i, SquareSide, SquareSide));
                }
            }
        }

        private void DrawSnake()
        {
            var points = _game.Snake.Points;
            for (int i = 0; i < points.Count; i++)
            {
                _graphics.FillRectangle(i == 0 ? Brushes.Green : Brushes.White,
                    new Rectangle(points[i].X * SquareSide, points[i].Y * SquareSide, SquareSide, SquareSide));
                _graphics.DrawRectangle(new Pen(Brushes.Green), new Rectangle(points[i].X * SquareSide, points[i].Y * SquareSide, SquareSide, SquareSide));
            }
        }

        private void DrawFood()
        {
            _graphics.FillRectangle(Brushes.Green, new Rectangle(_game.Food.X * SquareSide, _game.Food.Y * SquareSide, SquareSide, SquareSide));
        }

        private void FormSnake_KeyDown(object sender, KeyEventArgs e)
        {
            Movement movement;
            switch (e.KeyCode)
            {
                case Keys.Left:
                    movement = Movement.Left;
                    break;
                case Keys.Right:
                    movement = Movement.Right;
                    break;
                case Keys.Up:
                    movement = Movement.Up;
                    break;
                case Keys.Down:
                    movement = Movement.Down;
                    break;
                default:
                    movement = _game.Snake.PreviousMovement;
                    break;
            }

            if (_game.CheckChanceMove(movement))
                _game.Snake.NextMovement = movement;
        }

        private void FinishGame(int scores)
        {
            if (InvokeRequired)
            {
                Invoke((Action<int>)FinishGame, scores);
                return;
            }
            Close();
            OnShowResults(scores);
        }

        private void FormSnake_FormClosing(object sender, FormClosingEventArgs e)
        {
            _game.Timer.Stop();
            isStopped = true;
            OnShowResults(_game.Scores);
        }
    }
}
