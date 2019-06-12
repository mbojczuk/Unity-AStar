using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSizeNum : MonoBehaviour
{
    int gridSize;

    public GridSizeNum()
    {
        gridSize = 0;
    }

    public GridSizeNum(int gridSize)
    {
        this.gridSize = gridSize;
    }

    public void setGridSize(int gridSize)
    {
        this.gridSize = gridSize;
    }

    public int getGridSize()
    {
        return gridSize;
    }
}
