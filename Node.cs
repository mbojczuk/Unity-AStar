using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;   //distance from our starting node
    public int hCost;   //how far away the node is from the end
    public Node parent; //parent node for list traversal


    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    //returns fcost  f(n) = g(n) + h(n)
    public int Fcost
    {
        get{ return gCost + hCost; }
    }
}
