namespace QuizApp.Models;

public class Question
{
    public string question { get; set; }
    public List<string> answers { get; set; }
    public string correct { get; set; }
}