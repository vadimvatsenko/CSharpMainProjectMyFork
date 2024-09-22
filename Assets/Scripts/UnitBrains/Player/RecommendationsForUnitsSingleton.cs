using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class RecommendationsForUnitsSingleton
{
    public IReadOnlyRuntimeModel _runtimeModel { get; private set; }
    private TimeUtil _timeUtil;
    public Vector2Int _playerBasePos { get; private set; }
    public Vector2Int _enemyBasePos { get; private set; }

    private event Action UpdateRuntimeModelEvent;

    public RecommendationsForUnitsSingleton(RuntimeModel runtimeModel, TimeUtil timeUtil)
    {
        _runtimeModel = _runtimeModel;
        _timeUtil = timeUtil;
        UpdateRuntimeModelEvent += UpdateRuntimeModel;

        _timeUtil.AddFixedUpdateAction(UpdateRuntimeModelWrapperForEvent);

        _playerBasePos = _runtimeModel.RoMap.Bases[RuntimeModel.PlayerId];
        _enemyBasePos = _runtimeModel.RoMap.Bases[RuntimeModel.BotPlayerId];
    }
    ~RecommendationsForUnitsSingleton()
    {
        Debug.Log("Singleton delete");
    }

    public void Dispose()
    {
        _timeUtil.RemoveFixedUpdateAction(UpdateRuntimeModelWrapperForEvent);
        UpdateRuntimeModelEvent -= UpdateRuntimeModel;
    }

    public void UpdateRuntimeModelWrapperForEvent(float time) // ��������
    {
        UpdateRuntimeModel();
    }

    public void UpdateRuntimeModel() // ����� ������� ����� ��������� runtimeModel �� ���
    {
        _runtimeModel = ServiceLocator.Get<IReadOnlyRuntimeModel>();

    }

    public Vector2Int RecommendationTarget()
    {
        Vector2Int centerPointBetweenBases = _playerBasePos + (_playerBasePos - _enemyBasePos) / 2; // ����� �����
        float minEnemyHealth = float.MaxValue; // ����������� �������� �����
        Vector2Int targetPos = Vector2Int.zero; // ������� �������

        foreach (var enemy in _runtimeModel.RoBotUnits)
        {
            float distance = Vector2Int.Distance(enemy.Pos, _runtimeModel.RoMap.Bases[RuntimeModel.PlayerId]); // ��������� ����� ����� ���� � ������

            
            if (distance <= centerPointBetweenBases.magnitude)
            {
                Debug.Log($"Enemy with pos = {enemy.Pos} on my side");
                return enemy.Pos; // ���������� ������� ����� �� ������� ������
            }

            // ���� ����� � ���������� ���������
            if (minEnemyHealth > enemy.Health)
            {
                minEnemyHealth = enemy.Health;
                targetPos = enemy.Pos; // ��������� ������� ����� � ���������� ���������
               
            }
        }

        // ���� ������ �� ������� ������ ���, ������� ����� � ���������� ��������� ��� ����, ���� ������ �� ��������

        return targetPos == Vector2Int.zero? _runtimeModel.RoMap.Bases[RuntimeModel.BotPlayerId] : targetPos;

    }
}
