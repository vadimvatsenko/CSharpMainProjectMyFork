using Model.Config;
using System.Collections;
using System.Collections.Generic;
using UnitBrains;
using UnityEngine;

/*- ускорение передвижения,
- ускорение атаки,
- замедление передвижения,
- замедление атаки*/

public class BuffService: IBuffable
{
    public UnitConfig _baseStates { get; }
    public UnitConfig _currentStates { get; private set; }

    Dictionary<BaseUnitBrain, IBuff> _buffs = new Dictionary<BaseUnitBrain, IBuff>();
    public BuffService()
    {
        _baseStates = MonoBehaviour.FindAnyObjectByType<UnitConfig>();
        _currentStates = _baseStates;
        Debug.Log("Create Service Buffs");

        Debug.Log(_currentStates);
    }

    public void AddBuff(BaseUnitBrain unit, IBuff buff)
    {
        _buffs.Add(unit, buff);
    }

    public void RemoveBuff(BaseUnitBrain unit, IBuff buff)
    {
        _buffs.Remove(unit); // удаление по ключу
    }

    private void ApplyBuffs()
    {
        _currentStates = _baseStates;

        foreach(var buff in _buffs)
        {
            _currentStates = buff.Value.ApplyBuff(_currentStates);
        }
    }
}
