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
        private RuntimeModel _runtimeModel => ServiceLocator.Get<RuntimeModel>();

        VFXView _vfxView => ServiceLocator.Get<VFXView>();
        
        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {               
            List<BaseProjectile> projecttilesForThisUnit = new List<BaseProjectile>(0);          
        }

        public override Vector2Int GetNextStep()
        {
            var target = runtimeModel.RoMap.Bases[IsPlayerUnitBrain ? RuntimeModel.BotPlayerId : RuntimeModel.PlayerId];

            _activePath = new DummyUnitPath(runtimeModel, unit.Pos, target);
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
            
            List<IReadOnlyUnit> friendUnits = GetFriendlyUnit();

            if(friendUnits.Count != 0)
            {
                foreach (var u in friendUnits)
                {
                    Debug.Log(u is FourthUnitBrain);
                    if (u == this.unit) continue;
                    //Debug.Log(u.UnitID);
                    _buffService.TempBuff(u.UnitID, _buffService.GetRandomBuff(), 5f);
                    _vfxView.PlayVFX(u.Pos, VFXView.VFXType.BuffApplied);
                                      
                }
            }
        }

        
        private List<IReadOnlyUnit> GetFriendlyUnit()
        {           
            var units = new List<IReadOnlyUnit>();
            var pos = unit.Pos;

            foreach (var friendUnit in runtimeModel.RoPlayerUnits)
            {
                if(Vector2Int.Distance(this.unit.Pos, friendUnit.Pos) < 2f)
                {
                    units.Add(friendUnit);
                }
            }

            return units;
        }
    }
}