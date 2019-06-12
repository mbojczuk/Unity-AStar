# Unity-AStar
Dynamic A star in Unity

To work it:
1. Create a gameobject in the inspector and attach AStar.cs, GridSizeNum.cs and CreateNewGrid.cs to it
2. Create a new Layer call it unWalkable or whatever you want and create randoms object that will have that as its layer
3. Creat a player and attach the Character.cs script to it
4. Hit play, right click.


What it does:
This is for a dynamic enviroment, for one of my projects in University I had a static map which a character would follow an A*
path to get to where ever, now I wanted it to generate a grid of smallish size based on where you clicked take in its new surroundings
and find a path.
It deletes the path and grid and creates a new grid each time based on where you clicked then using A* algorithm finds the fastest path



TODO:
At the moment it works on first click.
fixing it for recurring clicks, most likely something to do not updating the player position, will fix at some point.....



