# Submarine Runner Game

## Description

**Submarine Runner Game** is an underwater-themed action game where the player controls a submarine navigating through challenging environments, avoiding obstacles, and using power-ups to survive and defeat AI-controlled submarines while avoiding getting eaten by a shark.

## Table of Contents

- [Game Overview](#game-overview)
- [Gameplay Mechanics](#gameplay-mechanics)
- [Code Architecture](#code-architecture)
<!-- - [Gameplay](#Gameplay) -->

## Game Overview

The game consists of two types of submarines:

1. **Player Submarine**: Controlled by the player using keyboard or controller inputs.
2. **AI Submarines**: Enemies that patrol the environment, chase the player, and interact with the game world based on certain conditions.

The player must survive by using power-ups, avoiding obstacles, and outsmarting the AI submarines.

## Gameplay Mechanics

- **Player Submarine**:

  - The player can rotate the submarine upward and downward by pressing the space bar.
  - Power-ups provide temporary enhancements such as speed boosts, teleportation, and disabling enemies.
    - **TURBO**: Temporarily boosts the player's submarine speed.
    - **TELEPORT**: Teleports the player submarine to a new location.
    - **FISHNET**: Fires a net projectile that disables AI submarines.
    - **EMP**: Disables nearby AI submarines with an EMP pulse.
    - **REVERSE_CONTROL**: Reverses the movement controls of AI submarines.

- **AI Submarine**:

  - The AI submarines have an obstacle avoidance system using AI sensors that calculate whether to turn up or down based on obstacles in front of them.

- **Shark**:
  - The shark automatically targets the submarine in front of it and consumes it when in range.

## Code Architecture

The **Submarine Runner Game** is designed with a modular and maintainable codebase, utilizing key design principles to ensure scalability, flexibility, and ease of modification.

### Key Components

1. **Player Submarine**

   - The player submarine is handled by a dedicated `PlayerSubmarineController` class, which controls its movement, power-ups, and interactions with the game world.
   - It implements logic for player input, such as acceleration, deceleration, and power-up activation.
   - The power-ups are implemented as distinct components, which can be added to the player submarine and activated by using certain key inputs.

2. **AI Submarine**

   - The **_AI Submarine_** uses an **_obstacle avoidance system_** that relies on **_AI sensors_** to detect environmental hazards and adjust its movement accordingly. It avoids obstacles by determining whether it needs to steer up or down, based on the readings from its sensors:

     - The **_top sensor_** detects obstacles above the submarine and causes it to steer downward.
     - The **_bottom senso_** detects obstacles below the submarine and causes it to steer upward.
     - The **_center sensor_** checks for obstacles directly ahead and adjusts the steering depending on their position.

   - The **_AISubmarine_** follows an autonomous movement model controlled by the `AISubmarineController` class. This controller uses the readings from the sensors to decide whether to move up, down, or rotate based on proximity to obstacles or other stimuli.

   - **_Behavior and Decision-Making_**:

     - If the submarine encounters an obstacle, the **AISubmarine** automatically adjusts its rotation to avoid collisions. For example, it can rotate up or down depending on where the obstacle is detected.

3. **Power-ups**

   - Power-ups in the game are special items that grant the player submarine enhanced abilities, allowing them to gain advantages or overcome specific challenges. The power-ups are defined as distinct classes, each implementing unique behaviors. When the player submarine collects a power-up, it activates the associated effect.

     **PowerUp Types**:

     - The following power-ups are currently implemented:

       1. **_TURBO_**: Temporarily boosts the speed of the player submarine.
       2. **_TELEPORT_**: Teleports the player submarine to a nearby position.
       3. **_FISHNET_**: Launches a net that disables the movement of AI submarines.
       4. **_EMP_**: Temporarily disables enemy AI submarines by emitting an electromagnetic pulse.
       5. **_REVERSE_CONTROL_**: Reverses the movement controls of AI submarines.

     **PowerUp Class**

     - The base `PowerUp` class defines common behavior such as detecting collisions and triggering effects through the `UsePowerUp` method. Each specific power-up overrides this method to implement its unique behavior.

     **PowerUp Manager**

     - Manages power-up spawning and collection. Power-ups are randomly placed in the game world and are triggered when collected by the player submarine.

     **Specific PowerUp Implementations**:

     1. **EMPPowerUp**: Disables AI submarines by triggering the `ConfigEMP` method.
     2. **FishnetPowerUp**: Fires a projectile that disables AI submarines.
     3. **ReversePowerUp**: Fires a projectile that flips AI submarine controls.
     4. **TeleportPowerUp**: Teleports the player submarine to a new location.
     5. **TurboPowerUp**: Increases player speed temporarily.

4. **Game Manager**
   - The `GameManager` class oversees the overall game flow, including the spawning of submarines, power-ups, and controlling the game state (e.g., start, pause, game over).
   - It also manages the scoring and handling the game's difficulty progression, such as increasing AI submarine speed and frequency of power-up spawns.

### Design Patterns

- **Observer Pattern**: Power-ups and game events use the Observer pattern to notify relevant entities (like the player or AI) when changes occur (e.g., power-up pickup, game over).
- **Singleton Pattern**: The `GameManager` is implemented as a singleton to ensure only one instance controls the overall game flow, keeping the game state consistent.

### Modular Structure

The code is organized into the following main folders:

- **Controllers**: Contains scripts for player and AI submarine behavior, game management, and UI controls.
- **PowerUps**: Contains scripts defining each power-up type and its behavior.
- **Models**: Holds data structures and classes for managing submarine states, power-up configurations, and other game models.
- **Utilities**: Contains helper scripts for tasks like event management, and sensor systems.

### Scalability & Extensibility

The architecture is designed with scalability in mind, allowing for:

- Adding new power-ups and AI behaviors easily by creating new classes and extending existing systems.
- Expanding the game world by introducing new types of obstacles, power-ups, and submarine types without disrupting the existing codebase.

The modular approach ensures that each component of the game is loosely coupled and easy to test individually, making the development process more efficient and reducing the risk of bugs.

<!-- ## Gameplay

[![](https://github.com/Bhawesh02/SubmarineRunner/blob/main/SubmarineRunner/Assets/Extra/Submarine%20Jump%20Record.gif)](https://youtube.com/shorts/wjc8JKgfTDQ) -->

## Contribution

This project represents my individual development efforts, where I solely created the code base for a game creative in Kwalee, with art being provided by various other colleagues. Feedback, suggestions, and collaborative contributions are highly encouraged. If you're passionate about mesh deformation techniques, Unity development workflows, or procedural generation, I’d love to connect!

## Contact

You can connect with me on LinkedIn: [Bhawesh Agarwal](https://www.linkedin.com/in/bhawesh-agarwal-70b98b113). Feel free to reach out if you're interested in discussing the game's mechanics, and development process, or if you simply want to talk about game design and development.
