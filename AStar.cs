using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour {


    CreateNewGrid grid;                                          //grid script call

    void Start()
    {
        grid = GetComponent<CreateNewGrid>();
    }


    public List<Node> findPath(Vector3 startPos, Vector3 targetPos)
    {
        //starting position player and destination 
        Node startNode = grid.NodeFromCharPoint(startPos);
        Node targetNode = grid.NodeFromCharPoint(targetPos);
        //next while
        Node currentEndNode = targetNode;
        List<Node> path = new List<Node>();


        List<Node> openSet = new List<Node>();                  //this will contain the set of nodes we need to check through
        HashSet<Node> closedSet = new HashSet<Node>();                //this will contain the node we have already been through
        openSet.Add(startNode);                                 //add startNode which is starting position for zombie

        //so while we have something in our list
        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                //if the two nodes have same fcost we check the hcost and then take the one closest to the end of the node
                if (openSet[i].Fcost < currentNode.Fcost || (openSet[i].Fcost == currentNode.Fcost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }
            //remove the current node we are on
            openSet.Remove(currentNode);
            //add the current node to the closed Set
            closedSet.Add(currentNode);

            //if we are at the goalState
            if (currentNode == targetNode)
            {
                currentEndNode = currentNode;
                break;
            }

            foreach (Node neighbour in grid.Neighbours(currentNode))
            {
                if (closedSet.Contains(neighbour) || !neighbour.walkable)
                {
                    continue;
                }
                //so remeber gCost is shortest path from the start node and is what we use to check if shorter
                int newCostToNeighbour = currentNode.gCost + Distance(currentNode, neighbour);

                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    //as we get fcost we just need to set the g and h cost
                    //which are the distance from start node which we just got
                    neighbour.gCost = newCostToNeighbour;
                    //and the hcost being distance to the goal node
                    neighbour.hCost = Distance(neighbour, targetNode);
                    //set new parent
                    neighbour.parent = currentNode;
                    //add neighbour to open set
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        while (currentEndNode != startNode)
        {
            path.Add(currentEndNode);
            currentEndNode = currentEndNode.parent;
        }
        path.Reverse();
        //grid.openedList = closedSet;
        //grid.path = path;
        return path;
    }

    //manhattan distance
    int Distance(Node node, Node goalNode)
    {
        return (int)Mathf.Abs(node.gridY - goalNode.gridY) + Mathf.Abs(node.gridX - goalNode.gridX);
    }

}

/*
 * PSUEDO CODE FOR A* PATHFINDING
 * function A*(start, goal)
 * OpenList - nodes to check
 * Closed List - nodes already checked
 * add the first node.Pos to the openList
 * 
 * Loop
 * While openSet is not Empty
 * check for lowest fCost based on current node
 * current = node in openSet with the lowest fCost value
 * remove currentNode from openSet
 * add currentNode to closedSet
 * if currentNode == targetNode return 
 * 
 * foreach neighbour of currentNode
 * if neighbour not in closedSet and can be walked on
 * continue
 * if neighbour not in openSet
 * add neighbour to openSet
 * if dist to neighbour is shorter or not in open
 * set fCost of neighbour
 * set parent of neighbour to current
 */
