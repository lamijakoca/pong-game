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
using System.Windows.Threading;

namespace Pong
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //player directions
        private bool P1UP = false;
        private bool P2UP = false;
        private bool P1DOWN = false;
        private bool P2DOWN = false;

        //timer
        private DispatcherTimer playerTimer = new DispatcherTimer();
        private DispatcherTimer animtionTimer = new DispatcherTimer();

        //ball speed
        private double beginSpeedX = 140;
        private double beginSpeedY = 200;
        private double speedX = 140;
        private double speedY = 200;

        private double beginBallX = 200;
        //private double beginBallY = 150;

        //score
        private int score1 = 0;
        private int score2 = 0;

        private bool leftCooldown = false;
        private bool rightCooldown = false;
        public MainWindow()
        {
            InitializeComponent();
            playerTimer.Interval = TimeSpan.FromMilliseconds(30);
            playerTimer.Tick += PlayerMove;
            playerTimer.Start();

            animtionTimer.Interval = TimeSpan.FromSeconds(0.05);
            animtionTimer.Tick += Animate;
            animtionTimer.Start();
        }
        private void PlayerMove(object sender, EventArgs e)
        {
            if (P1UP)
            {
                if(Canvas.GetTop(LeftBorder) > 0.0)
                {
                    Canvas.SetTop(LeftBorder, Canvas.GetTop(LeftBorder) - 10);
                }
            }
            else if (P1DOWN)
            {
                if (Canvas.GetTop(LeftBorder) + LeftBorder.Height < game.ActualHeight)
                {
                    Canvas.SetTop(LeftBorder, Canvas.GetTop(LeftBorder) + 10);
                }
            }
            if (P2UP)
            {
                if (Canvas.GetTop(RightBorder) > 0.0)
                {
                    Canvas.SetTop(RightBorder, Canvas.GetTop(RightBorder) - 10);
                }
            }
            else if (P2DOWN)
            {
                if (Canvas.GetTop(RightBorder) + 100 < game.ActualHeight)
                {
                    Canvas.SetTop(RightBorder, Canvas.GetTop(RightBorder) + 10);
                }
            }
        }
        private void keyUp (object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    {
                        P1UP = false;
                        P1DOWN = false;
                        break;
                    }
                case Key.S:
                    {
                        P1UP = false;
                        P1DOWN = false;
                        break;
                    }
                case Key.Up:
                    {
                        P2DOWN = false;
                        P2UP = false;
                        break;
                    }
                case Key.Down:
                    {
                        P2DOWN = false;
                        P2UP = false;
                        break;
                    }
            }
        }
        private void keyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    {
                        P1UP = true;
                        P1DOWN = false;
                        break;
                    }
                case Key.S:
                    {
                        P1DOWN = true;
                        P1UP = false;
                        break; 
                    }
                case Key.Up:
                    {
                        P2UP = true;
                        P2DOWN = false;
                        break;
                    }
                case Key.Down:
                    {
                        P2DOWN = true;
                        P2UP = false;
                        break;
                    }
            }
        }
        private void Animate(object sender, EventArgs e)
        {
            var x = Canvas.GetLeft(ball);
            var y = Canvas.GetTop(ball);

            if (x <= LeftBorder.Width && y >= Canvas.GetTop(LeftBorder) && y + ball.Height <= Canvas.GetTop(LeftBorder) + LeftBorder.Height && !leftCooldown)
            {
                speedX = -speedX;
                leftCooldown = true;
                rightCooldown = false;
            }
            if (x + ball.Width >= game.ActualWidth - (Canvas.GetRight(RightBorder) + RightBorder.Width) && y + RightBorder.Width >= Canvas.GetTop(RightBorder) + RightBorder.Height && !rightCooldown)
            {
                speedX = -speedX;
                rightCooldown = true;
                leftCooldown = false;
            }

            if(y <= 0.0 || y >= game.ActualHeight - ball.Height)
            {
                speedY = -speedY;
            }

            if (x <= -10 - ball.Width)
            {
                speedX = beginSpeedX;
                speedY = beginSpeedY;
                score2++;
                leftCooldown = false;
                rightCooldown = false;
                Canvas.SetLeft(ball, beginBallX);
                Canvas.SetTop(ball, beginSpeedY);
                second.Content = score1.ToString();
                return;
            }
            if (x >= 10 + game.ActualWidth)
            {
                speedX = beginSpeedX;
                speedY = beginSpeedY;
                score1++;
                leftCooldown = false;
                rightCooldown = false;
                Canvas.SetLeft(ball, beginBallX);
                Canvas.SetTop(ball, beginSpeedY);
                first.Content = score2.ToString();
                return;
            }

            speedX *= 1.0001;
            speedY *= 1.0001;

            //Ball movement
            x += speedX + animtionTimer.Interval.TotalSeconds;
            y += speedY * animtionTimer.Interval.TotalSeconds;
            Canvas.SetLeft(ball, x);
            Canvas.SetTop(ball, y);
        }
    }
}
