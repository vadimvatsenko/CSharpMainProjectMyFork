using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSlowlyBuff : IBuff
{
    public void ApplyBuff(CharacterStats stats)
    {
        CharacterStats newStats = stats;
        newStats.moveSpeed /= 2;
    }
}
