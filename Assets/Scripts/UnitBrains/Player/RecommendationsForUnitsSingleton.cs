using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class RecommendationsForUnitsSingleton
{
    private IReadOnlyRuntimeModel _runtimeModel;
    private TimeUtil _timeUtil;

    private static RecommendationsForUnitsSingleton _instance;
    private RecommendationsForUnitsSingleton() 
    {
        _runtimeModel = ServiceLocator.Get<IReadOnlyRuntimeModel>();
        _timeUtil = ServiceLocator.Get<TimeUtil>();
    }

    public static RecommendationsForUnitsSingleton GetInstance()
    {
        if (_instance == null)
        {
            _instance = new RecommendationsForUnitsSingleton();
        }

        return _instance;
    }

    public void UpdateRuntimeModel(IReadOnlyRuntimeModel runtimeModel) // метод который будет обновлять runtimeModel из вне
    {
        _runtimeModel = runtimeModel;
    }

}
