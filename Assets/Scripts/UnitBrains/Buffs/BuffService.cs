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
    public Modifers _baseStates { get; }
    public Modifers _currentStates { get; private set; }
    public Dictionary<BaseUnitBrain, IBuff> _buffs {  get; private set; }
    public BuffService(Modifers modifers)
    {
        _buffs = new Dictionary<BaseUnitBrain, IBuff>();
        _baseStates = modifers;
        _currentStates = _baseStates;
        Debug.Log("Create Service Buffs");
    }

    public void AddBuff(BaseUnitBrain unit, IBuff buff)
    {
        _buffs.Add(unit, buff);
        Debug.Log($"Бафф {buff} юнита {unit} добавлен");
    }

    public void RemoveBuff(BaseUnitBrain unit)
    {
        _buffs.Remove(unit); // удаление по ключу
        Debug.Log($"Бафф юнита {unit} Removed");
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
