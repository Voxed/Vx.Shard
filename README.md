<h1 align="center">Vx.Shard</h1>
<p align="center">
    <img width="300" src="VxShard-logo.svg">
</p>
An ECS based game engine designed for the DIT455 Game Engine Architecture course.

## Description
The engine uses the popular design pattern ECS (Entity-Component-System). It encapsulates a game world which contains Entities who act as containers for Components. The world also has a variable number of Systems attached, these systems are completely modular and make up all the logic of the engine. The chosen systems might include ones for Graphics and Physics, but there might also be game specific systems such as a Breakout System or a Ball System. This is how you make games in the engine, you extend and modify it to follow your will.

This structure is as one might imagine extremely modular, it allows for adding and replacing integral systems even from outside of the main engine source tree. As such, everything is optional, the engine ships with a couple of useful premade systems which you can use, or you can head out on your own and just make use of the ECS at it's core.

## Implemented(ish) Systems
## Todo
* Comments
* Improve existing systems
* More nice to have engine features
* Sound system
* Collision system
* Resource manager (no clear idea yet)