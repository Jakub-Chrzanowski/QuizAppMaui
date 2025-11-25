
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
            playerGradientRight.Color = Color.FromArgb("#414141");
            playerGradientRight.Offset = 1;
            playerGradientLeft.Color = Color.FromArgb("#FF0000");
            float v = 0.4;
            playerGradientLeft.Offset = v;
            which = true;
        }
        else
        {
            labelWhichPlayer.Text = "Player 1 is answering";
            playerGradientRight.Color = Color.FromArgb("#00F6FF");
            playerGradientRight.Offset = 0;
            playerGradientLeft.Color = Color.FromArgb("#414141");
            float v = 0.6;
            playerGradientLeft.Offset = v;
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