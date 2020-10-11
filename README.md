# Battleships
This project aims to be a simple, one-sided game of battleships.
In its current state, the game allows one player to make shots at a 10x10 board, aiming at a randomly generated fleet.

## Setup
To create a release version, use dotnet CLI. The target system and architecture should be specified, i.e. for Win10 x64:
```
dotnet publish -c Release -r win10-x64 -o release
```
The executable will be created in `release` folder.
From there, it can be run from the console or otherwise.
In case of Windows systems:
```
.\release\Battleships.ConsoleApp.exe
```

## Console Application
The Battleships console application provides a very simple console-based GUI.
Instructions are provided on-screen to help the player operate the game.
The GUI is more a PoC than anything else and was only tested manually.

## Logic
All of the game's logic is contained in `Battleships.Logic` project and its tests can be found in `Battleships.Logic.Tests`, respectively.
The point of entry (`GameFlowFacade`) is constructed through the construction classes (the builder), i.e.
```c#
var gameFlow = Generate.GameFlow()
    .WithBoardSize(boardSizeBounds)
    .WithBoardViewUpdater(boardViewState)
    .Build();
```
Game logic contracts are gathered in the `Contracts` folder.

Most notable points in this implementation are:
 - `FleetGenerator` creates the randomly generated fleet; it's used upon creating a new game
 - `PlayerInteractionHandler` handles the shots made by the player
 - `FleetSupervisor` supervises the current state of the fleet

## Tests
All tests are in `Battleships.Logic.Tests` project.
Besides unit tests, there are also integration tests (`Integration` folder) and more thoroughly testing the whole game flow - scenarios (in `Scenarios` folder).

## Expandability
 - a WebApi version could be added with little effort.
 - game board and fleet of any size are supported out-of-the-box
 - two player game could be added - a more elaborate game state management should then be implemented
 - 3D board option could be added but would be hard to present on the GUI