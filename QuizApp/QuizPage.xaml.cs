
namespace QuizApp;

public partial class QuizPage : ContentPage
{
    bool which = false;
    public QuizPage()
    {
        InitializeComponent();
    }
    public QuizPage(Player player1,Player player2,int questionsNumber)
	{
        InitializeComponent();
        labelPlayer1.Text = player1.name;
        labelPlayer2.Text = player2.name;
        scorePlayer1.Text = player1.score.ToString();
        scorePlayer2.Text = player2.score.ToString();
    }
    private void NextRound()
    {
        if(!which)
        {
            labelWhichPlayer.Text = "Player 2 is answering";
            playerGradient.Color = Color.FromArgb("#FF0000");
            which = true;
        }
        else
        {
            labelWhichPlayer.Text = "Player 1 is answering";
            playerGradient.Color = Color.FromArgb("#00F6FF");
            which = false;
        }
    }
    public void AnswerClicked(object sender,EventArgs e)
    {
        //if (!which)
        //{
        //    if ()
        //    {
        //        player1.score += 1;
        //    }
        //}
        //else
        //{
        //    if ()
        //    {
        //        player2.score += 1;
        //    }
        //}
        NextRound();
    }
}