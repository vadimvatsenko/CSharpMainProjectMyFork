using Model.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public struct Modifers
{
    public float MovementAcceleration;
    public float MovementSlowing;
    public float AttackAcceleration;
    public float AttackSlowdown;

   /* public Modifers()
    {
        MovementAcceleration = ServiceLocator.Get<UnitConfig>().MoveDelay;
        MovementSlowing = ServiceLocator.Get<UnitConfig>().MoveDelay;
        AttackAcceleration = ServiceLocator.Get<UnitConfig>().AttackDelay;
        AttackSlowdown = ServiceLocator.Get<UnitConfig>().AttackDelay;
    }*/

}
