using System.Collections.Generic;
using EcoRoot.Models;

namespace EcoRoot.Services;

public interface IContentService
{
    IReadOnlyList<Module> GetModules();

    Module? GetModule(string id);

    Lesson? GetLesson(string moduleId, string lessonId);
}
