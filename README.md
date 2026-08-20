# UNITY PROJECT : RE-END

> A 3D Action-RPG with Soulslike combat, developed in Unity 6 and C#.

## 🎮 Demo
- [Download](https://pakbot4124.itch.io/re-end)
- [Gameplay Video](https://www.youtube.com/watch?v=n6PHi1XOHdM)
- [Play Demo](https://pakbot4124.itch.io/re-en-web)
  
## 📖 Overview
- RE-END is a 3D action, RPG game developed with Unity and C#.
- The project focuses on implementing core gameplay systems, including player movement, combat, skills, enemy behavior, and game progression. The combat system emphasizes timing. You can upgrade character stats or change skills according to your style.
- This project was created as a portfolio piece to demonstrate my Unity development and C# programming skills.

## ✨ Features
- Soulslike-style combat with precise timing, i-frames, light/heavy attacks, and skill system.
- Character progression: upgrade stats and skills after each death.
- Boss fights with multiple phases.
- Checkpoint system.
- Multiple skill and stat build options.

## 🛠️ Technical Highlights
### Engine & Tools
- Unity 6.3 (6000.3.8f1).
- C#.
- Unity Input System.
- Universal Render Pipeline (URP).
- Animator.
- Timeline.
- Unity UI (Canvas).
  
### Gameplay & Architecture
- **Combat System**: Timing-based combat using OverlapBox / BoxCastAll for hit detection, Dodge i-frames, light & heavy attacks, and skill execution.
- **Progression System**: Allow players to upgrade stats and change skills after each failure.
- **State Machine**: Manage Player and Boss states for cleaner and more maintainable logic.
- **Object Pooling**: Optimize performance by reusing VFX and frequently spawned objects.
- **Tree Behavior**: Controlling the boss's behavior.
- **Timeline**: Control cutscenes and boss phase transitions.
- **Trigger-based Events**: Handle gameplay transitions between sections.

## 🎯 Controls

| Action          | Input              |
|-----------------|--------------------|
| Move            | WASD               |
| Dodge           | Spacebar           |
| Jump            | Ctrl               |
| Run             | Left Shift         |
| Interact        | E                  |
| Light Attack    | Left Mouse         |
| Heavy Attack    | Right Mouse        |
| Skill           | 1 / 2 / 3          |
| Use Potion      | R                  |
| Change Potion   | Scroll Up / Down   |
| Skip Cutscene   | Tab                |
## 📂 Project Structure

```text
Assets/
├── Script/
│   ├── Attack/           # Combat systems
│   ├── Audio/            # Audio managers
│   ├── Cutscene/         # Cutscene logic
│   ├── Design Pattern/   # State Machine, Object Pool
│   ├── Environment/      # Interactable environment
│   ├── Input/            # Input handling
│   ├── Interact/         # Interaction systems
│   ├── Managers/         # Global & gameplay managers
│   ├── Physics/          # Physics-related systems
│   ├── Target/           # Enemy targeting
│   └── UI/               # UI systems
├── Settings/             # URP and rendering settings
├── Timeline/             # Cutscene sequences
├── Prefab/               # Reusable prefabs
└── Scene/                # Game scenes
```
## 🚀 How to Run
1. Clone the repository.
2. Open the project with Unity 6000.3.8f1.
3. Open the main scene.
4. Press Play.

## 👨‍💻 Developer

Phạm Anh Khoa        
