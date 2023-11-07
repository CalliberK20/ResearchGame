using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeGrid
{
    public int GCost;
    public int HCost;
    public int GridX;
    public int GridY;
    [SerializeField]
    public NodeGrid parent;

    public bool nonWalkable = false; 


    public int FCost
    {
        get
        {
            return GCost + HCost;
        }
    }


    public NodeGrid(int gridX, int gridY)
    {
        this.GridX = gridX; 
        this.GridY = gridY;
    }

}
