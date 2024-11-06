namespace WhackAMoleFinal;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();

		startButton.GestureRecognizers.Add(new TapGestureRecognizer
		{ 
			Command = new Command (() =>
			{
				Navigation.PushAsync(new GameSizeChoice());
			})
		});

        leaderBoardButton.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() =>
            {
                Navigation.PushAsync(new LeaderBoard());
            })
        });
    }
	
	
}

