using System.Collections.Generic;
using Model.Runtime;
using Model.Runtime.ReadOnly;
using UnityEngine;

namespace Model
{
    public interface IReadOnlyRuntimeModel
    {
        IReadOnlyMap RoMap { get; } // карта
        RuntimeModel.GameStage Stage { get; } // текущее состояние игры
        public int Level { get; } // уровень
        public IReadOnlyDictionary<int, int> RoMoney { get; } // деньги
        public IEnumerable<IReadOnlyUnit> RoUnits { get; } // все юниты
        public IEnumerable<IReadOnlyProjectile> RoProjectiles { get; } // все RoProjectiles

        public IEnumerable<IReadOnlyUnit> RoPlayerUnits { get; } // все юниты принадлежащее игроку
        public IEnumerable<IReadOnlyUnit> RoBotUnits { get; } // все юниты принадлежащее врагу
        public IReadOnlyList<IReadOnlyBase> RoBases { get; } // все базы

        public bool IsTileWalkable(Vector2Int pos); // является ли клетка проходимой
    }
}