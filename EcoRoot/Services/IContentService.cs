using System.Collections.Generic;
using EcoWay.Models;

namespace EcoWay.Services;

public interface IContentService
{
    IReadOnlyList<Module> GetModules();

    Module? GetModule(string id);

    Lesson? GetLesson(string moduleId, string lessonId);
}

