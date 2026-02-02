using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using EcoRoot.Models;
using EcoRoot.Resources;
using EcoRoot.Services;
using Microsoft.Maui.Controls;

namespace EcoRoot.ViewModels;

public sealed class QuizViewModel : BaseViewModel
{
    readonly IContentService contentService;
    readonly IProgressStore progressStore;
    readonly IQuizStatsStore quizStatsStore;
    IReadOnlyList<QuizQuestion> questions = Array.Empty<QuizQuestion>();
    IReadOnlyList<string> options = Array.Empty<string>();
    readonly Dictionary<int, bool> answerResults = new();
    int currentIndex;
    string? lessonTitle;
    string? prompt;
    string? selectedOption;
    string? feedback;
    string? moduleId;
    string? lessonId;
    bool quizCompleted;

    public QuizViewModel(IContentService contentService, IProgressStore progressStore, IQuizStatsStore quizStatsStore)
    {
        this.contentService = contentService;
        this.progressStore = progressStore;
        this.quizStatsStore = quizStatsStore;
        Title = "Quiz";
        SelectOptionCommand = new Command<string>(SelectOption);
        CheckAnswerCommand = new Command(CheckAnswer);
        NextQuestionCommand = new Command(NextQuestion);
    }

    public string? LessonTitle
    {
        get => lessonTitle;
        private set => SetProperty(ref lessonTitle, value);
    }

    public string? Prompt
    {
        get => prompt;
        private set => SetProperty(ref prompt, value);
    }

    public IReadOnlyList<string> Options
    {
        get => options;
        private set => SetProperty(ref options, value);
    }

    public string? SelectedOption
    {
        get => selectedOption;
        private set => SetProperty(ref selectedOption, value);
    }

    public string? Feedback
    {
        get => feedback;
        private set
        {
            if (SetProperty(ref feedback, value))
            {
                OnPropertyChanged(nameof(HasFeedback));
            }
        }
    }

    public bool HasFeedback => !string.IsNullOrWhiteSpace(Feedback);

    public string NextButtonText => currentIndex >= questions.Count - 1
        ? Strings.QuizFinish
        : Strings.QuizNextQuestion;

    public ICommand SelectOptionCommand { get; }

    public ICommand CheckAnswerCommand { get; }

    public ICommand NextQuestionCommand { get; }

    public void LoadQuiz(string? moduleId, string? lessonId)
    {
        this.moduleId = moduleId;
        this.lessonId = lessonId;
        quizCompleted = false;
        answerResults.Clear();
        var lesson = string.IsNullOrWhiteSpace(moduleId) || string.IsNullOrWhiteSpace(lessonId)
            ? null
            : contentService.GetLesson(moduleId, lessonId);

        LessonTitle = lesson?.Title;
        questions = lesson?.Quiz ?? Array.Empty<QuizQuestion>();
        currentIndex = 0;
        SetCurrentQuestion();
    }

    void SetCurrentQuestion()
    {
        if (questions.Count == 0)
        {
            Prompt = "No quiz available yet.";
            Options = Array.Empty<string>();
            SelectedOption = null;
            Feedback = null;
            OnPropertyChanged(nameof(NextButtonText));
            return;
        }

        var question = questions[currentIndex];
        Prompt = question.Prompt;
        Options = question.Options;
        SelectedOption = null;
        Feedback = null;
        OnPropertyChanged(nameof(NextButtonText));
    }

    void SelectOption(string? option)
    {
        if (string.IsNullOrWhiteSpace(option))
        {
            return;
        }

        SelectedOption = option;
        Feedback = null;
    }

    void CheckAnswer()
    {
        if (questions.Count == 0)
        {
            Feedback = "No questions available.";
            return;
        }

        if (SelectedOption is null)
        {
            Feedback = "Select an answer to continue.";
            return;
        }

        var question = questions[currentIndex];
        var isCorrect = question.CorrectIndex >= 0
            && question.CorrectIndex < question.Options.Count
            && string.Equals(question.Options[question.CorrectIndex], SelectedOption, StringComparison.Ordinal);

        answerResults[currentIndex] = isCorrect;

        Feedback = isCorrect
            ? $"Correct! {question.Explanation}"
            : $"Not quite. {question.Explanation}";
    }

    void NextQuestion()
    {
        if (questions.Count == 0)
        {
            return;
        }

        if (currentIndex >= questions.Count - 1)
        {
            if (!quizCompleted && !string.IsNullOrWhiteSpace(moduleId) && !string.IsNullOrWhiteSpace(lessonId))
            {
                progressStore.MarkLessonCompleted(moduleId, lessonId);
                var correctCount = answerResults.Count(result => result.Value);
                quizStatsStore.RecordAttempt(moduleId, lessonId, LessonTitle, correctCount, questions.Count);
                quizCompleted = true;
            }

            Feedback = "Quiz complete! Lesson marked as done.";
            return;
        }

        currentIndex++;
        SetCurrentQuestion();
    }
}
