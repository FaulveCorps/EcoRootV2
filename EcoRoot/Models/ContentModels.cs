using System.Collections.Generic;

namespace EcoWay.Models;

public sealed class Module
{
    public Module(string id, string title, string summary, IReadOnlyList<Lesson> lessons)
    {
        Id = id;
        Title = title;
        Summary = summary;
        Lessons = lessons;
    }

    public string Id { get; }
    public string Title { get; }
    public string Summary { get; }
    public IReadOnlyList<Lesson> Lessons { get; }
}

public sealed class Lesson
{
    public Lesson(string id, string title, string summary, string? timeEstimate, IReadOnlyList<LessonSection> sections, IReadOnlyList<QuizQuestion> quiz)
    {
        Id = id;
        Title = title;
        Summary = summary;
        TimeEstimate = timeEstimate;
        Sections = sections;
        Quiz = quiz;
    }

    public string Id { get; }
    public string Title { get; }
    public string Summary { get; }
    public string? TimeEstimate { get; }
    public IReadOnlyList<LessonSection> Sections { get; }
    public IReadOnlyList<QuizQuestion> Quiz { get; }
}

public sealed class LessonSection
{
    public LessonSection(string heading, string body)
    {
        Heading = heading;
        Body = body;
    }

    public string Heading { get; }
    public string Body { get; }
}

public sealed class QuizQuestion
{
    public QuizQuestion(string prompt, IReadOnlyList<string> options, int correctIndex, string explanation)
    {
        Prompt = prompt;
        Options = options;
        CorrectIndex = correctIndex;
        Explanation = explanation;
    }

    public string Prompt { get; }
    public IReadOnlyList<string> Options { get; }
    public int CorrectIndex { get; }
    public string Explanation { get; }
}

