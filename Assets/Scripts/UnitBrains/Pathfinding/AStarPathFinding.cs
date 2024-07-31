using Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnitBrains.Pathfinding;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class AStarPathFinding : BaseUnitPath
{
    private int[] dx = { -1, 0, 1, 0 };
    private int[] dy = { 0, 1, 0, -1 };

    public AStarPathFinding(IReadOnlyRuntimeModel runtimeModel, Vector2Int startPoint, Vector2Int endPoint) : base(runtimeModel, startPoint, endPoint)
    {
        
    }

    protected override void Calculate()
    {
        List<Nodes> pathList = FindPath();

        if (pathList == null || pathList.Count == 0)
        {
            Debug.LogError("Path not found");
            return;
        }

        path = FindPath().Select(p => new Vector2Int(p.x, p.y)).ToArray();
    }

    public List<Nodes> FindPath()
    {
        Nodes startNode = new Nodes(startPoint.x, startPoint.y);
        Nodes targetNode = new Nodes(endPoint.x, endPoint.y);

        List<Nodes> openList = new List<Nodes>() { startNode};
        List<Nodes> closedList = new List<Nodes>();

        while(openList.Count > 0 )
        {
            Nodes currentNode = openList[0];
            
            foreach(Nodes node in openList)
            {                              
                if (node.value < targetNode.value) currentNode = node;              
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if(currentNode.x == targetNode.x && currentNode.y == targetNode.y || closedList.Count > runtimeModel.RoMap.Width * runtimeModel.RoMap.Height)
            {
                
                List<Nodes> newPath = new List<Nodes>();
                
                while (currentNode != null)
                {
                    newPath.Add(currentNode);
                    Debug.Log("Add pathNode");
                    currentNode = currentNode.parent;
                }

                path.Reverse();
                return newPath;
            }

            for(int i = 0; i < dx.Length; i++)
            {
                int newX = currentNode.x + dx[i];
                int newY = currentNode.y + dy[i];
                
                if(IsValid(new Vector2Int(newX, newY)))
                {
                    Nodes neighbor = new Nodes(newX, newY);

                    if (closedList.Contains(neighbor)) continue;

                    neighbor.parent = currentNode;
                    neighbor.CalculateEstimate(targetNode.x, targetNode.y);
                    neighbor.CalculateValue();

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                    
                }
            }

        }
        return null;
    }

    private bool IsValid(Vector2Int cell)
    {
        bool isValidX = cell.x >=0 && cell.x < runtimeModel.RoMap.Width;
        bool isValidY = cell.y >=0 && cell.y < runtimeModel.RoMap.Height;

        return isValidX && isValidY && (runtimeModel.IsTileWalkable(cell));
    }

}


