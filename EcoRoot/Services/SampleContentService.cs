using EcoWay.Models;
using System.Collections.Generic;
using System.Linq;

namespace EcoWay.Services;

public sealed class SampleContentService : IContentService
{
    IReadOnlyList<Module>? cachedModules;

    public IReadOnlyList<Module> GetModules()
    {
        return cachedModules ??= new[]
        {
            new Module(
                "foundations",
                "Foundations",
                "Understand what soil is, why it matters, and how it gets polluted.",
                new[]
                {
                    new Lesson(
                        "f-1",
                        "What is Soil?",
                        "Soil is a mix of minerals, organic matter, water, and air.",
                        "5 min read",
                        new[]
                        {
                            new LessonSection("Definition", "Soil is a living system that supports plants and ecosystems."),
                            new LessonSection("Why it Matters", "Healthy soil grows food, filters water, and stores carbon.")
                        },
                        new[]
                        {
                            new QuizQuestion(
                                "Which is a key role of healthy soil?",
                                new[] { "Stores carbon", "Produces plastic", "Creates smoke" },
                                0,
                                "Healthy soil stores carbon and supports plant life."
                            ),
                            new QuizQuestion(
                                "Soil is made up of which components?",
                                new[] { "Minerals, organic matter, water, air", "Only rocks", "Only water" },
                                0,
                                "Soil includes minerals, organic matter, water, and air."
                            )
                        }
                    )
                }
            ),
            new Module(
                "sources",
                "Pollution Sources",
                "Learn where soil pollution comes from and how it spreads.",
                new[]
                {
                    new Lesson(
                        "s-1",
                        "Industrial Waste",
                        "Factories and spills can release harmful chemicals into soil.",
                        "4 min read",
                        new[]
                        {
                            new LessonSection("Key Sources", "Leaks, improper storage, and dumping are common causes."),
                            new LessonSection("Prevention", "Safe disposal and monitoring reduce risks.")
                        },
                        new[]
                        {
                            new QuizQuestion(
                                "Which action helps prevent industrial soil pollution?",
                                new[] { "Proper waste storage", "Open dumping", "Ignoring leaks" },
                                0,
                                "Proper storage and monitoring prevent contamination."
                            )
                        }
                    )
                }
            ),
            new Module(
                "impacts",
                "Impacts",
                "See how soil pollution affects health, food, and ecosystems.",
                new[]
                {
                    new Lesson(
                        "i-1",
                        "Health Impacts",
                        "Polluted soil can lead to unsafe food and water.",
                        "6 min read",
                        new[]
                        {
                            new LessonSection("Human Health", "Contaminants can enter the food chain and harm people."),
                            new LessonSection("Communities", "Children and farmers are often most exposed.")
                        },
                        new[]
                        {
                            new QuizQuestion(
                                "How can soil pollution affect people?",
                                new[] { "By contaminating food", "By improving air quality", "By making soil glow" },
                                0,
                                "Pollutants can enter crops and water, affecting health."
                            )
                        }
                    )
                }
            ),
            new Module(
                "solutions",
                "Solutions",
                "Explore ways communities can prevent and clean up soil pollution.",
                new[]
                {
                    new Lesson(
                        "so-1",
                        "Prevention First",
                        "Reducing pollution at the source is the most effective solution.",
                        "3 min read",
                        new[]
                        {
                            new LessonSection("Reduce at the Source", "Proper waste handling and safer chemicals reduce contamination risks."),
                            new LessonSection("Community Action", "Clean-up drives and awareness campaigns help protect local soil.")
                        },
                        new[]
                        {
                            new QuizQuestion(
                                "Which action best prevents soil pollution?",
                                new[] { "Proper waste handling", "Dumping chemicals", "Ignoring leaks" },
                                0,
                                "Preventing pollution at the source is the most effective solution."
                            )
                        }
                    ),
                    new Lesson(
                        "so-2",
                        "Cleaning Contaminated Soil",
                        "Remediation methods help restore polluted areas.",
                        "5 min read",
                        new[]
                        {
                            new LessonSection("Bioremediation", "Plants and microbes can break down or absorb pollutants."),
                            new LessonSection("Soil Washing", "Soil can be treated to remove contaminants and then reused.")
                        },
                        new[]
                        {
                            new QuizQuestion(
                                "What is bioremediation?",
                                new[] { "Using living organisms to clean soil", "Adding more chemicals", "Leaving soil untreated" },
                                0,
                                "Bioremediation uses plants or microbes to reduce pollution."
                            )
                        }
                    ),
                    new Lesson(
                        "so-3",
                        "Policy and Community Action",
                        "Rules, monitoring, and community efforts keep soil safer.",
                        "4 min read",
                        new[]
                        {
                            new LessonSection("Regulations", "Policies limit dumping and require safe waste handling."),
                            new LessonSection("Monitoring", "Testing soil helps detect pollution early.")
                        },
                        new[]
                        {
                            new QuizQuestion(
                                "Why is monitoring soil important?",
                                new[] { "It detects pollution early", "It causes pollution", "It replaces recycling" },
                                0,
                                "Monitoring finds pollution early so it can be addressed."
                            )
                        }
                    )
                }
            )
        };
    }

    public Module? GetModule(string id)
    {
        return GetModules().FirstOrDefault(module => module.Id == id);
    }

    public Lesson? GetLesson(string moduleId, string lessonId)
    {
        return GetModule(moduleId)?.Lessons.FirstOrDefault(lesson => lesson.Id == lessonId);
    }
}

