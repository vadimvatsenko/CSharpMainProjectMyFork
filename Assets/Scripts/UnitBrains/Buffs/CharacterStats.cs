using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterStats 
{
    public float moveSpeed;
    public float shootSpeed;

    public CharacterStats(float moveSpeed, float shootSpeed)
    {
        this.moveSpeed = moveSpeed;
        this.shootSpeed = shootSpeed;
    }
}
