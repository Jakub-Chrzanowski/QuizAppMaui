
namespace QuizApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void StartGame_Clicked(object sender, EventArgs e)
        {
            string FirstPlayer = FirstPlayerLabel.Text;
            string SecondPlayer = SecondPlayerLabel.Text;
            int QuestionsNumber;
            if (int.TryParse(QuestionsNumberLabel.Text, out QuestionsNumber))
            {
                if(QuestionsNumber >=10 && QuestionsNumber <= 15)
                {
                    if (FirstPlayer != null && SecondPlayer != null && !(FirstPlayer == SecondPlayer))
                    {
                        await Navigation.PushAsync(new QuizPage());
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
