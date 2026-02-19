using System;
using System.Collections.Generic;

namespace EcoWay.Services;

public interface IProgressStore
{
    event EventHandler? ProgressChanged;

    IReadOnlyCollection<string> GetCompletedLessonKeys();

    bool IsLessonCompleted(string moduleId, string lessonId);

    void MarkLessonCompleted(string moduleId, string lessonId);
}

