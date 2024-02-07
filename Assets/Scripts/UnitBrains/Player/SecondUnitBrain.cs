using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Runtime.Projectiles;
using UnityEngine;
using UnityEngine.UIElements;
// Start HomeWork-7
namespace UnitBrains.Player
{
    public class SecondUnitBrain : DefaultPlayerUnitBrain
    {
        public override string TargetUnitName => "Cobra Commando";
        private const float OverheatTemperature = 3f;
        private const float OverheatCooldown = 2f;
        private float _temperature = 0f;
        private float _cooldownTime = 0f;
        private bool _overheated;

        // ДЗ-7
        private const int MAXTARGETSFORATTACK = 4; // максимально число юнитов для атаки 
        private static int _playerIdCount = 0; // счётчик ID
        private int _playerUnitId; // id SecondUnitBrain
        public static int unitIndexer = 0;
        private List<Vector2Int> _targetsToMove = new List<Vector2Int>(); // поле для всех опасных целей // ДЗ-6
        public SecondUnitBrain() // конструктор для создания ID
        {
            _playerUnitId = _playerIdCount++;
        }

        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {
            float overheatTemperature = OverheatTemperature;
            float temp = GetTemperature();

            if (temp >= overheatTemperature)
            {
                return;
            }

            IncreaseTemperature();

            for (int i = 0; i <= temp; i++)
            {
                var projectile = CreateProjectile(forTarget);
                AddProjectileToList(projectile, intoList);
            }
        }

        public override Vector2Int GetNextStep()
        {
            Vector2Int currentTarget = Vector2Int.zero; // завел вспомогательную переменную в которой будут координаты самой опасной цели на данный момент, по умолчанию будет x=0, y=0

            if (_targetsToMove.Count > 0)
            {
                currentTarget = _targetsToMove[0]; // если список _allCurrentTargets не пуст, присвой координаты цели со списка _allCurrentTargets под индексом 0 (так как он там единственный)
            }
            else
            {
                currentTarget = unit.Pos; // если список пуст, то присвой координаты самого себя, стой на месте, совершай выстрелы на расстоянии.
            }

            if (IsTargetInRange(currentTarget)) // если цель в зоне досягаемости, то стой на месте. вернёт позицию самого юнита. Атака на расстоянии.
            {
                return unit.Pos; // позиция самого себя(юнита)
            }
            else
            {
                return CalcNextStepTowards(currentTarget); // в противном случае верни позицию самого опасного врага к которому нужно ехать
            }

        }


        protected override List<Vector2Int> SelectTargets()
        {
            List<Vector2Int> _allTargets = new List<Vector2Int>();
            List<Vector2Int> _targetsForAttack = new List<Vector2Int>();
            int indexCurrentTargetForAttack = _playerUnitId % MAXTARGETSFORATTACK;

            _targetsToMove.Clear();
            _allTargets.Clear();

            foreach (var target in GetAllTargets())
            {
                _allTargets.Add(target);
            }

            SortByDistanceToOwnBase(_allTargets);

            Debug.Log(_playerUnitId);

            if (_allTargets.Count == 1)
            {
                if (IsTargetInRange(_allTargets[0]))
                {
                _targetsForAttack.Add(_allTargets[0]);
                } 
                else
                {
                    _targetsToMove.Add(_allTargets[0]);
                }
            }

            else if (_allTargets.Count == 0 && IsPlayerUnitBrain)
            {
                var enemyBaseTarget = runtimeModel.RoMap.Bases[
                IsPlayerUnitBrain ? RuntimeModel.BotPlayerId : RuntimeModel.PlayerId];
                _targetsForAttack.Add(enemyBaseTarget);
            }

            else if (_allTargets.Count > 1)
            {
                int index = indexCurrentTargetForAttack - 1;

                if (index < 0)
                {
                    index = 0; // Если индекс отрицательный, устанавливаем его в 0
                }
                else if(index >= _allTargets.Count)
                {
                    index = 0; // Если индекс больше или равен количеству элементов в списке, устанавливаем его в индекс 1й элемент
                }

                if (IsTargetInRange(_allTargets[index]))
                {
                    _targetsForAttack.Add(_allTargets[index]);
                }
                else
                {
                    _targetsToMove.Add(_allTargets[index]);
                }

            }

            return _targetsForAttack;

        }

        public override void Update(float deltaTime, float time)
        {
            if (_overheated)
            {
                _cooldownTime += Time.deltaTime;
                float t = _cooldownTime / (OverheatCooldown / 10);
                _temperature = Mathf.Lerp(OverheatTemperature, 0, t);
                if (t >= 1)
                {
                    _cooldownTime = 0;
                    _overheated = false;
                }
            }
        }

        private int GetTemperature()
        {
            if (_overheated) return (int)OverheatTemperature;
            else return (int)_temperature;
        }

        private void IncreaseTemperature()
        {
            _temperature += 1f;
            if (_temperature >= OverheatTemperature) _overheated = true;
        }
    }
}