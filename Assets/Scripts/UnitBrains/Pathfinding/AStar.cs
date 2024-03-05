using Model;
using Model.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnitBrains.Pathfinding;
using UnityEngine;
using Utilities;


namespace Assets.Scripts.UnitBrains.Pathfinding
{
    internal class AStar : BaseUnitPath
    {
        private Unit unit;
        private int[] dx = { -1, 0, 1, 0 };
        private int[] dy = { 0, 1, 0, -1 };
        public AStar(IReadOnlyRuntimeModel runtimeModel, Vector2Int startPoint, Vector2Int endPoint) : base(runtimeModel, startPoint, endPoint) // runtimeModel - является ли клетка проходимой
        {
        }

        protected override void Calculate()
        {
            Node startNode = new Node(startPoint.x, startPoint.y);
            Node targetNode = new Node(endPoint.x, endPoint.y);

            List<Node> openList = new List<Node>() {startNode };
            List<Node> closedList = new List<Node>();

            while(openList.Count > 0)
            {
                Node currentNode = openList[0];

                foreach(var node in openList)
                {
                    if(node.Value < currentNode.Value)
                    {
                        currentNode = node;
                    }
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                if(currentNode.X == targetNode.X && currentNode.Y == targetNode.Y)
                {
                    List<Vector2Int> _path = new List<Vector2Int>();

                    while (currentNode != null)
                    {
                        _path.Add(new Vector2Int(currentNode.X, currentNode.Y));
                        currentNode = currentNode.Parent;
                    }
                    _path.Reverse();

                    path = _path.ToArray();

                }
                for (int i = 0; i < dx.Length; i++) 
                {
                    int newX = currentNode.X + dx[i];
                    int newY = currentNode.Y + dy[i];

                    if (runtimeModel.IsTileWalkable(new Vector2Int(newX, newY)))
                    {
                        Node neighbor = new Node(newX, newY);

                        if (closedList.Contains(neighbor))
                            continue;

                        neighbor.Parent = currentNode;
                        neighbor.CalculateEstimate(targetNode.X, targetNode.Y);
                        neighbor.CalculateValue();

                        openList.Add(neighbor);
                    }
                }                
            }
        }
    }
}




