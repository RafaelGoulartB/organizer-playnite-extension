# Sort Games by Status (Playnite Extension)

A Playnite C# Generic Plugin that enhances how your games are organized by letting you quickly toggle specific Completion Status groupings, "List Installed/All" views, and "Ascending/Descending" directions directly via menus or highly customizable keyboard shortcuts.

The extension also offers a unique feature to automatically append and remove organizational order numbers (e.g., `1. Beaten`, `2. Playing`) natively on the Playnite database, ensuring you get exactly the sorting queue you want instead of Playnite's default alphabetical constraints.

<img width="1360" height="840" alt="image" src="https://github.com/user-attachments/assets/5f997ad0-2041-4c42-acb1-22a0fb76c34c" />


## Features

- **Toggle Settings:** Bind customizable shortcuts to switch quickly between showing only Installed games or all Games, and to toggle group sorting orders Ascending or Descending.
- **Custom Native Status Sorting:** Playnite SDK natively sorts Completion Statuses alphabetically. This extension provides one-click helper methods (`Add Numbers to Statuses` / `Remove Numbers from Statuses`) to append precise numbering hierarchies to force Playnite into grouping states seamlessly such as:
  1. Beaten
  2. Playing
  3. On Going
  4. Plan to Play
  5. Played
  6. On Hold
  7. Not Played
  8. Abandoned
- **Visual Configurations:** Setup all specific shortcut modifiers (Ctrl, Shift, Alt) directly through `Settings > Add-ons > Generic Plugins > Sort Games by Status`.

## Development & Build Environment

The plugin was built targeting `.NET Framework 4.6.2` via `dotnet build` utilizing the PlayniteSDK. A `Makefile` is bundled, providing zero-configuration building.

1. Install the [.NET SDK 10+](https://dotnet.microsoft.com/)
2. Within the central folder, run the available targets:

- `make build`: Compiles the WPF views and C# logic, outputting the `PnExtension.dll` library securely into the project root next to the `extension.yaml` file so Playnite can load it automatically.
- `make restore`: Restores Nuget's minimum PlayniteSDK dependencies and Reference Assemblies.
- `make clean`: Removes residual `obj` and `bin` cache elements.
- `make publish`: Builds a clean `Release` bundle suitable for distribution framework.

## Installation / Update Steps

1. Make sure the Playnite Desktop Application is **completely Closed** before attempting a clean build to prevent file-locking.
2. Run `make build` within this directory.
3. Start Playnite.
4. Verify the extension is successfully loaded by navigating to Playnite's *Top Left Menu > Settings > Add-ons > Generic Plugins*.
