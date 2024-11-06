namespace WhackAMoleFinal;

public partial class LeaderBoard : ContentPage
{
	public LeaderBoard()
	{
		InitializeComponent();


        returnToMenu.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() =>
            {
                Navigation.PushAsync(new MainPage());
            })
        });

    }
}