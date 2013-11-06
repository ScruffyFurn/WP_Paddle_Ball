using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace WP_Pong
{
    public partial class GamePage : PhoneApplicationPage
    {
        private enum GameState
        {
            Start = 1,
            Playing,
            GameOver,
            Paused
        };

        GameState currentGameState;

        DispatcherTimer timer;
        Paddle playerPaddle, cpuPaddle;
        Ball ball;

        double ScreenHeight = Application.Current.RootVisual.RenderSize.Height;
        double ScreenWidth = Application.Current.RootVisual.RenderSize.Width;

        int playerScore = 0;
        int cpuScore = 0;
        int cpuSpeed = 4;
        int xSpeed = 6;
        int ySpeed = 6;

        TranslateTransform ballTransform = new TranslateTransform();
        TranslateTransform cpuPaddleTransform = new TranslateTransform();
        TranslateTransform dragTranslation = new TranslateTransform();
        TranslateTransform playerResetTransform = new TranslateTransform();
        TranslateTransform cpuResetTransform = new TranslateTransform();
        TranslateTransform ballResetTransform = new TranslateTransform();

        TextBlock CPUScoreText = new TextBlock();
        TextBlock PlayerScoreText = new TextBlock();
        TextBlock TitleText = new TextBlock();
        TextBlock PausedText = new TextBlock();
        TextBlock ReadyText = new TextBlock();
        TextBlock GameOverText = new TextBlock();

        public GamePage()
        {
            InitializeComponent();

            //Create and add the background to the screen
            ImageBrush BackgroundImage = new ImageBrush();
            BackgroundImage.ImageSource = new BitmapImage(new Uri(@"\bg.png", UriKind.Relative));
            LayoutRoot.Background = BackgroundImage;

            //Setup text for when the game is paused
            PausedText.Text = "Paused";
            PausedText.TextAlignment = TextAlignment.Center;
            PausedText.Margin = new Thickness(0, 210, 0, 216);
            LayoutRoot.Children.Add(PausedText);

            //Setup the text for start instructions
            ReadyText.Text = "Tap to continue";
            ReadyText.TextAlignment = TextAlignment.Center;
            ReadyText.Margin = new Thickness(0, 210, 0, 216);
            LayoutRoot.Children.Add(ReadyText);

            //Setup the text for when the game is over
            GameOverText.Text = "GameOver!";
            GameOverText.TextAlignment = TextAlignment.Center;
            GameOverText.Margin = new Thickness(0, 210, 0, 216);
            LayoutRoot.Children.Add(GameOverText);

            TitleText.Text = "WP Paddle Ball";
            TitleText.TextAlignment = TextAlignment.Center;
            LayoutRoot.Children.Add(TitleText);

            //Setup the score text for player and cpu
            CPUScoreText.TextAlignment = TextAlignment.Right;
            PlayerScoreText.TextAlignment = TextAlignment.Left;
            LayoutRoot.Children.Add(CPUScoreText);
            LayoutRoot.Children.Add(PlayerScoreText);

            playerPaddle = new Paddle(-200, 200); //Create and set the location of the player paddle
            LayoutRoot.Children.Add(playerPaddle.paddleRect); //Add the paddle to the screen
            cpuPaddle = new Paddle(200, 200); //Create and set the location of the cpu paddle
            LayoutRoot.Children.Add(cpuPaddle.paddleRect); //Add the paddle to the screen

            ball = new Ball(); //Create the ball
            LayoutRoot.Children.Add(ball.ballRect); //Add the ball to screen

            // Create a timer for the game
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromTicks(333333);
            timer.Tick += OnTimerTick;
            timer.Start();

            playerPaddle.paddleRect.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(Drag_ManipulationDelta); //Setup the event handler for the drag input
            dragTranslation.X += -200;
            cpuPaddleTransform.X += 200;

            // Set the current state to 'Start'
            currentGameState = GameState.Start;
            LayoutRoot.Tap += new EventHandler<GestureEventArgs>(Click_Handler);
        }

        private void OnTimerTick(Object sender, EventArgs args)
        {
            Update();
            Draw();
        }

        private void Click_Handler(Object sender, EventArgs args)
        {
            switch (currentGameState)
            {
                case GameState.Start:
                    currentGameState = GameState.Playing;
                    break;
                case GameState.Playing:
                    break;
                case GameState.Paused:
                    currentGameState = GameState.Playing;
                    break;
                case GameState.GameOver:
                    playerScore = 0;
                    cpuScore = 0;
                    currentGameState = GameState.Start;
                    break;
            }
        }

        private void Reset()
        {
            //Reset the player paddle to its starting location
            playerResetTransform.X = -200;
            playerResetTransform.Y = 200;
            dragTranslation = playerResetTransform;

            //Reset the cpu paddle to its starting location
            cpuResetTransform.X = 200;
            cpuPaddleTransform = cpuResetTransform;

            //Reset the ball to its starting location
            ballResetTransform.X = 0;
            ballResetTransform.Y = 0;
            ballTransform = ballResetTransform;
        }

        private void Update()
        {
            switch (currentGameState)
            {
                case GameState.Start:
                    break;
                case GameState.Playing:
                    if (ballTransform.Y < 0)
                    {
                        ySpeed = -ySpeed;
                    }
                    if (ballTransform.Y > ScreenHeight / 2)
                    {
                        ySpeed = -ySpeed;
                    }

                    if (ballTransform.X < -(ScreenWidth / 2))
                    {
                        cpuScore++;
                        Reset();
                        xSpeed = -xSpeed;
                    }
                    if (ballTransform.X > ScreenWidth / 2)
                    {
                        playerScore++;
                        Reset();
                        xSpeed = -xSpeed;
                    }

                    ballTransform.X += xSpeed;
                    ballTransform.Y += ySpeed;
                    ball.ballRect.RenderTransform = this.ballTransform;

                    //Check if player or cpu has reached 10 points
                    //If they have the game is over
                    if (playerScore == 10)
                    {
                        currentGameState = GameState.GameOver;
                    }
                    if (cpuScore == 10)
                    {
                        currentGameState = GameState.GameOver;
                    }

                    if (ballTransform.Y < cpuPaddleTransform.Y)
                    {
                        cpuPaddleTransform.Y -= cpuSpeed;
                        cpuPaddle.paddleRect.RenderTransform = cpuPaddleTransform;
                    }
                    if (ballTransform.Y > cpuPaddleTransform.Y)
                    {
                        cpuPaddleTransform.Y += cpuSpeed;
                        cpuPaddle.paddleRect.RenderTransform = cpuPaddleTransform;
                    }

                    if (Intersect(new Rect(cpuPaddleTransform.X, cpuPaddleTransform.Y, cpuPaddle.paddleRect.Width, cpuPaddle.paddleRect.Height),
                                 new Rect(ballTransform.X, ballTransform.Y, ball.ballRect.Width, ball.ballRect.Height)))
                    {
                        ballTransform.X -= 5;
                        xSpeed = -xSpeed;
                    }

                    if (Intersect(new Rect(dragTranslation.X, dragTranslation.Y, playerPaddle.paddleRect.Width, playerPaddle.paddleRect.Height),
                                 new Rect(ballTransform.X, ballTransform.Y, ball.ballRect.Width, ball.ballRect.Height)))
                    {
                        ballTransform.X += 5;
                        xSpeed = -xSpeed;
                    }
                    break;
                case GameState.Paused:
                    break;
                case GameState.GameOver:
                    break;
            }
        }

        public static bool Intersect(Rect rectangle1, Rect rectangle2)
        {
            rectangle1.Intersect(rectangle2);

            if (rectangle1.IsEmpty)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void Draw()
        {
            switch (currentGameState)
            {
                case GameState.Start:
                    PausedText.Visibility = Visibility.Collapsed;
                    GameOverText.Visibility = Visibility.Collapsed;
                    ReadyText.Visibility = Visibility.Visible;
                    break;
                case GameState.Playing:
                    PausedText.Visibility = Visibility.Collapsed;
                    GameOverText.Visibility = Visibility.Collapsed;
                    ReadyText.Visibility = Visibility.Collapsed;
                    CPUScoreText.Text = "CPU: " + cpuScore.ToString();
                    PlayerScoreText.Text = "Player: " + playerScore.ToString();
                    break;
                case GameState.Paused:
                    PausedText.Visibility = Visibility.Visible;
                    GameOverText.Visibility = Visibility.Collapsed;
                    ReadyText.Visibility = Visibility.Collapsed;
                    break;
                case GameState.GameOver:
                    PausedText.Visibility = Visibility.Collapsed;
                    GameOverText.Visibility = Visibility.Visible;
                    ReadyText.Visibility = Visibility.Collapsed;
                    break;
            }

        }

        void Drag_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            // Move the Paddle.
            dragTranslation.Y += e.DeltaManipulation.Translation.Y;
            playerPaddle.paddleRect.RenderTransform = this.dragTranslation;
        }


    }
}