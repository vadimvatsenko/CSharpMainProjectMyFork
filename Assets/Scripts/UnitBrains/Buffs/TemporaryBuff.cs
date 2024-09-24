using System.Collections;
using System.Collections.Generic;
using UnitBrains;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

public class TemporaryBuff : IBuff
{
    private BuffService _buffService;
    private BaseUnitBrain _buffOwner;
    private IBuff _coreBuff;
    private float _lifeTime;
    private TimeUtil _timeUtil;

    public TemporaryBuff(BuffService buffService, BaseUnitBrain buffOwner, IBuff coreBuff, TimeUtil timeUtil, float lifeTime)
    {
        _buffService = buffService;
        _buffOwner = buffOwner;
        _coreBuff = coreBuff;
        _lifeTime = lifeTime;
        _timeUtil = timeUtil;
    }

    public Modifers ApplyBuff(Modifers config)
    {
        Modifers newConfig = _coreBuff.ApplyBuff(config);

        _buffService.RemoveBuff(_buffOwner);
        return newConfig;
    }

    
}
