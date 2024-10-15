using System.Collections.Generic;
using Model;
using Model.Runtime.Projectiles;
using Model.Runtime.ReadOnly;
using UnitBrains.Pathfinding;
using UnityEngine;
using Utilities;
using View;

namespace UnitBrains.Player
{
    public class FourthUnitBrain : DefaultPlayerUnitBrain
    {
        public override string TargetUnitName => "Buff Buddy";
        private const float OverheatTemperature = 3f;
        private const float OverheatCooldown = 2f;
        private float _temperature = 0f;
        private float _cooldownTime = 0f;
        private bool _overheated;

        private BaseUnitPath _activePath;

        // ДЗ-13
        VFXView _vfxView => ServiceLocator.Get<VFXView>();
        
        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {
            float overheatTemperature = OverheatTemperature;
                  
            var projectile = CreateProjectile(forTarget);
            
            // ДЗ-13 убрал projectile, он не должен стрелять
            List<BaseProjectile> projecttilesForThisUnit = new List<BaseProjectile>();
            AddProjectileToList(projectile, projecttilesForThisUnit);
           
        }

        public override Vector2Int GetNextStep()
        {
            var target = runtimeModel.RoMap.Bases[IsPlayerUnitBrain ? RuntimeModel.BotPlayerId : RuntimeModel.PlayerId];

            _activePath = new DummyUnitPath(runtimeModel, unit.Pos, runtimeModel.RoMap.Bases[RuntimeModel.PlayerId]);
            return _activePath.GetNextStepFrom(unit.Pos);
        }

        protected override List<Vector2Int> SelectTargets()
        {
            List<Vector2Int> result = GetReachableTargets();
            while (result.Count > 1)
            {
                result.RemoveAt(result.Count - 1);
            }
            return result;
        }

        public override void Update(float deltaTime, float time)
        {
            if (_overheated)
            {              
                _cooldownTime += Time.deltaTime;
                float t = _cooldownTime / (OverheatCooldown/10);
                _temperature = Mathf.Lerp(OverheatTemperature, 0, t);
                if (t >= 1)
                {
                    _cooldownTime = 0;
                    _overheated = false;
                }
            }

            // ДЗ-13
            /* if (HasTargetsInRange() && !_isBuffble)
             {
                 _isBuffble = true;
                 _buffService.TempBuff(unit.UnitID, _buffService.GetRandomBuff(), 5f);
                 _vfxView.PlayVFX(unit.Pos, VFXView.VFXType.BuffApplied);
             }*/

            List<IReadOnlyUnit> frienUnits = GetFriendlyUnit();

            if(frienUnits.Count != 0)
            {
                foreach (var u in frienUnits)
                {
                    if (u == this.unit) continue;
                    
                    _vfxView.PlayVFX(u.Pos, VFXView.VFXType.BuffApplied);
                                      
                }
            }
        }

        private int GetTemperature()
        {
            if(_overheated) return (int) OverheatTemperature;
            else return (int)_temperature;
        }

        private void IncreaseTemperature()
        {
            _temperature += 1f;
            if (_temperature >= OverheatTemperature) _overheated = true;
        }

        private List<IReadOnlyUnit> GetFriendlyUnit()
        {
           
            var units = new List<IReadOnlyUnit>();
            var pos = unit.Pos;

            foreach (var friedUnit in runtimeModel.RoPlayerUnits)
            {
                if(Vector2Int.Distance(this.unit.Pos, friedUnit.Pos) < 2f)
                {
                    units.Add(friedUnit);
                }
            }

            return units;
        }
    }
}