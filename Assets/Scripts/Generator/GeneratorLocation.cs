using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using  Platformer2D.Level;
using System.Linq;


namespace Platformer2D.Generator
{
    public class GeneratorLocation
    {
        private int _difficulty;
        public GeneratorLocation()
        {
            _difficulty= PlayerPrefs.GetInt("Difficulty");
        }

        /*public LocationModel GenerateNewLocation(int indexLocation, LocationType type, int coefXLoc, int countDoor)
        {
            Level.Size size = GenerateLocationSize();
            var staticplatform = GenerateStaticPlatformPosition(size);
            var specialplatform = GenerateSpecialPlatformPosition(staticplatform);
            var newModel = new LocationModel(
                indexLocation,
                SelectPlatformCoordinateForChest(staticplatform),
                type,
                GetScorePerCrystal(coefXLoc),
                GetTargetScore(),
                new Vector3(2, 2, 0),
                size,
                GenerateCrystalPosition(size.Width, size.Height, GetCountCrystal(coefXLoc), 5),
                staticplatform,
                specialplatform,
                SelectPlatformCoordinatesForDoors(staticplatform, countDoor)
            );
            return newModel;
        }*/

        public LevelModel GenerateNewLocation(int coefXLoc)
        {
            Level.Size size = GenerateLocationSize();
            var countCrystal = GetCountCrystal(coefXLoc);
            var staticplatform = GenerateStaticPlatformPosition(size);
            var specialplatform = GenerateSpecialPlatformPosition(staticplatform);
            var newModel = new LevelModel(
                GenerateCrystalPosition(size.Width, size.Height, countCrystal,5),
                staticplatform,
                specialplatform,
                GenerateBoundaryPlatforms(size),
                countCrystal*GetScorePerCrystal(coefXLoc),
                countCrystal,
                size.Height, size.Width
            );
            return newModel;
        }
        
        private Level.Size GenerateLocationSize()
        {
            var random = new System.Random();
            return new Level.Size
            {
                Width = random.Next(15, 20), // Генерирует случайное число от 500 до 900 включительно
                Height = random.Next(10, 20) // Генерирует случайное число от 500 до 700 включительно
            };
        }
        private int GetScorePerCrystal(int locationIndex)
        {
            switch (locationIndex)
            {
                case 1:
                    return (int)(1+ 0.1f *_difficulty);
                case 2:
                    return (int)(2+ 0.1f *_difficulty);
                case 3:
                    return (int)(3+ 0.1f *_difficulty);
                default:
                    return 0;
            }
        }
        private int GetCountCrystal(int locationIndex)
        {
            switch (locationIndex)
            {
                case 1:
                    return (int)(UnityEngine.Random.Range(20,30) * (1+ 0.1f *_difficulty));
                case 2:
                    return (int)(UnityEngine.Random.Range(30,50) * (1+ 0.1f *_difficulty));
                case 3:
                    return (int)(UnityEngine.Random.Range(50,55) * (1+ 0.1f *_difficulty));
                default:
                    return 0;
            }
        }

        private int GetTargetScore()
        {
            switch (_difficulty)
            {
                case 1:
                    return 60;
                case 2:
                    return 70;
                case 3:
                    return 85;
                default:
                    return 0;
            }
        }

        private float GetPercentCountSpecialPlatform()
        {
            switch (_difficulty)
            {
                case 1:
                    return 0.1f;
                case 2:
                    return 0.2f;
                case 3:
                    return 0.3f;
                default:
                    return 0;
            }
        }

