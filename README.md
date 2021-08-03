# GameAlgorithm 🛸👾 Space Battle 

![Game Just Started](https://github.com/mich0292/Game_Physics/blob/51ac23eb160a2f651e0c7878f30723fcdfc2871e/Screenshots/Starting%20Game.png)

*Note: This game uses MonoGame framework.*

The game, Space Battle is a shoot ‘em up game with a top-down perspective.\
There are a few objects in this game: asteroids, turrets, enemies, boss, missile, and player’s avatar.\
Since the assignment is for the Game Algorithm subject, the focus is on algorithms such as:
* kinematic seek
* dynamic wander 
* pattern movement (CatmullRomSpline) 
* line of sight (Bresenham's line), pathfinding (A* algorithm) 
* decision-making (finite state machine)

**Win Condition**

The player needs to progress through the stage without getting destroyed by the barrage of bullets from the enemies and defeat the boss at the end of the stage.

**Lose Condition**

The player loses all of their health bars.

**Controls**
  Keys	 |   In-game action
------- | ------------------------------
   ↑	   |   Move forward
   ↓	   |   Move backward
   ←	   |   Move left
   →	   |   Move right 
  Space |   Fire bullet
   Z	   |   Fire missile  
   Tab	 |   Select enemy to fire missile
  Enter	|   Pause Game
   
**Mechanics:**
1.  Control the angle of the ship’s trajectory
2.  Strength of the trajectory

![Charge Bar](https://github.com/mich0292/Game_Physics/blob/51ac23eb160a2f651e0c7878f30723fcdfc2871e/Screenshots/Mechanics%20-%20Charging%20Bar.png)

❓**How to install our game**❓
1.  Ensure that you are at the directory of the game folder in cmd. 
	
2.  To compile our game and generate an executable file, please copy the following line and paste it to your cmd:
    ```
    compile Main.cpp Planet.cpp Strength.cpp Player.cpp Wall.cpp MyContactListener.cpp
    ```

3.  Type a in the cmd to run our game

![How to Install](https://github.com/mich0292/Game_Physics/blob/d3a4ca847372713c0c5966ee6d178bff7fcfcb6f/Screenshots/How%20to%20Install.png)

**Features**
1. Powerup - This powerup can change the bullet pattern of the player.
2. Sound effect - We added sound effects when player or enemy shoots.
3. Pattern movement - The enemy2 moves in a pattern, which is based on our predefined list of control points.
4. Challenging boss - Our boss has the ability to dodge player’s bullets and the ability to attack faster during low health.

Please check out our [documentation](https://github.com/mich0292/Game_Physics/blob/6957b9effd1935083e4262de4c4bd5b76ef51d75/(1171100973_1171101517)%20WrittenReport.pdf) for more details! 😀
