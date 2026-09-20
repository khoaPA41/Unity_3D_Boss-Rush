# UNITY PROJECT : RE-END

> **A 3D Action-RPG · Soulslike Combat** - solo portfolio project, built in Unity 6.3 LTS with C#.


## 🎮 Demo
- [Download](https://pakbot4124.itch.io/re-end)
- [Gameplay Video](https://youtu.be/ITwK9bQ_CvQ)
  
## 📖 Overview
- RE-END is a 3D action, RPG game developed with Unity and C#.
- The project focuses on implementing core gameplay systems, including player movement, combat, skills, enemy behavior, and game progression. The combat system emphasizes timing. You can upgrade character stats or change skills according to your style.
- This project was created as a portfolio piece to demonstrate my Unity development and C# programming skills.

## Key Systems
- **Combat** — Timing-based hit detection OverlapBox, dodge i-frames, light/heavy attacks, skill execution.
- **Boss AI** — The Action Tree that controls the bosses.
- **State Machines** — Separate FSMs for Player, Player Clone, and Boss.
- **Save System** — JSON persistence for checkpoints, stats, boss-defeat tracking, and settings; Continue only shows when a save exists.
- **Performance** — Object pooling for VFX/spawned objects, custom Event Bus.
- **Progression** — Stat/skill upgrades unlocked on defeat of boss, multiple build paths.
- **Cinematics & Triggers** — Timeline-driven cutscenes and boss-phase transitions, trigger-based section flow.

## 🛠️ Technical Highlights
### Engine & Tools
- Unity 6.3 (6000.3.8f1).
- C#.
- Unity Input System.
- Universal Render Pipeline (URP).
- Animator.
- Timeline.
- Unity UI (Canvas).

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
├── Scripts/
│   ├── Attack/            # Combat systems
│   ├── Audio/             # Audio managers
│   ├── Cutscene/          # Cutscene logic
│   ├── Design Pattern/    # State Machine, Object Pool, Event Bus, Tree Behavior
│   ├── Environment/       # Interactable environment
│   ├── Input/             # Input handling
│   ├── Interact/          # Interaction systems
│   ├── Managers/          # Global & gameplay managers (Save, Boss Phase, Audio, Graphics)
│   ├── Physics/           # Physics-related systems
│   ├── ScriptableObject/  # Data assets (attack data, etc.)
│   ├── Target/            # Enemy targeting
│   └── UI/                # UI systems
├── Settings/              # URP and rendering settings
├── TimeLine/              # Cutscene sequences
├── Prefabs/               # Reusable prefabs (Player, Boss, Weapon, Audio, Proxy)
└── Scenes/                # Game scenes
```
## 🚀 Run Locally
1. Clone the repository.
2. Open the project with Unity 6000.3.8f1.
3. Open the Start scene.
4. Press Play.

## 👨‍💻 Developer
Phạm Anh Khoa        
