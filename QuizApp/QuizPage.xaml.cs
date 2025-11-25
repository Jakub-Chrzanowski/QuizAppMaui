using System.Text.Json;
using QuizApp.Services;
using QuizApp.Models;

namespace QuizApp;

public partial class QuizPage : ContentPage
{
    bool which = false;
    Player player1;
    Player player2;
    int requestedQuestionsCount = 10;

    List<Question> allQuestions = new();
    List<Question> quizQuestions = new();
    int currentIndex = 0;
    Random rng = new();

    readonly Color defaultButtonBg = Color.FromArgb("#676565");
    readonly Color correctColor = Color.FromArgb("#00C853");
    readonly Color wrongColor = Color.FromArgb("#D50000");

    //CancellationTokenSource timerCts;
    //int maxTime = 10;
    //int remainingTime;
    //bool answerLocked = false;

    public QuizPage()
    {
        InitializeComponent();
    }

    public QuizPage(Player p1, Player p2, int questionsNumber
        //, int time
        )
    {
        InitializeComponent();
        player1 = p1;
        player2 = p2;
        requestedQuestionsCount = questionsNumber;

        player1.hp = 3;
        player2.hp = 3;
        player1.score = 0;
        player2.score = 0;

        labelPlayer1.Text = player1.name;
        labelPlayer2.Text = player2.name;
        scorePlayer1.Text = player1.score.ToString();
        scorePlayer2.Text = player2.score.ToString();

        labelWhichPlayer.Text = $"Odpowiada {player1.name}";

        //maxTime = time;
        UpdateHearts();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = LoadQuestionsAsync();
    }

    private async Task LoadQuestionsAsync()
    {
        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("questions.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            var loaded = JsonSerializer.Deserialize<List<Question>>(json);
            if (loaded is null || loaded.Count == 0)
            {
                await DisplayAlert("Błąd", "Brak pytań w pliku JSON.", "OK");
                return;
            }

            allQuestions = loaded;
            int take = Math.Min(requestedQuestionsCount, allQuestions.Count);
            quizQuestions = allQuestions.OrderBy(_ => rng.Next()).Take(take).ToList();
            currentIndex = 0;
            DisplayQuestion();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Błąd", $"Nie udało się wczytać pytań: {ex.Message}", "OK");
        }
    }

    //async
    private void DisplayQuestion()
    {
        if (currentIndex < 0 || currentIndex >= quizQuestions.Count)
            return;

        var q = quizQuestions[currentIndex];
        questionCounterLabel.Text = $"Pytanie {currentIndex + 1}/{quizQuestions.Count}";
        questionLabel.Text = q.question;

        var answers = q.answers.Select(a => new { Text = a, IsCorrect = a == q.correct }).OrderBy(_ => rng.Next()).ToList();

        btnA.Text = $"A. {answers[0].Text}";
        btnA.ClassId = answers[0].IsCorrect ? "true" : "false";

        btnB.Text = $"B. {answers[1].Text}";
        btnB.ClassId = answers[1].IsCorrect ? "true" : "false";

        btnC.Text = $"C. {answers[2].Text}";
        btnC.ClassId = answers[2].IsCorrect ? "true" : "false";

        btnD.Text = $"D. {answers[3].Text}";
        btnD.ClassId = answers[3].IsCorrect ? "true" : "false";

        ResetButtonVisuals();
        SetButtonsEnabled(true);


        //await StartTimerAsync();
    }

    private void NextRoundVisuals()
    {
        if (!which)
        {
            labelWhichPlayer.Text = $"Odpowiada {player1.name}";
            playerGradientLeft.Color = Color.FromArgb("#414141");
            playerGradientRight.Offset = 1;
            playerGradientRight.Color = Color.FromArgb("#FF0000");
            playerGradientLeft.Offset = 0.04f;
            which = true;
        }
        else
        {
            labelWhichPlayer.Text = $"Odpowiada {player2.name}";
            playerGradientRight.Color = Color.FromArgb("#414141");
            playerGradientRight.Offset = 1;
            playerGradientLeft.Color = Color.FromArgb("#00F6FF");
            playerGradientLeft.Offset = 0.6f;
            which = false;
        }
    }


    public async void AnswerClicked(object sender, EventArgs e)
    {
        if (sender is not Button clicked)
            return;

        //answerLocked = true;
        //timerCts?.Cancel();
        SetButtonsEnabled(false);

        bool isCorrect = clicked.ClassId == "true";

        if (isCorrect)
        {
            clicked.BackgroundColor = correctColor;
            clicked.TextColor = Colors.White;
        }
        else
        {
            clicked.BackgroundColor = wrongColor;
            clicked.TextColor = Colors.White;

            var correct = FindCorrectButton();
            if (correct != null)
            {
                correct.BackgroundColor = correctColor;
                correct.TextColor = Colors.White;
            }
        }

        //int gainedPoints = remainingTime;

        if (!which)
        {
            if (isCorrect)
            {
                player1.score += 1;
                scorePlayer1.Text = player1.score.ToString();
            }
            else
            {
                player1.hp = Math.Max(0, player1.hp - 1);
                UpdateHearts();
            }
        }
        else
        {
            if (isCorrect)
            {
                player2.score += 1;
                scorePlayer2.Text = player2.score.ToString();
            }
            else
            {
                player2.hp = Math.Max(0, player2.hp - 1);
                UpdateHearts();
            }
        }

        await Task.Delay(700);

        if (player1.hp == 0 || player2.hp == 0)
        {
            await Navigation.PushAsync(new SummaryPage(player1, player2, quizQuestions.Count));
            return;
        }

        currentIndex++;
        if (currentIndex >= quizQuestions.Count)
        {
            await Navigation.PushAsync(new SummaryPage(player1, player2, quizQuestions.Count));
            return;
        }

        NextRoundVisuals();
        DisplayQuestion();
    }
    //public async void AnswerClicked(object sender, EventArgs e)
    //{
    //    if (sender is not Button clicked)
    //        return;

