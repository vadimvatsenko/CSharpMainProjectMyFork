using Model.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFasterBuff : IBuff
{
    private float _indexSpeed;
    public MoveFasterBuff(int indexSpeed)
    {
        _indexSpeed = indexSpeed;
    }
    public Modifers ApplyBuff(Modifers config)
    {

        Modifers newConfig = config;
        newConfig.MovementAcceleration = _indexSpeed;

        return newConfig;
    }
}
