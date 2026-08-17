# UNITY PROJECT : RE-END

> A 3D single-player - action - rpg, has Soulslike elements game developed with Unity and C#.

## 🎮 Demo
[Download](https://pakbot4124.itch.io/re-end)
[Gameplay Video](https://www.youtube.com/watch?v=n6PHi1XOHdM)
[Play Demo](https://pakbot4124.itch.io/re-en-web)
## 📖 Overview
- RE-END is a 3D action, RPG game developed with Unity and C#.
- The project focuses on implementing core gameplay systems, including player movement, combat, skills, enemy behavior, and game progression. The combat system emphasizes timing. You can upgrade character stats or change skills according to your style.
- This project was developed as a portfolio project to demonstrate my
- Unity development and C# programming skills.

## ✨ Features
- Fighting the boss.
- Ability to upgrade after each failure.
- Many different types of skills and stats can be upgraded.

## 🛠️ Technical Highlights
### Engine & Tools
- Unity 6.3 (6000.3.8f1)
- C#
- Unity Input System
- Universal Render Pipeline (URP)
- Animator
- Timeline
- Unity UI (Canvas)
### Gameplay & Architecture
- Combat mechanics based on Soulslike style - Combat focuses on precise timing, checkpoints, boss fights, and character building.
- Trigger-based gameplay events - Used to control gameplay transitions between sections.
- State Machine Pattern - Organizes gameplay behaviors into independent states for clearer and more maintainable logic.
- Object Pooling Pattern- Used to efficiently reuse frequently spawned objects.
- Timeline — Used to orchestrate cinematic sequences.

## 🎯 Controls

| Action | Input |
|---|---|
| Move | WASD |
| Dodge| Spacebar |
| Jump | Ctrl |
| Run  | Left Shift |
| Interact | E |
| Light Attack | Left Mouse |
| Heavy Attack | Right Mouse |
| Skill | 1-2-3 |
| Use Potion | R |
| Change Potion | Scrooll Up/Down|
| Skip Cutscene | Tab|
## 📂 Project Structure

```text
Assets/
├── Script/
│   ├── Attack/         # Combat systems
│   ├── Audio/          # Audio Managers
│   ├── Cutscene/       # Cutscene
│   ├── Design Pattern/ # State Machine, Object Pool
│   ├── Environment/   # Environment can interact   
│   ├── Input/          # Input handling
│   ├── Interact/       # Interaction systems
│   ├── Managers/       # Global/gameplay/setting managers
│   ├── Physics/        # Physics systems
│   ├── Target/         # Focus on the enemy
│   ├── UI/             # UI systems
│
├── Settings/           # URP and rendering configuration
├── Timeline/           # Cutscene sequences
├── Prefab/             # Reusable game objects
└── Scene/              # Game scenes
```
## 🚀 How to Run

1. Clone the repository.
2. Open it with Unity 6000.3.8f1.
3. Open the main scene.
4. Press Play.

## 👨‍💻 Developer

Phạm Anh Khoa        
