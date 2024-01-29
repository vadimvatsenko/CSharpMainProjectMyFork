using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Model;
using Model.Runtime.Projectiles;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

            List<Vector2Int> targets = SelectTargets(); // получаем коллекцию самого опасного врага

            Vector2Int moveToCurrentTarget = new(); // переменная, которая будет хранить позицию, куда должны идти Юниты

            var baseTarget = runtimeModel.RoMap.Bases[
                IsPlayerUnitBrain ? RuntimeModel.BotPlayerId : RuntimeModel.PlayerId]; // позиция Вражеской базы, проверка

            if(targets.Count > 0) // если есть цели в targets
            {
                foreach (var target in targets)
                {
                    if (!IsTargetInRange(target) && target != null) // если target самый близкий и не в зоне досягаемости, запиши цель в маршрут
                    {
                        moveToCurrentTarget = target;
                    }
                }
            } else
            {
                moveToCurrentTarget = baseTarget; // в противном случае добавь в маршрут базу Врага.
            }

            return CalcNextStepTowards(moveToCurrentTarget); // расчёт маршрута
        }


        protected override List<Vector2Int> SelectTargets()
        {
  
            List<Vector2Int> mostDangerousTarget = new List<Vector2Int>();   // список с самым опасным Врагом

            Vector2Int mostDangerousTargetPosition = Vector2Int.zero; // позиция самого опасного врага

            float enemyWithMinDistanceToBaseValue = float.MaxValue; // промежуточная переменная для хранения минимального расстояния от Врага к Базе

            foreach (Vector2Int dangerTarget in GetAllTargets()) // перебираем в цикле все вражеские цели на карте, метод GetAllTargets() получит все цели.
            {

                float enemyDistanceToBaseValue = DistanceToOwnBase(dangerTarget); // промежуточная переменная для врага в которой будет хранится абсолютное расстояние от Врага к Базе

                if (enemyDistanceToBaseValue < enemyWithMinDistanceToBaseValue) // условие, дистанция от нашей базы до врага меньше enemyWithMinDistanceToBaseValue
                {
                    enemyWithMinDistanceToBaseValue = enemyDistanceToBaseValue; // то, запишем эту дистанцию в enemyWithMinDistanceToBaseValue

                    mostDangerousTargetPosition = dangerTarget; // позиция самого опасного врага, запишем её в Vector2Int mostDangerousTargetPosition
                }
            }

            mostDangerousTarget.Clear(); // очищаем коллекцию самого опасного Врага, перед записью.

            if (enemyWithMinDistanceToBaseValue < float.MaxValue)
            {
                mostDangerousTarget.Add(mostDangerousTargetPosition); // добавляем в коллекцию самого опасного Врага
            }

            return mostDangerousTarget; // возвращаем коллекцию
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