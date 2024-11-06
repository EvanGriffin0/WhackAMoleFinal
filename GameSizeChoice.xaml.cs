using System.Timers;
using System;
using System.IO;

namespace WhackAMoleFinal;

public partial class GameSizeChoice : ContentPage
{

    //Create variables

    private static System.Timers.Timer gameTimer;
    private System.Timers.Timer userTimer;
    public const int MoleMoveTimeDefault = 5;
    public const int MoleAxisXDefault = 0;
    public const int MoleAxisYDefault = 0;
    public const bool GameStatusDefault = false;
    public const int GameLengthDefault = 10;
    public int SelectedGrid;
    public int MoleAxisX = MoleAxisXDefault;
    public int MoleAxisY = MoleAxisYDefault;
    public int PlayerScore = 0;
    public const int GridThree = 3;
    public const int GridFour = 4;
    public const int GridFive = 5;
    private int GameDuration = GameLengthDefault;
    private int MoleMoveCountdown = MoleMoveTimeDefault;
    private bool IsGameActive = GameStatusDefault;
    public int currentHighestScore ;
    // create int for grid choice
    public  int gridChoice = 0;


    public  GameSizeChoice()
    {

        InitializeComponent();
        updateHighestScore(PlayerScore);

        // create a way to access the menu but verify game is not runnning


            returnToMenu.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    if (IsGameActive)
                    {
                        var result = await DisplayAlert("Exit Confirmation", "Are you sure you want to exit the application? Your Score will not be saved ", "Yes", "No");

                        if (result)
                        {
                            await Navigation.PopAsync();
                        }
                        // You can add an 'else' block here to handle the "No" case if needed.
                    }
                    else
                    {
                        await Navigation.PushAsync(new MainPage());
                    }
                })
            });


        // take users input on board game design
        //3*3
        threeByThreeButton.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() =>
            {
                //change background to start game
                //choose user grid

                BackgroundImageSource = "gamebackground.png";
                createGameGrid(GridThree);
                

            })
        });

        //4*4
        fourByFourButton.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() =>
            {
                //change background to start game
                //choose user grid

                BackgroundImageSource = "gamebackground.png";
                createGameGrid(GridFour);

            })
        });

        //5*5
        fiveByFiveButton.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() =>
            {
                //change background to start game
                //choose user grid
                BackgroundImageSource = "gamebackground.png";
                createGameGrid(GridFive);


            })
        });
    }


    public void createGameGrid(int gridChoice)
    {

        // use a switch statement to select between the Grids

        // option 1
        switch(gridChoice)
        {
            case GridThree:
                SelectedGrid = GridThree;
                for (int x = 0; x < 3; x++)
                {
                    GameGrid.RowDefinitions.Add(new RowDefinition { Height = 100 });
                }
                for (int x = 0; x < 3; x++)
                {
                    GameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 100 });
                }
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        int localX = x;
                        int localY = y;

                        var image = new Image
                        {
                            Source = ImageSource.FromFile("pipe.png"),
                            WidthRequest = 100,
                            HeightRequest = 100,
                        };

                       image.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { moleHasBeenWhacked(localX, localY); }) });

                        Grid.SetRow(image, x);
                        Grid.SetColumn(image, y);

                        GameGrid.Children.Add(image);
                    }
                }
               loopGame(gridChoice);
                break;

                // option 2
            case GridFour:
                SelectedGrid = GridFour;
                for (int x = 0; x < 4; x++)
                {
                    GameGrid.RowDefinitions.Add(new RowDefinition { Height = 100 });
                }
                for (int x = 0; x < 4; x++)
                {
                    GameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 100 });
                }
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        int localX = x;
                        int localY = y;

                        var image = new Image
                        {
                            Source = ImageSource.FromFile("pipe.png"),
                            WidthRequest = 100,
                            HeightRequest = 100,
                        };

                     image.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { moleHasBeenWhacked(localX, localY); }) });

                        Grid.SetRow(image, x);
                        Grid.SetColumn(image, y);

                        GameGrid.Children.Add(image);
                    }
                }
              loopGame(gridChoice);
                break;

                // option 3
            case GridFive:
                SelectedGrid = GridFive;
                for (int x = 0; x < 5; x++)
                {
                   GameGrid.RowDefinitions.Add(new RowDefinition { Height = 100 });
                }
                for (int x = 0; x < 5; x++)
                {
                  GameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 100 });
                }
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 5; y++)
                    {
                        int localX = x;
                        int localY = y;

                        var image = new Image
                        {
                            Source = ImageSource.FromFile("pipe.png"),
                            WidthRequest = 100,
                            HeightRequest = 100,
                        };

                       image.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { moleHasBeenWhacked(localX, localY); }) });

                        Grid.SetRow(image, x);
                        Grid.SetColumn(image, y);
                        GameGrid.Children.Add(image);
                    }
                }
               loopGame(gridChoice);
                break;
            }

        // hide previous options
        threeByThreeButton.IsVisible = false;
        fourByFourButton.IsVisible = false;
        fiveByFiveButton.IsVisible = false;

        gameTimer = new System.Timers.Timer(1000);
        gameTimer.Elapsed += Timer_Started;

        if (GameStatusDefault)
        {
            gameTimer.Stop();
            IsGameActive = false;

        }
        else
        {
            gameTimer.Start();

            IsGameActive = true;
        }

    }

    // create timer 
    private async void Timer_Started(object sender, ElapsedEventArgs e)
    {
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            if (GameDuration > 0)
            {
                GameDuration--;
                gameTime.Text = "Countdown: " + GameDuration;
                MoleMoveCountdown--;
                if (MoleMoveCountdown <= 0)
                {
                    retileGrid();
                    loopGame(SelectedGrid);
                }
            }
            else
            {
                // Countdown has finished
                gameTimer.Stop();
                IsGameActive = false;
                gameTime.Text = "Game Over";
                updateHighestScore(PlayerScore);


            }
        });
    }

    //if mole is hit
    public void moleHasBeenWhacked(int localX, int localY)
    {
        if (localX == MoleAxisX && localY == MoleAxisY)
        {
            retileGrid();
            loopGame(SelectedGrid);
            PlayerScore++;
            playerScoreText.Text = "Score: " + PlayerScore;
            
        }
    }

    // re load game grid
    public void retileGrid()
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                foreach (View view in GameGrid.Children)
                {
                    if (Grid.GetRow(view) == x && Grid.GetColumn(view) == y && view is Image)
                    {
                        // Check if the view is an Image in the specified cell
                        ((Image)view).Source = ImageSource.FromFile("pipe.png");
                        break; // Assuming you want to update only one cell; you can remove the break if you want to update multiple cells
                    }
                }
            }
        }
        var targetImage = GameGrid.Children.OfType<Image>()
    .FirstOrDefault(image => Grid.GetRow(image) == MoleAxisX && Grid.GetColumn(image) == MoleAxisY);

        if (targetImage != null)
        {
            // Change the source of the target image
            targetImage.Source = ImageSource.FromFile("pipe.png");
        }
    }

    // create the mole character
    public async void spawnMole(int x, int y)
    {
        var targetImage = GameGrid.Children.OfType<Image>()
               .FirstOrDefault(image =>
               Grid.GetRow(image) == x && Grid.GetColumn(image) == y);
               
        if (targetImage != null)
        {
            // Change the source of the target image
            targetImage.Source = ImageSource.FromFile("mole.png");
        }
    }


    //moving mole around the board
    private void loopGame(int type)
    {
        MoleMoveCountdown = 5;

        Random random = new Random();
        int x = 0;
        int y = 0;

        switch (type)
        {
            case GridThree:

                SelectedGrid = GridThree;
                x = random.Next(3);
                y = random.Next(3);
                while (x == MoleAxisX && y == MoleAxisY)

                {
                    x = random.Next(3);
                    y = random.Next(3);
                }

                MoleAxisX = x;
                MoleAxisY = y;
                spawnMole(x, y);

                break;

            case GridFour:

                SelectedGrid = GridFour;
                x = random.Next(4);
                y = random.Next(4);
                while (x == MoleAxisX && y == MoleAxisY)

                {
                    x = random.Next(4);
                    y = random.Next(4);
                }

                MoleAxisX = x;
                MoleAxisY = y;
                spawnMole(x, y);

                break;

            case GridFive:
                SelectedGrid = GridFive;
                x = random.Next(3);
                y = random.Next(3);

                while (x == MoleAxisX && y == MoleAxisY)
                {
                    x = random.Next(3);
                    y = random.Next(3);
                }

                MoleAxisX = x;
                MoleAxisY = y;
                spawnMole(x, y);

                break;
        }
    }


    // updating users higher score and assigning it to the xaml tag
    public void updateHighestScore(int score)
    {
        if( score >= currentHighestScore )
        {
            currentHighestScore = score;

            HighScore.Text = "Highest Score :" + currentHighestScore.ToString();
        }
        else
        {
            
        }
    }


}
