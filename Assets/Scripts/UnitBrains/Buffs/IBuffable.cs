using System.Collections;
using System.Collections.Generic;
using UnitBrains;
using UnityEngine;

public interface IBuffable
{
    public void AddBuff(BaseUnitBrain unit, IBuff buff);
    public void RemoveBuff(BaseUnitBrain unit);
}
