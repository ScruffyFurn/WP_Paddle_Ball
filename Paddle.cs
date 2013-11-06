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
    public class Paddle
    {
        private ImageBrush paddleBrush;
        public Rectangle paddleRect;
        private TranslateTransform placementTranslation = new TranslateTransform();

        public Paddle(float posX, float posY)
        {
            paddleRect = new Rectangle();
            paddleRect.Width = 30;
            paddleRect.Height = 90;

            placementTranslation.X = posX;
            placementTranslation.Y = posY;

            paddleRect.RenderTransform = placementTranslation;

            paddleBrush = new ImageBrush();
            paddleBrush.ImageSource = new BitmapImage(new Uri(@"\paddle.png", UriKind.Relative)); //Load the paddle image

            paddleRect.SetValue(Canvas.ZIndexProperty, 3);
            paddleBrush.Stretch = Stretch.None;

            paddleBrush.AlignmentY = AlignmentY.Top;
            paddleBrush.AlignmentX = AlignmentX.Left;
            paddleRect.Fill = paddleBrush;
        }
    }
}

