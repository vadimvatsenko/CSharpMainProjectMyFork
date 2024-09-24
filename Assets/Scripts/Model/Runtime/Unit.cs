using System.Collections.Generic;
using System.Linq;
using Model.Config;
using Model.Runtime.Projectiles;
using Model.Runtime.ReadOnly;
using UnitBrains;
using UnitBrains.Pathfinding;
using UnityEngine;
using Utilities;
using static UnityEngine.UI.CanvasScaler;

namespace Model.Runtime
{
    public class Unit : IReadOnlyUnit
    {
        public UnitConfig Config { get; }
        public Vector2Int Pos { get; private set; }
        public int Health { get; private set; }
        public bool IsDead => Health <= 0;
        public BaseUnitPath ActivePath => _brain?.ActivePath;
        public IReadOnlyList<BaseProjectile> PendingProjectiles => _pendingProjectiles;

        private readonly List<BaseProjectile> _pendingProjectiles = new();
        private IReadOnlyRuntimeModel _runtimeModel;
        private BaseUnitBrain _brain;

        private float _nextBrainUpdateTime = 0f;
        private float _nextMoveTime = 0f;
        private float _nextAttackTime = 0f;

        // ДЗ-12
        private BuffService _buffService => ServiceLocator.Get<BuffService>(); // ссылка на систему
        private Modifers _unitModifers; // ссылка на модификаторы

        private List<IBuff> _buffsList = new List<IBuff> // список из баффов
        {
            new MoveFasterBuff(Random.Range(100, 150)), // быстрей
            new ShootFasterBuff(Random.Range(100, 150)), // быстрей
            new MoveFasterBuff(Random.Range(0, 100)), // медленней
            new ShootFasterBuff(Random.Range(0, 100)), // медленней
        };
        
        //

        public Unit(UnitConfig config, Vector2Int startPos, RecommendationsForUnitsSingleton recommendationsForUnitsSingleton) // 4. добавлена зависимость
        {
            Config = config;
            Pos = startPos;
            Health = config.MaxHealth;
            _brain = UnitBrainProvider.GetBrain(config);
            _brain.SetUnit(this, recommendationsForUnitsSingleton); // 5.добавлена зависимость
            _runtimeModel = ServiceLocator.Get<IReadOnlyRuntimeModel>();

            // ДЗ-12

            _unitModifers = new Modifers() // создаю модификаторы для юнита
            {
                AttackAcceleration = this.Config.AttackDelay,
                MovementAcceleration = this.Config.MoveDelay,
                MovementSlowing = this.Config.MoveDelay,
                AttackSlowdown = this.Config.AttackDelay,
            };

            ApplyRandomBuff();
        }

        private void ApplyRandomBuff()
        {
            var randomBuff = _buffsList[Random.Range(0, _buffsList.Count)];
            _buffService.AddBuff(_brain, randomBuff);
        }

        public void Update(float deltaTime, float time)
        {
            
            if (IsDead)
                return;
            
            if (_nextBrainUpdateTime < time)
            {
                _nextBrainUpdateTime = time + Config.BrainUpdateInterval;
                _brain.Update(deltaTime, time);
            }
            
            if (_nextMoveTime < time)
            {

                IBuff moveSpeedBuff = _buffService._buffs[_brain];
                float moveSpeed = moveSpeedBuff.ApplyBuff(_unitModifers).MovementAcceleration;

                Debug.Log($"default speed {Config.MoveDelay} vs MoveSpeed {moveSpeed}");
                //_nextMoveTime = time + Config.MoveDelay;
                _nextMoveTime = time + moveSpeed;
                Move();
            }
            
            if (_nextAttackTime < time && Attack())
            {
                float shootSpeed = _unitModifers.AttackAcceleration;
                //Debug.Log($"default shoot {Config.AttackDelay} vs shootSpeed {shootSpeed}");
                _nextAttackTime = time + Config.AttackDelay;
                //_nextAttackTime = time + shootSpeed;
                
            }
        }

        private bool Attack()
        {
            var projectiles = _brain.GetProjectiles();
            if (projectiles == null || projectiles.Count == 0)
                return false;
            
            _pendingProjectiles.AddRange(projectiles);
            return true;
        }

        private void Move()
        {
            var targetPos = _brain.GetNextStep();
            var delta = targetPos - Pos;
            if (delta.sqrMagnitude > 2)
            {
                Debug.LogError($"Brain for unit {Config.Name} returned invalid move: {delta}");
                return;
            }

            if (_runtimeModel.RoMap[targetPos] ||
                _runtimeModel.RoUnits.Any(u => u.Pos == targetPos))
            {
                return;
            }
            
            Pos = targetPos;
        }

        public void ClearPendingProjectiles()
        {
            _pendingProjectiles.Clear();
        }

        public void TakeDamage(int projectileDamage)
        {
            Health -= projectileDamage;
        }
    }
}