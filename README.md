# GameAlgorithm 🛸👾 Space Battle 

<img src="https://github.com/mich0292/Game_Algorithm/blob/5ee7216a022f8474914778f57e973fc6eb218859/Screenshots/manual/gameplay%20-%20level%201.png" alt="Level 1 Gameplay" width="80%" height="80%">

*Note: This game uses MonoGame framework.*

The game, Space Battle is a shoot ‘em up game with a top-down perspective.\
There are a few objects in this game: asteroids, turrets, enemies, boss, missile, and player’s avatar.\
Since the assignment is for the Game Algorithm subject, the focus is on algorithms such as:
* kinematic seek
* dynamic wander 
* pattern movement (CatmullRomSpline) 
* line of sight (Bresenham's line), pathfinding (A* algorithm) 
* decision-making (finite state machine)

## Win Condition

The player needs to progress through the stage without getting destroyed by the barrage of bullets from the enemies and defeat the boss at the end of the stage.

## Lose Condition

The player loses all of their health bars.

## Controls
  Keys	|   In-game action
:------:| --------------------------
   ↑	|   Move forward
   ↓	|   Move backward
   ←	|   Move left
   →	|   Move right 
 Space  |   Fire bullet
   Z	|   Fire missile  
  Tab	|   Select enemy (missile)
  Enter	|   Pause Game

## Character Design

Game Object	    | Health bar| Destroyable	| Can shoot? | Bullet damage   | Clash damage | Movement         | Line of sight | Decision-making
----------------| --------- |-------------|------------|-----------------|--------------|------------------|---------------|------------------
Player’s avatar	|  10	      |  ✔️	       |✔️(bullet, missile)|	1	      |1	           |Player controlled	|Player’s	      |❌
Missile	        |   ❌      | ❌	       |❌	         | 3               |3	         	  |Pathfinding	     |❌	           |❌
Asteroid	      |    1	    |✔️	         |❌	         |❌	              |1	            |Dynamic wander	  |❌	          |❌
Turret	        |    10	    |✔️	         |✔️	        |1	              |1	            |Stationary	       |✔️	          |✔️
Enemy1	        |    3	    |✔️	         |✔️	        |1	              |1	            |Kinematic seek	   |✔️	          |❌
Enemy2	        |    7	    |✔️	         |✔️	        |1	              |1	            |Pattern movement	 |✔️	          |❌
Boss (Level 1)	|    25	    |✔️	         |✔️	        |1	              |1	            |Stationary	       |✔️	          |✔️
Boss (Level 2)	|    50	    |✔️	         |✔️	        |1	              |1	            |Stationary	       |✔️	          |✔️

## Mechanics
1.  Move around and dodge bullets from enemies
2.  Fire bullets to kill enemies
3.  Fire missile to kill enemies

<img src="https://github.com/mich0292/Game_Algorithm/blob/5ee7216a022f8474914778f57e973fc6eb218859/Screenshots/manual/missiles.png" alt="Missile fired" width="50%" height="50%">

## ❓How to play our game❓
1.  Open the Project.sln file with Microsoft Visual Studio.
2.  Press Start

## Features
1. Powerup - This powerup can change the bullet pattern of the player.
2. Sound effect - We added sound effects when player or enemy shoots.
3. Pattern movement - The enemy2 moves in a pattern, which is based on our predefined list of control points.
4. Challenging boss - Our boss has the ability to dodge player’s bullets and the ability to attack faster during low health.

#### Please check out our [documentation](https://github.com/mich0292/Game_Algorithm/blob/c4bc46e7d496711917a0ee01d8e7e60706344cc7/Documentation%20&%20Presentation/SpaceBattle_1171101517_1171100973.pdf) for more details! 😀
