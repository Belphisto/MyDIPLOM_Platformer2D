using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Platformer2D.Crystal;
using Platformer2D.Platform;
using Platformer2D.Background;

/*
Класс LevelModel представляет модель данных для уровня игры.
 Он содержит информацию о префабах для кристаллов, платформ и фона, 
 позициях для размещения кристаллов и платформ, общем счете, количестве кристаллов и текущем счете. 
 Класс также предоставляет методы для управления моделью текущего счета: увеличения и уменьшения текущего счета. 
 Класс связывает данные об уровне.
*/

namespace Platformer2D.Level
{
    // Класс LevelModel представляет модель данных для игрового уровня
    public class LevelModel 
    {
        public List<GameObjectModel> Crystal { get; set; }
        public List<GameObjectModel> Platform { get; set; }
        public Dictionary<GameObjectModel, DoorView> Doors {get;set;}
        public List<GameObjectModel> SpecialPlatform { get; set; }
        public List<GameObjectModel> Bounds { get; set; }
        public GameObjectModel Background { get; set; }

        // Общий счет, количество кристаллов и текущий счет игрока
        public float Width { get; set; }
        public float Height { get; set; }

        public int TotalScore { get; set; }
        public int CrystalCount { get; set; }
        public int CurrentScore { get; set; }

        public LevelModel(List<Vector3> crystalPositions,
                List<Vector3> platformPositions,
                List<Vector3> specialPlatformPositions, 
                List<Vector3> boundsPosition,
                //List<Vector3> doorsPositions,
                //List<LocationType> locations, 
                int score, int count, float x, float y)
        {
            Crystal = crystalPositions.Select(pos => new GameObjectModel { Position = pos }).ToList();
            Platform = platformPositions.Select(pos => new GameObjectModel { Position = pos }).ToList();
            SpecialPlatform = specialPlatformPositions.Select(pos => new GameObjectModel { Position = pos }).ToList();
            Bounds = boundsPosition.Select(pos => new GameObjectModel { Position = pos }).ToList();
            Background = new GameObjectModel { Position = new Vector3 (0,0,0) };
            //Door = platformPositions.Select(pos => new GameObjectModel { Position = pos }).ToList();

            TotalScore = score;
            CrystalCount = count;
            CurrentScore = 0;
            Width = x;
            Height = y;

            //Bounds = new List<GameObjectModel>();
        }

        /*public void Boundarycalculation(Vector2 size)
        {
            // Расчет количества платформ для каждой стены
            int numPlatformsWidth = Mathf.CeilToInt(Width / size.x);
            int numPlatformsHeight = Mathf.CeilToInt(Height / size.y);

            // Создание GameObjectModel для каждой платформы в каждой стене
            
            for (int i = 0; i < numPlatformsWidth; i++)
            {
                Bounds.Add(new GameObjectModel { Position = new Vector3(i * size.x, 0, 0) }); // Нижняя стена
                Bounds.Add(new GameObjectModel { Position = new Vector3(i * size.x, Height, 0) }); // Верхняя стена
            }
            for (int i = 0; i < numPlatformsHeight; i++)
            {
                Bounds.Add(new GameObjectModel { Position = new Vector3(0, i * size.y, 0) }); // Левая стена
                Bounds.Add(new GameObjectModel { Position = new Vector3(Width, i * size.y, 0) }); // Правая стена
            }
        }*/

        // Метод для увеличения текущего счета
        public void IncrementScore(int amount)
        {
            CurrentScore += amount;
        }

        // Метод для уменьшения текущего счета
        public void DecrementScore(int amount)
        {
            CurrentScore -= amount;
        }

        public int GetPercentLevel()
        {
            return ((int)100*CurrentScore/TotalScore);
        }

    }
}

