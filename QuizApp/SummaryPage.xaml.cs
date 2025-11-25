using QuizApp.Services;

namespace QuizApp;

public partial class SummaryPage : ContentPage
{
    Player player1;
    Player player2;
    int totalQuestions;

    public SummaryPage(Player p1, Player p2, int total)
    {
        InitializeComponent();
        player1 = p1;
        player2 = p2;
        totalQuestions = total;

        lblPlayer1Name.Text = player1.name;
        lblPlayer1Score.Text = player1.score.ToString();

        lblPlayer2Name.Text = player2.name;
        lblPlayer2Score.Text = player2.score.ToString();

        lblSummary.Text = $"Odpowiedziano na {totalQuestions} pytañ. Wynik koñcowy: {player1.name} {player1.score} - {player2.score} {player2.name}";
    }

    private async void OnBackToMainClicked(object sender, EventArgs e)
    { 
        await Navigation.PopToRootAsync();
    }
}