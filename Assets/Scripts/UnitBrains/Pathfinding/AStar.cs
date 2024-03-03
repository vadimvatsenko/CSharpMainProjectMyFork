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
            Node startNode = new Node(startPoint);
            Node targetNode = new Node(endPoint);

            List<Node> openList = new List<Node> { startNode };
            List<Node> closedList = new List<Node>();

            while (openList.Count > 0)
            {
                Node currentNode = openList[0];

                foreach (var node in openList)
                {
                    if (node.Value < targetNode.Value)
                    {
                        currentNode = node;
                    }
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                if (currentNode.Pos.x == targetNode.Pos.x && currentNode.Pos.y == targetNode.Pos.y)
                {
                    //List<Node> _path = new List<Node>();
                    List<Vector2Int> _path = new List<Vector2Int>();

                    while (currentNode != null)
                    {
                        _path.Add(currentNode.Pos);
                        currentNode = currentNode.Parent;
                    }

                    //_path.Reverse();
                    path = path.ToArray();
                    return;

                }

                for (int i = 1; i < dx.Length; i++)
                {
                    int newX = currentNode.Pos.x + dx[i];
                    int newY = currentNode.Pos.y + dy[i];

                    Vector2Int newPos = new Vector2Int(newX, newY);

                    if (runtimeModel.IsTileWalkable(newPos))
                    {
                        Node neighbor = new Node(newPos);
                        if (closedList.Contains(neighbor)) continue;

                        neighbor.Parent = currentNode;
                        neighbor.CalculateEstimate(targetNode.Pos);
                        neighbor.CalculateValue();

                        if (!openList.Contains(neighbor))
                        {
                            openList.Add(neighbor);
                        }
                    }
                }
            }

        }

    }
}




