using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridCreateManager : MonoBehaviour
{
    public static GridCreateManager instance;

    public bool showGizmos = true;
    [Space]
    public Tilemap obstacleMap;
    [Space]
    public int rowLeght;
    public int colLeght;
    NodeGrid[,] gridList;

    private void Awake()
    {
        instance = this;
    }

    void GridCreate()
    {
        gridList = new NodeGrid[rowLeght, colLeght];

        for (int x = 0; x < rowLeght; x++)
        {
            for (int y = 0; y < colLeght; y++)
            {

                gridList[Mathf.Abs(x), Mathf.Abs(y)] = new NodeGrid(x, y);
                gridList[Mathf.Abs(x), Mathf.Abs(y)].nonWalkable = false;

                if (obstacleMap != null && obstacleMap.HasTile(new Vector3Int(x, y, 0)))
                {
                    gridList[Mathf.Abs(x), Mathf.Abs(y)].nonWalkable = true;
                }
                else if (Physics2D.OverlapCircle(new Vector3(x + 0.5f, y + 0.5f), 0.4f, LayerMask.GetMask("Blocked")))
                {
                    gridList[Mathf.Abs(x), Mathf.Abs(y)].nonWalkable = true;
                }
            }
        }
    }

    public List<NodeGrid> FindTarget(Vector3 seekPoint, Vector3 tarPoint)
    {
        GridCreate();

        List<NodeGrid> openPath = new List<NodeGrid>();
        HashSet<NodeGrid> closePath = new HashSet<NodeGrid>();

        NodeGrid seekerPoint = ContainNode(new Vector3((int)seekPoint.x, (int)seekPoint.y));
        NodeGrid targetPoint = ContainNode(new Vector3((int)tarPoint.x, (int)tarPoint.y));

        seekerPoint.GCost = 0;
        seekerPoint.HCost = GetDistance(seekerPoint, targetPoint);

        openPath.Add(seekerPoint);

        while (openPath.Count > 0)
        {
            NodeGrid node = openPath[0];
            for (int j = 1; j < openPath.Count; j++)
            {
                if (openPath[j].FCost <= node.FCost)
                    if (openPath[j].HCost < node.HCost)
                        node = openPath[j];
            }

            openPath.Remove(node);
            closePath.Add(node);

            if (node == targetPoint)
            {
                return RetracePath(seekerPoint, targetPoint);

            }

            foreach (NodeGrid neighbor in GetNeighbors(node))
            {
                if (neighbor.nonWalkable || closePath.Contains(neighbor))
                    continue;

                int newCost = node.GCost + GetDistance(node, neighbor);
                if (newCost < neighbor.GCost || !openPath.Contains(neighbor))
                {
                    neighbor.GCost = newCost;
                    neighbor.HCost = GetDistance(neighbor, targetPoint);
                    neighbor.parent = node;
                    GetNeighbors(neighbor);
                    openPath.Add(neighbor);
                }
            }
        }

        return null;
    }


    public List<NodeGrid> GetNeighbors(NodeGrid point)
    {
        List<NodeGrid> neighbors = new List<NodeGrid>();

        //Up
        if (point.GridY + 1 < colLeght)
        {
            neighbors.Add(ContainNode(new Vector3(point.GridX, point.GridY + 1)));
        }

        //Right
        if (point.GridX + 1 < rowLeght)
        {
            neighbors.Add(ContainNode(new Vector3(point.GridX + 1, point.GridY)));
        }

        //Left
        if (point.GridX - 1 >= 0)
        {
            neighbors.Add(ContainNode(new Vector3(point.GridX -1 , point.GridY)));
        }

        //Down
        if (point.GridY - 1 >= 0)
        {
            neighbors.Add(ContainNode(new Vector3(point.GridX, point.GridY - 1)));
        }

        #region HexagonMovement
        //Left-up
        if (point.GridX - 1 >= 0 && point.GridY + 1 < colLeght)
        {
            neighbors.Add(ContainNode(new Vector3(point.GridX - 1, point.GridY + 1)));
        }
        
        //Left-down
        if (point.GridX - 1 >= 0 && point.GridY - 1 >= 0)
        {
            neighbors.Add(ContainNode(new Vector3(point.GridX - 1, point.GridY - 1)));
        }

        //Right-up
        if (point.GridX + 1 < rowLeght && point.GridY + 1 < colLeght)
        {
            neighbors.Add(ContainNode(new Vector3(point.GridX + 1, point.GridY + 1)));
        }

        //Right-down
        if (point.GridX + 1 < rowLeght && point.GridY - 1 >= 0)
        {
            neighbors.Add(ContainNode(new Vector3(point.GridX + 1, point.GridY - 1)));
        }
        #endregion

        return neighbors;
    }

    private List<NodeGrid> RetracePath(NodeGrid pointA, NodeGrid pointB)
    {
        List<NodeGrid> pathF = new List<NodeGrid>();
        NodeGrid currentPath = pointB;

        while (currentPath != pointA)
        {
            pathF.Add(currentPath);
            currentPath = currentPath.parent;
        }

        pathF.Reverse();

        return pathF;
    }

    

    int GetDistance(NodeGrid pointA, NodeGrid pointB)
    {
        int dstX = Mathf.Abs(pointA.GridX - pointB.GridX);
        int dstY = Mathf.Abs(pointA.GridY - pointB.GridY);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }

    NodeGrid ContainNode(Vector3 points)
    {
        foreach(NodeGrid n in gridList)
        {
            if (n.GridX == points.x && n.GridY == points.y)
                return n;
        }
        return null;
    }

    void OnDrawGizmos()
    {
        if(showGizmos)
        {
            float addOffsetX = 0;
            float addOffsetY = 0;

            if (rowLeght % 2 != 0)
                addOffsetX = 0.5f;

            if (colLeght % 2 != 0)
                addOffsetY = 0.5f;

            Gizmos.DrawWireCube(new Vector3(rowLeght / 2 + addOffsetX, colLeght / 2 + addOffsetY), new Vector3(rowLeght, colLeght));
            if (gridList != null)
            {
                foreach (NodeGrid n in gridList)
                {
                    if (n.nonWalkable)
                        Gizmos.color = new Color(1, 0, 0, 0.3f);
                    else
                        Gizmos.color = new Color(1, 1, 1, 0.3f);

                    Gizmos.DrawCube(new Vector3(n.GridX + 0.5f, n.GridY + 0.5f), new Vector3(0.5f, 0.5f));
                }
            }
        }
    }
}
