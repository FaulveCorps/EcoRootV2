# Eco Way — Long-Term Context

## Product Identity

- **App Name:** Eco Way
- **Product Type:** Mobile-only interactive choose-your-own-adventure simulation
- **Theme:** Environmental decision-making and long-term sustainability outcomes
- **Core Experience:** Visual, animated scenario branching with meaningful consequences

## Vision

Build a polished, mobile-first .NET MAUI sustainability simulation where users learn through decisions, not lectures. Every choice should produce visible world changes and alter future paths.

## Mission

Help users understand environmental trade-offs by placing them inside interactive, animated scenarios where immediate and long-term consequences are both visible and measurable.

## Platform Scope (Non-Negotiable)

- **Framework:** .NET MAUI
- **Targets:** Android and iOS only
- **Architecture:** MVVM with strict separation of:
	- `Views`
	- `ViewModels`
	- `Models`
	- `Services`
- **Not in scope:** Desktop-specific UX flows and desktop-targeted delivery

## Experience Model

Eco Way presents connected environmental scenes such as:

- waste management
- transportation choices
- household/community energy use
- neighborhood sustainability initiatives

Each scenario must include:

1. a short contextual setup,
2. a visual animated scene,
3. 2 to 4 action choices,
4. branch navigation to next scenario based on selected action,
5. sustainability score and impact updates.

## Visual Storytelling Standard

The app must emphasize visual communication over long text blocks.

### Required Native MAUI Animation Tools

- `TranslateTo`, `FadeTo`, `ScaleTo`, `RotateTo`
- `Animation` class for coordinated timelines
- `GraphicsView` for custom scene rendering
- `AbsoluteLayout` or `Grid` for layered composition
- `Image`, `Shapes`, `BoxView` for animated objects

### Example Consequence Visuals

- Factory smoke intensity rises/falls after industrial decisions
- Traffic density changes if public transport is selected
- Trees grow/shrink from policy outcomes
- Water level transitions up/down based on water-use choices

### Animation Quality Rules

- Consequences must be visually clear after each decision
- Animations must remain smooth and responsive on mobile
- Use `async/await` for non-blocking transitions
- Avoid external animation packages unless absolutely necessary

## Core State Management

Persist and manage:

- current scenario ID
- ordered decision history
- total environmental score
- optional category impact totals (waste, transport, energy, community)
- ending classification based on score thresholds/rules

Use a branching graph (tree/graph) loaded from local JSON or equivalent local model data.

## Required Data Models

### Scenario

- `Id`
- `Title`
- `Description`
- `VisualConfig`
- `Choices` (2–4)

### Choice

- `Id`
- `Text`
- `TargetScenarioId`
- `ImpactDelta`
- optional category deltas

### Ending

- `Id`
- `Title`
- `Description`
- threshold/rule condition

### Supporting Models

- `DecisionRecord` (scenario, choice, score before/after, timestamp)
- `GameState` (current node, score, history, ending)

## UI/UX Direction

- Clean, minimal eco-themed palette
- Mobile-first layout and spacing
- Responsive across common phone form factors
- Reusable scenario template page
- Animated transitions between scenarios
- Fast interaction loop with clear feedback

## Advanced Features (Target)

- Progress indicator through scenario flow
- Restart / Play Again capability
- Summary screen with environmental impact timeline
- Optional ambient background animation
- Lightweight performance optimization suitable for mobile devices

## Project Structure Target

- `Models/`
- `ViewModels/`
- `Views/`
- `Services/`
- `Data/` (scenario graph JSON)
- `Resources/Styles/`
- `Resources/Images/`
- `Resources/Raw/`
- `Helpers/` (animation and utility helpers)

## Technical Deliverables

The implementation must include:

1. documented project folder structure,
2. at least one complete branching scenario chain,
3. ViewModel example handling choice + branching + score updates,
4. XAML with layered animated scene composition,
5. animation logic using native MAUI capabilities,
6. clear branching-logic explanation,
7. run instructions for Android emulator and iOS simulator,
8. performance notes and maintainability guidelines.

## Performance & Maintainability Rules

- Keep animation loops lightweight and frame-friendly
- Avoid unnecessary object allocation during scene updates
- Prefer reusable view elements and data-driven scene config
- Keep business/branching logic in ViewModels/Services, not page code-behind
- Keep code modular, testable, and easy to extend with new scenarios

## Definition of Done

Eco Way is considered complete when:

- Android and iOS builds run successfully,
- branching works deterministically for all defined choices,
- score/history/endings update correctly,
- consequence animations visibly match decisions,
- restart resets state cleanly,
- summary timeline reflects actual decision history,
- architecture remains MVVM-clean and maintainable.

## Current Build Priorities

1. Establish robust branching data model and scenario JSON loader
2. Implement reusable animated scenario page template
3. Implement state container (history + score + ending resolver)
4. Add sample scenario chain (waste + transport + energy)
5. Build summary timeline and restart flow
6. Tune animation smoothness for mobile hardware

---
Last updated: 2026-02-21
