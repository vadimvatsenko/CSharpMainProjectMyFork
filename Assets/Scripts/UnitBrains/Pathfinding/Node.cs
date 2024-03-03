using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector2Int Pos; // позиция нашей ноды
    public int Cost = 10;
    public int Estimate;
    public int Value;
    public Node Parent;

    public Node(Vector2Int pos)
    {
        Pos = pos;
    }

    public void CalculateEstimate(Vector2Int targetPos)
    {
        Estimate = Mathf.Abs(Pos.x - targetPos.x) + Mathf.Abs(Pos.y - targetPos.y);
    }

    public void CalculateValue()
    {
        Value = Cost + Estimate;
    }

    public override bool Equals(object? obj)
    {
        if(obj is not Node node) return false;

        return Pos.x == node.Pos.x && Pos.y == node.Pos.y;

    }
}