    //    SetButtonsEnabled(false);

    //    bool isCorrect = clicked.ClassId == "true";

   
    //    if (isCorrect)
    //    {
    //        clicked.BackgroundColor = correctColor;
    //        clicked.TextColor = Colors.White;
    //    }
    //    else
    //    {
    //        clicked.BackgroundColor = wrongColor;
    //        clicked.TextColor = Colors.White;

     
    //        var correct = FindCorrectButton();
    //        if (correct != null)
    //        {
    //            correct.BackgroundColor = correctColor;
    //            correct.TextColor = Colors.White;
    //        }
    //    }

       
    //    if (!which)
    //    {
       
    //        if (isCorrect)
    //        {
    //            player1.score += 1;
    //            scorePlayer1.Text = player1.score.ToString();
    //        }
    //        else
    //        {
    //            player1.hp = Math.Max(0, player1.hp - 1);
    //            UpdateHearts();
    //        }
    //    }
    //    else
    //    {
    //        if (isCorrect)
    //        {
    //            player2.score += 1;
    //            scorePlayer2.Text = player2.score.ToString();
    //        }
    //        else
    //        {
    //            player2.hp = Math.Max(0, player2.hp - 1);
    //            UpdateHearts();
    //        }
    //    }

        
    //    await Task.Delay(700);

       
    //    if (player1.hp == 0 || player2.hp == 0)
    //    {
    //        UpdateHearts();
    //        await Navigation.PushAsync(new SummaryPage(player1, player2, quizQuestions.Count));
    //        return;
    //    }

       
    //    currentIndex++;

    //    if (currentIndex >= quizQuestions.Count)
    //    {
    //        await Navigation.PushAsync(new SummaryPage(player1, player2, quizQuestions.Count));
    //        return;
    //    }

    
    //    NextRoundVisuals();
    //    DisplayQuestion();
    //}

    private Button? FindCorrectButton()
    {
        if (btnA.ClassId == "true") return btnA;
        if (btnB.ClassId == "true") return btnB;
        if (btnC.ClassId == "true") return btnC;
        if (btnD.ClassId == "true") return btnD;
        return null;
    }

    private void SetButtonsEnabled(bool enabled)
    {
        btnA.IsEnabled = enabled;
        btnB.IsEnabled = enabled;
        btnC.IsEnabled = enabled;
        btnD.IsEnabled = enabled;
    }

    private void ResetButtonVisuals()
    {
        btnA.BackgroundColor = defaultButtonBg;
        btnB.BackgroundColor = defaultButtonBg;
        btnC.BackgroundColor = defaultButtonBg;
        btnD.BackgroundColor = defaultButtonBg;

        btnA.TextColor = Colors.White;
        btnB.TextColor = Colors.White;
        btnC.TextColor = Colors.White;
        btnD.TextColor = Colors.White;
    }

    private void UpdateHearts()
    {
        if (player1.hp == 3)
        {
            p1Heart1.Source = "heart_icon.png";
            p1Heart2.Source = "heart_icon.png";
            p1Heart3.Source = "heart_icon.png";
        }
        else if (player1.hp == 2)
        {
            p1Heart3.Source = "heart_icon_empty.png";
        }
        else if (player1.hp == 1)
        {
            p1Heart2.Source = "heart_icon_empty.png";
        }
        else if (player1.hp == 0)
        {
            p1Heart1.Source = "heart_icon_empty.png";
        }
        if (player2.hp == 3)
        {
            p2Heart1.Source = "heart_icon.png";
            p2Heart2.Source = "heart_icon.png";
            p2Heart3.Source = "heart_icon.png";
        }
        else if (player2.hp == 2)
        {
            p2Heart3.Source = "heart_icon_empty.png";
        }
        else if (player2.hp == 1)
        {
            p2Heart2.Source = "heart_icon_empty.png";
        }
        else if (player2.hp == 0)
        {
            p2Heart1.Source = "heart_icon_empty.png";
        }
    }
    
    //private async Task StartTimerAsync()
    //{
    //    timerCts?.Cancel();
    //    timerCts = new CancellationTokenSource();

    //    remainingTime = maxTime;
    //    answerLocked = false;

    //    double fullWidth = this.Width;
    //    if (fullWidth <= 0) fullWidth = 300;

    //    while (remainingTime > 0 && !answerLocked)
    //    {
    //        timerBar.WidthRequest = fullWidth * (remainingTime / (double)maxTime);
    //        remainingTime--;
    //        try
    //        {
    //            await Task.Delay(1000, timerCts.Token);
    //        }
    //        catch
    //        {
    //            return;
    //        }
    //    }

    //    if (!answerLocked)
    //    {
    //        TimeOut();
    //    }
    //}
    //private async void TimeOut()
    //{
    //    answerLocked = true;
    //    SetButtonsEnabled(false);

    //    if (!which)
    //    {
    //        player1.hp--;
    //        UpdateHearts();
    //    }
    //    else
    //    {
    //        player2.hp--;
    //        UpdateHearts();
    //    }

    //    await Task.Delay(700);

    //    if (player1.hp == 0 || player2.hp == 0)
    //    {
    //        await Navigation.PushAsync(new SummaryPage(player1, player2, quizQuestions.Count));
    //        return;
    //    }

    //    currentIndex++;
    //    if (currentIndex >= quizQuestions.Count)
    //    {
    //        await Navigation.PushAsync(new SummaryPage(player1, player2, quizQuestions.Count));
    //        return;
    //    }

    //    NextRoundVisuals();
    //    DisplayQuestion();
    //}
}