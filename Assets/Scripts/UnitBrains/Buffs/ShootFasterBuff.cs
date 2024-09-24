using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFasterBuff : IBuff
{
    private float _indexSpeed;

    public ShootFasterBuff(float indexSpeed)
    {
        _indexSpeed = indexSpeed;
    }

    public Modifers ApplyBuff(Modifers config)
    {
        Modifers newConfig = config;
        newConfig.AttackAcceleration += _indexSpeed;
        return newConfig;
    }
}
