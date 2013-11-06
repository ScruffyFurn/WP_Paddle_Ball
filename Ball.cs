using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace WP_Pong
{
    public class Ball
    {
        private ImageBrush ballBrush;
        public Rectangle ballRect;

        public Ball()
        {
            ballRect = new Rectangle();
            ballRect.Width = 30;
            ballRect.Height = 30;

            ballBrush = new ImageBrush();
            ballBrush.ImageSource = new BitmapImage(new Uri(@"\ball.png", UriKind.Relative)); //Load the ball image

            ballRect.SetValue(Canvas.ZIndexProperty, 3);
            ballBrush.Stretch = Stretch.None;

            ballBrush.AlignmentY = AlignmentY.Top;
            ballBrush.AlignmentX = AlignmentX.Left;
            ballRect.Fill = ballBrush;
        }
    }
}