        private List<Vector3> GenerateCrystalPosition(int width, int height, int numSamples, int relaxationSteps)
        {
            var positions = new List<Vector3>();
            for (int i = 0; i < numSamples; i++)
            {
                // Генерация случайной точки в пределах сетки
                var point = new Vector3(Random.Range(0, width), Random.Range(0, height), 0);
                positions.Add(point); 
            }

            for (int _ = 0; _ < relaxationSteps; _++)
            {
                var newSamples = new List<Vector3>();
                foreach (var sample in positions)
                {
                    // Находим ближайшую точку
                    Vector3 nearest = FindNearest(sample, positions);
                    // Вычисляем направление от текущей точки к ближайшей
                    Vector3 direction = nearest - sample;
                    direction.Normalize();
                    // Двигаем точку на расстояние радиуса в направлении ближайшей точки
                    Vector3 newSample = sample + direction * (width / (float)numSamples);

                    // Проверяем, не выходит ли новая точка за границы
                    if (newSample.x < 0) newSample.x = -newSample.x;
                    if (newSample.y < 0) newSample.y = -newSample.y;
                    if (newSample.x > width) newSample.x = 2 * width - newSample.x;
                    if (newSample.y > height) newSample.y = 2 * height - newSample.y;

                    newSamples.Add(newSample);
                }
                positions = newSamples;
            }

            return positions;
        }


        private Vector3 FindNearest(Vector3 point, List<Vector3> points)
        {
            Vector3 nearest = points[0];
            float minDistance = Vector3.Distance(point, nearest);
            foreach (var p in points)
            {
                float distance = Vector3.Distance(point, p);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = p;
                }
            }
            return nearest;
        }

        private List<Vector3> GenerateStaticPlatformPosition(Level.Size grid)
        {
            var gen = new GeneratorPlatformPosition(new Vector2(1.1f, 0.2f), grid);
            return gen.Position();
        }
        private List<Vector3> GenerateSpecialPlatformPosition(List<Vector3> statics)
        {
            int platformCount = (int)GetPercentCountSpecialPlatform()*statics.Count; // или любое другое выражение, которое вам подходит
            var selectedPlatforms = statics.OrderBy(x => UnityEngine.Random.value).Take(platformCount).ToList();
            // Удаляем из staticplatform все платформы, которые есть в selectedPlatforms
            var remainingPlatforms = statics.Except(selectedPlatforms).ToList();
            return remainingPlatforms;
        }

        private List<Vector3> SelectPlatformCoordinatesForDoors(List<Vector3> coordinates,  int doorCount)
        {

            // Выбираем случайные координаты для дверей
            var doorCoordinates = coordinates.OrderBy(x => UnityEngine.Random.value).Take(doorCount).ToList();
            for (int i = 0; i < doorCoordinates.Count; i++)
            {
                doorCoordinates[i] = new Vector3(doorCoordinates[i].x, doorCoordinates[i].y + 1, doorCoordinates[i].z);
            }

            return doorCoordinates;
        }

        private List<Vector3> GenerateBoundaryPlatforms(Level.Size size)
        {
            var platforms = new List<Vector3>();

            // Размер платформы
            float platformWidth = 1.1f;
            float platformHeight = 0.5f;

            // Расчет количества платформ для каждой стены
            int numPlatformsWidth = Mathf.CeilToInt(size.Width / platformWidth);
            int numPlatformsHeight = Mathf.CeilToInt(size.Height / platformHeight);

            // Создание платформ для каждой стены
            for (int i = 0; i < numPlatformsWidth; i++)
            {
                platforms.Add(new Vector3(i * platformWidth, -platformHeight / 2, 0)); // Нижняя стена
                platforms.Add(new Vector3(i * platformWidth, size.Height + platformHeight / 2, 0)); // Верхняя стена
            }
            for (int i = 0; i < numPlatformsHeight; i++)
            {
                platforms.Add(new Vector3(-platformWidth / 2, i * platformHeight, 0)); // Левая стена
                platforms.Add(new Vector3(size.Width + platformWidth / 2, i * platformHeight, 0)); // Правая стена
            }

            return platforms;
        }


        private Vector3 SelectPlatformCoordinateForChest(List<Vector3> coordinates)
        {
            // Выбираем случайную координату для сундука
            var chestCoordinate = coordinates[UnityEngine.Random.Range(0, coordinates.Count)];

            // Поднимаем сундук на одну единицу выше платформы
            chestCoordinate = new Vector3(chestCoordinate.x, chestCoordinate.y + 1, chestCoordinate.z);

            return chestCoordinate;
        }
    }

}
