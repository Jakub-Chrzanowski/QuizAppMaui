

using QuizApp.Services;

namespace QuizApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }
        Player player1 = new Player();
        Player player2 = new Player();

        private async void StartGame_Clicked(object sender, EventArgs e)
        {
            string FirstPlayer = FirstPlayerLabel.Text;
            string SecondPlayer = SecondPlayerLabel.Text;
            int QuestionsNumber;
            //int Time;
            if (int.TryParse(QuestionsNumberLabel.Text, out QuestionsNumber))
            {
                if(QuestionsNumber >=10 && QuestionsNumber <= 15)
                {
                    if (FirstPlayer != null && SecondPlayer != null && !(FirstPlayer == SecondPlayer))
                    {
                        //if(int.TryParse(TimeLabel.Text, out Time) && Time>=10)
                        //{
                            player1.name = FirstPlayer;
                            player2.name = SecondPlayer;

                            await Navigation.PushAsync(new QuizPage(player1, player2, QuestionsNumber//, Time
                                ));
                        //}
                        //else
                        //{
                        //    await DisplayAlert("Podaj poprawną długość rundy", "Spróbuj ponownie", "Ok");
                        //}    
                        
                    }
                    else
                    {
                        await DisplayAlert("Podaj poprawne nazwy graczy.", "Spróbuj ponownie", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Podaj poprawną ilość pytań.", "Spróbuj ponownie", "Ok");
                }
            }
            else {
                await DisplayAlert("Podaj poprawną ilość pytań.", "Spróbuj ponownie", "Ok");
            }
        }
    }
}
