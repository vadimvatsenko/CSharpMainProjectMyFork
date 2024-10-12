using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSlowlyBuff : IBuff
{
    public void ApplyBuff(CharacterStats stats)
    {
        CharacterStats newStats = stats;
        newStats.shootSpeed /= 2;
    }
}
