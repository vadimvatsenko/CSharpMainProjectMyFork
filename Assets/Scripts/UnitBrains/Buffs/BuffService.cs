using Model.Config;
using System.Collections;
using System.Collections.Generic;
using UnitBrains;
using UnityEngine;

public class BuffService: IBuffable
{
    public CharacterStats _baseStates { get; }
    public CharacterStats _currentStates { get; private set; }
    public Dictionary<BaseUnitBrain, IBuff> _buffs {  get; private set; }
    public BuffService(CharacterStats stats)
    {
        _buffs = new Dictionary<BaseUnitBrain, IBuff>();
        _baseStates = stats;
        _currentStates = _baseStates;
        Debug.Log("Create Service Buffs");
    }
    public void AddBuff(BaseUnitBrain brain, IBuff buff)
    {
        if(!_buffs.ContainsKey(brain)) _buffs.Add(brain, buff);
    }

    public void RemoveBuff(BaseUnitBrain brain)
    {
        _buffs.Remove(brain);
    }

    private void ApplyBuffs()
    {
        _currentStates = _baseStates;

        foreach (var buff in _buffs.Values)
        {
            buff.ApplyBuff(_currentStates);
        }
    }
}
