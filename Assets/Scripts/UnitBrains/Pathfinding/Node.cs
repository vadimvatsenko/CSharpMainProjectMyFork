using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int X;
    public int Y;
    public int Cost = 10;
    public int Estimate;
    public int Value;
    public Node Parent;

    public Node(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void CalculateEstimate(int targetX, int targetY)
    {
        Estimate = Mathf.Abs(X - targetX) + Mathf.Abs(Y - targetY);
    }

    public void CalculateValue()
    {
        Value = Cost + Estimate;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Node node) return false;

        return X == node.X && Y == node.Y;

    }
}
