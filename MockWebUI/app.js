// Data Models (Replicated from SampleContentService.cs)
const appData = {
    modules: [
        {
            id: "foundations",
            title: "Foundations",
            summary: "Understand what soil is, why it matters, and how it gets polluted.",
            lessons: [
                {
                    id: "f-1",
                    title: "What is Soil?",
                    summary: "Soil is a mix of minerals, organic matter, water, and air.",
                    sections: [
                        { heading: "Definition", body: "Soil is a living system that supports plants and ecosystems." },
                        { heading: "Why it Matters", body: "Healthy soil grows food, filters water, and stores carbon." }
                    ],
                    quiz: [
                        {
                            prompt: "Which is a key role of healthy soil?",
                            options: ["Stores carbon", "Produces plastic", "Creates smoke"],
                            correctIndex: 0,
                            explanation: "Healthy soil stores carbon and supports plant life."
                        },
                        {
                            prompt: "Soil is made up of which components?",
                            options: ["Minerals, organic matter, water, air", "Only rocks", "Only water"],
                            correctIndex: 0,
                            explanation: "Soil includes minerals, organic matter, water, and air."
                        }
                    ]
                }
            ]
        },
        {
            id: "sources",
            title: "Pollution Sources",
            summary: "Learn where soil pollution comes from and how it spreads.",
            lessons: [
                {
                    id: "s-1",
                    title: "Industrial Waste",
                    summary: "Factories and spills can release harmful chemicals into soil.",
                    sections: [
                        { heading: "Key Sources", body: "Leaks, improper storage, and dumping are common causes." },
                        { heading: "Prevention", body: "Safe disposal and monitoring reduce risks." }
                    ],
                    quiz: [
                        {
                            prompt: "Which action helps prevent industrial soil pollution?",
                            options: ["Proper waste storage", "Open dumping", "Ignoring leaks"],
                            correctIndex: 0,
                            explanation: "Proper storage and monitoring prevent contamination."
                        }
                    ]
                }
            ]
        },
        {
            id: "impacts",
            title: "Impacts",
            summary: "See how soil pollution affects health, food, and ecosystems.",
            lessons: [
                {
                    id: "i-1",
                    title: "Health Impacts",
                    summary: "Polluted soil can lead to unsafe food and water.",
                    sections: [
                        { heading: "Human Health", body: "Contaminants can enter the food chain and harm people." },
                        { heading: "Communities", body: "Children and farmers are often most exposed." }
                    ],
                    quiz: [
                        {
                            prompt: "How can soil pollution affect people?",
                            options: ["By contaminating food", "By improving air quality", "By making soil glow"],
                            correctIndex: 0,
                            explanation: "Pollutants can enter crops and water, affecting health."
                        }
                    ]
                }
            ]
        },
        {
            id: "solutions",
            title: "Solutions",
            summary: "Explore ways communities can prevent and clean up soil pollution.",
            lessons: [
                {
                    id: "so-1",
                    title: "Prevention First",
                    summary: "Reducing pollution at the source is the most effective solution.",
                    sections: [
                        { heading: "Reduce at the Source", body: "Proper waste handling and safer chemicals reduce contamination risks." },
                        { heading: "Community Action", body: "Clean-up drives and awareness campaigns help protect local soil." }
                    ],
                    quiz: [
                        {
                            prompt: "Which action best prevents soil pollution?",
                            options: ["Proper waste handling", "Dumping chemicals", "Ignoring leaks"],
                            correctIndex: 0,
                            explanation: "Preventing pollution at the source is the most effective solution."
                        }
                    ]
                },
                {
                    id: "so-2",
                    title: "Cleaning Contaminated Soil",
                    summary: "Remediation methods help restore polluted areas.",
                    sections: [
                        { heading: "Bioremediation", body: "Plants and microbes can break down or absorb pollutants." },
                        { heading: "Soil Washing", body: "Soil can be treated to remove contaminants and then reused." }
                    ],
                    quiz: [
                        {
                            prompt: "What is bioremediation?",
                            options: ["Using living organisms to clean soil", "Adding more chemicals", "Leaving soil untreated"],
                            correctIndex: 0,
                            explanation: "Bioremediation uses plants or microbes to reduce pollution."
                        }
                    ]
                },
                {
                    id: "so-3",
                    title: "Policy and Community Action",
                    summary: "Rules, monitoring, and community efforts keep soil safer.",
                    sections: [
                        { heading: "Regulations", body: "Policies limit dumping and require safe waste handling." },
                        { heading: "Monitoring", body: "Testing soil helps detect pollution early." }
                    ],
                    quiz: [
                        {
                            prompt: "Why is monitoring soil important?",
                            options: ["It detects pollution early", "It causes pollution", "It replaces recycling"],
                            correctIndex: 0,
                            explanation: "Monitoring finds pollution early so it can be addressed."
                        }
                    ]
                }
            ]
        }
    ]
};

// State Management
const getProgress = () => JSON.parse(localStorage.getItem('ecoRootProgress') || '[]');

const saveProgress = (lessonId) => {
    const current = getProgress();
    if (!current.includes(lessonId)) {
        current.push(lessonId);
        localStorage.setItem('ecoRootProgress', JSON.stringify(current));
    }
};

const isLessonComplete = (lessonId) => getProgress().includes(lessonId);

const calculateProgress = () => {
    let totalLessons = 0;
    let completedLessons = 0;
    const completedIds = getProgress();

    appData.modules.forEach(mod => {
        totalLessons += mod.lessons.length;
        mod.lessons.forEach(l => {
            if (completedIds.includes(`${mod.id}.${l.id}`)) {
                completedLessons++;
            }
        });
    });

    return { total: totalLessons, completed: completedLessons, ratio: totalLessons === 0 ? 0 : completedLessons / totalLessons };
};

// UI Helpers
const renderNav = (activePage) => {
    const nav = document.createElement('div');
    nav.className = 'tab-bar';
    nav.innerHTML = `
        <a href="index.html" class="tab-item ${activePage === 'home' ? 'active' : ''}">
            <span class="tab-icon">🏠</span>
            <span>Home</span>
        </a>
        <a href="modules.html" class="tab-item ${activePage === 'modules' ? 'active' : ''}">
            <span class="tab-icon">📚</span>
            <span>Modules</span>
        </a>
         <a href="resources.html" class="tab-item ${activePage === 'resources' ? 'active' : ''}">
            <span class="tab-icon">🍎</span>
            <span>Resources</span>
        </a>
        <a href="glossary.html" class="tab-item ${activePage === 'glossary' ? 'active' : ''}">
            <span class="tab-icon">📖</span>
            <span>Glossary</span>
        </a>
    `;
    document.body.appendChild(nav);
};
