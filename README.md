# Space Empires Unity Project

Welcome to the Space Empires Unity project repository! This repository contains the source code and assets for a space-themed simulation developed as part of the Interactive Agents and Procedural Content Generation assignment during my master's at Queen Mary University of London.
This project utilizes NPBehave and UnityMovementAI libraries for agent behaviour management and movement AI, respectively.

## Video Link

[Watch the Space Empires gameplay video](https://youtu.be/2dZfpoKTvGY)

## Unity Version

This project is developed using Unity version 2022.2.1f1.

## The Generator

The Generator component of this project is responsible for generating the game level. It utilizes a Cellular Automata algorithm to create a 2D array representing the level layout, with 1s representing filled spaces (asteroids) and 0s representing free space. The algorithm generates a random level and then smooths it by applying rules based on neighbouring cells. The resulting level is used to place objects in the game scene, including asteroids, stars, and planets.

The Cellular Automata code is based on the implementation by Sebastian Lague, as demonstrated in [this video](https://www.youtube.com/watch?v=v7yyZZjF1z).

## Agents

The game features three types of agents:

1. **Protoss**: Protoss agents are smart explorers who either explore the scene, defend themselves from weaker Tarrens, or flee from stronger Zergs. The behaviour of Protoss agents is determined by a behaviour tree, which can assign one of four behaviours: ProtossBehave, AttackBehave, FleeBehave, and WonderBehave.

2. **Zerg**: Zerg agents are aggressive and relentless attackers. They target Tarrens and even attack Protoss if they come too close. Like Protoss agents, Zergs have behaviour trees with options to attack Tarrens or Protosses.

3. **Tarrens**: Tarren agents are the hunted entities in the game. If they come too close, they flee from both Zergs and Protosses. Tarrens also have behaviour trees with options to flee from Zergs or to wander.



## Getting Started

To get started with the project, clone this repository and open it in Unity. Ensure you have installed Unity version 2022.2.1f1 or later. You can then explore the code, run the game, and make modifications as needed.


*This project was developed by Nagarjuna as part of the Interactive Agents and Procedural Content Generation coursework at Queen Mary University of London.*
