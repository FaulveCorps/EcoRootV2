# Eco Way

Eco Way is a mobile-only interactive choose-your-own-adventure simulation built with .NET MAUI. It focuses on environmental decision-making and long-term sustainability outcomes.

## Project Folder Structure

- `Models/`: Contains the data models (`Scenario`, `Choice`, `Ending`, `GameState`, `DecisionRecord`).
- `ViewModels/`: Contains the MVVM ViewModels (`ScenarioViewModel`, `SummaryViewModel`, `BaseViewModel`).
- `Views/`: Contains the XAML pages (`ScenarioPage`, `SummaryPage`).
- `Services/`: Contains the business logic and state management (`ScenarioService`, `GameStateService`).
- `Resources/Raw/`: Contains the scenario graph JSON (`scenarios.json`).
- `Helpers/`: (Optional) Contains animation and utility helpers.

## Branching Logic Explanation

The branching logic is driven by a JSON file (`scenarios.json`) that defines a graph of scenarios. Each scenario has a list of choices, and each choice specifies a `TargetScenarioId` and an `ImpactDelta`.

When a user makes a choice:
1. The `GameStateService` records the decision, updates the total score with the `ImpactDelta`, and updates the current scenario ID to the `TargetScenarioId`.
2. The `ScenarioViewModel` loads the new scenario based on the updated ID.
3. If the `TargetScenarioId` is `"end"`, the app navigates to the `SummaryPage`.
4. The `SummaryPage` calculates the final ending based on the total score and the thresholds defined in the JSON file.

## Run Instructions

### Android Emulator
1. Open the project in Visual Studio or VS Code.
2. Select an Android emulator as the target device.
3. Run the project (`F5` or `dotnet run -f net10.0-android`).

### iOS Simulator
1. Open the project in Visual Studio or VS Code on a Mac (or connected to a Mac build host).
2. Select an iOS simulator as the target device.
3. Run the project (`F5` or `dotnet run -f net10.0-ios`).

### Windows (For Testing)
1. Run the project (`dotnet run -f net10.0-windows10.0.19041.0`).

## Performance Notes & Maintainability Guidelines

- **Animation:** Animations are kept lightweight using native MAUI capabilities (`ScaleToAsync`, `TranslateToAsync`, etc.). Avoid complex layouts inside animated elements.
- **State Management:** The `GameStateService` is a singleton that maintains the current state. This ensures the state is preserved across page navigations.
- **Data Loading:** The scenario graph is loaded asynchronously from a local JSON file. This keeps the app responsive during startup.
- **MVVM:** The app strictly follows the MVVM pattern. Business logic is kept in ViewModels and Services, not in the code-behind of the Views.
- **Reusability:** The `ScenarioPage` is a reusable template that dynamically updates its content based on the current scenario. This avoids creating a separate page for each scenario.