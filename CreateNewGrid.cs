using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewGrid : MonoBehaviour {

    public LayerMask walkableMask;  //things that can't be walked on
    public float nodeRadius = 0.5f;        //how big the ndoes are
    Node[,] grid;                   //2d array of nodes
    float nodeDiameter;
    int gridSizeX, gridSizeY;
    GridSizeNum  gridNum;
    public int tempGridSize = 0;

    void Start()
    {
        gridNum = GetComponent<GridSizeNum>();
    }

    public int getGridSize()
    {
        return gridNum.getGridSize();
    }

    public void initialGridCreation(int size, Vector3 charPos)
    {
        nodeDiameter = nodeRadius * 2;
        //rounding here to stay int cause that will return a float otherwise
        gridSizeX = Mathf.RoundToInt(size / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(size / nodeDiameter);
        //setting a value that we manipulate
        gridNum.setGridSize(size);
        tempGridSize = gridNum.getGridSize();
        CreateGrid(charPos, tempGridSize);
    }

     void CreateGrid(Vector3 charPos, int size)
    {
        //create grid of node size
        grid = new Node[gridSizeX, gridSizeY];
        //creating a anchor position
        Vector3 worldBottomLeft = charPos - Vector3.right * size / 2 - Vector3.forward * size / 2;

        //inputting the nodes into the grid array (starts from 0,0,0 and works its way out)
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                //if there is a collision with a node
                //physics.checksphere returns true if hit
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, walkableMask));
                //pass those points accross
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
        Debug.Log(grid[5, 3].gridX);
    }


    //for neighbour search we wanna search around the current node we are at and add them to a list of nodes
    //we then return the list
    public List<Node> Neighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        //for loops for x and y values
        for (int i = -1; i <= 1; i++)
        {
            for (int k = -1; k <= 1; k++)
            {
                //skip over the if we equal currentLocation
                if (i == 0 && k == 0)
                {
                    continue;
                }
                //added in check so array doesn't go out of bounds....
                int checkXBounds = node.gridX + i;
                int checkYBounds = node.gridY + k;

                if (checkXBounds >= 0 && checkXBounds < gridSizeX && checkYBounds >= 0 && checkYBounds < gridSizeY)
                {
                    neighbours.Add(grid[checkXBounds, checkYBounds]);
                }
            }
        }
        return neighbours;
    }

    //TODO: grid not referenced here?? everything else working
    //this gets a position of what you pass accross it takes in a vector3 pos and returns a grid location 
    public Node NodeFromCharPoint(Vector3 worldPosition)
    {
        //first convert into a percentage of the x and y coordinate of how far along the grid it is. eg middle is .5% righht 1% and far left 0%
        float percentX = (worldPosition.x + gridNum.getGridSize() / 2) / gridNum.getGridSize();     //percent x value
        float percentY = (worldPosition.z + gridNum.getGridSize() / 2) / gridNum.getGridSize();     //percent y value
        percentX = Mathf.Clamp01(percentX);                                             //we clamp it so its doesnt go outside of the grid region for x and y
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridNum.getGridSize() - 1) * percentX);                           //rounding to nearest int
        int y = Mathf.RoundToInt((gridNum.getGridSize() - 1) * percentY);

        Debug.Log("I'm here");
        //why am i broken???
        Debug.Log(grid);

        return grid[x, y];        //returns an x and y pos on a grid
    }

    public List<Node> path;
    public List<Node> openedList;


    //visualise grid
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSizeX, 0, gridSizeY));
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                //for zones that are non walkable are red walkable is white
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
