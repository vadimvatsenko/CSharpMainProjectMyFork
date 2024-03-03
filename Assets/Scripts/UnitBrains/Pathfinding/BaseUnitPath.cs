using System.Collections.Generic;
using Model;
using UnityEngine;

namespace UnitBrains.Pathfinding
{
    public abstract class BaseUnitPath // базовый класс для всех путей // расчёт пути от точки до точки
    {
        public Vector2Int StartPoint => startPoint; // стартовая точка
        public Vector2Int EndPoint => endPoint; // конечная точка
        
        protected readonly IReadOnlyRuntimeModel runtimeModel; // является ли клетка проходимой
        protected readonly Vector2Int startPoint;
        protected readonly Vector2Int endPoint;
        protected Vector2Int[] path = null; // массив из тайлов или клеток на карте
        protected abstract void Calculate(); // метод для просчёта путей
        
        public IEnumerable<Vector2Int> GetPath()
        {
            if (path == null)
                Calculate(); // тут вызывается
            
            return path;
        }

        public Vector2Int GetNextStepFrom(Vector2Int unitPos) // возвращает следующую ячейку от позиции
        {
            var found = false;
            foreach (var cell in GetPath()) // проходим по всем клеткам
            {
                if (found)
                    return cell; // вернули следующую клетку

                found = cell == unitPos; // ищем клетку которая равна позиции игрока
            }

            Debug.LogError($"Unit {unitPos} is not on the path");
            return unitPos;
        }

        protected BaseUnitPath(IReadOnlyRuntimeModel runtimeModel, Vector2Int startPoint, Vector2Int endPoint)
        {
            this.runtimeModel = runtimeModel;
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }
    }
}