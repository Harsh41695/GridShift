
# GridShift
Capsitech Assignment --> Harsh Yadav Unity Game Developer Assignment

## Overview
A grid-based 2D puzzle game developed in Unity where the player
pushes boxes onto goal tiles while navigating walls and conveyors.

## Features
- Logical N x M grid independent of rendering
- WASD / Arrow key movement
- Mobile swipe controls
- Box-pushing puzzle mechanics
- Deterministic movement
- Undo system using compact move records
- Restart functionality
- Move counter
- Multiple levels
- Conveyor gameplay mechanic
- Level Complete / Game Complete flow
- How To Play screen
- Landscape mobile UI

## Controls
WASD / Arrow Keys - Move
Swipe - Move on mobile
Z - Undo
R - Restart

## Architecture

InputController
      ↓
GameController
      ↓
GridModel
      ↓
MoveResult
   ↙          ↘
MoveHistory   GridView
                  ↓
             GameplayUI

## Data Flow

User Gesture / Keyboard
        ↓
InputController
        ↓
GameController
        ↓
GridModel.TryMovePlayer()
        ↓
MoveResult
        ↓
MoveHistory + GridView
        ↓
GameplayUI

## Core Components

### GridModel
Owns the logical grid state including cells, player position,
boxes, goals, walls and conveyor positions.

### GameController
Coordinates gameplay systems and acts as the bridge between
input, logical state, history, rendering and UI.

### InputController
Converts keyboard and mobile swipe input into directional
movement commands.

### GridView
Responsible only for visual representation and movement
animations.

### MoveHistory
Stores successful MoveResult records in a stack to provide
memory-efficient deterministic Undo without copying the entire
grid.

### LevelData
ScriptableObject containing level dimensions and initial
positions of walls, boxes, goals, conveyors and the player.

### GameplayUI
Handles move count and game-state UI such as level completion
and game completion.

## Undo Design

Instead of storing a deep copy of the complete board after every
move, the game records only the information required to reverse
a successful move:

- Previous player position
- New player position
- Whether a box moved
- Previous box position
- New box position

Undo therefore restores the previous state using a compact
MoveResult record.

## Extra Gameplay Mechanic

Level 3 introduces right-moving conveyor tiles.

When a box is pushed onto a conveyor, the conveyor attempts to
move it one additional cell to the right. If that destination is
blocked, the box remains on the conveyor.

The complete push/conveyor operation is treated as one
deterministic move and can be reversed with a single Undo.

## Project Structure

Assets/#Grid_Shift/
├── Art/
├── Materials/
├── Prefabs/
├── Scenes/
├── ScriptableObjects/
└── Scripts/
    ├── Core/
    ├── Grid/
    ├── Gameplay/
    ├── Input/
    ├── View/
    └── UI/

## How to Run

1. Open the project in Unity.
2. Open Assets/_Project/Scenes/Game.unity.
3. Enter Play Mode.
4. Use WASD/Arrow Keys or swipe to move.
5. Push all boxes onto goal tiles to complete each puzzle.

## Technical Decisions

- Grid logic is separated from Unity rendering.
- GridModel is a plain C# class rather than a MonoBehaviour.
- ScriptableObjects are used for data-driven level definitions.
- HashSet<Vector2Int> is used for efficient box/conveyor lookup.
- MoveResult records are used instead of full board snapshots.
- InputController abstracts keyboard and touch input from gameplay.
- GameController coordinates systems without placing grid logic
  inside the visual layer.

## Known Limitations

- Conveyor direction is currently right-only.
- Levels are authored manually using ScriptableObject data.
- The project focuses on the assignment's core puzzle mechanics
  rather than a full production-level level-selection/progression system.

  ## Additional Info

 AI-assisted tools were used as a supporting resource during development for project planning, architecture decisions, reviewing implementation approaches, and preparing/refining documentation, including this README.

The final implementation was integrated, tested, and validated by me.



## Developer

`Harsh Yadav` 
Unity Game Developer

---

## Thank You

Thank you for taking the time to review my assignment. I appreciate the opportunity to work on this project.

  