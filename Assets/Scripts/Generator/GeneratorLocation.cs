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

        public LevelModel GenerateNewLocation(int coefXLoc)
        {
            
            Level.Size size = GenerateLocationSize();
            Vector2 labelSize = new Vector2(1.0f, 1.3f);
            GeneratorMazePlatform mazeGenerator = new GeneratorMazePlatform(labelSize);
            mazeGenerator.GenerateMaze(size.X, size.Y, 0.9f);
            Rect region = new Rect(0, 0, size.X * labelSize.x, size.Y* labelSize.y);
            var countCrystal = GetCountCrystal(coefXLoc);
            var staticplatform = GenerateStaticPlatformPosition(size);
            var specialplatform = GenerateSpecialPlatformPosition(staticplatform);
            var newModel = new LevelModel(
                GenerateCrystalPosition(size.X, size.Y, countCrystal,3),
                staticplatform,
                specialplatform,
                //GenerateBoundaryPlatforms(size),
                mazeGenerator.GenerateBorderPlatforms(region),
                countCrystal*GetScorePerCrystal(coefXLoc),
                countCrystal,
                size.Y, size.X
            );
            return newModel;
        }
        
        private Level.Size GenerateLocationSize()
        {
            var random = new System.Random();
            return new Level.Size
            {
                X = random.Next(15, 20), // Генерирует случайное число от 500 до 900 включительно
                Y = random.Next(10, 20) // Генерирует случайное число от 500 до 700 включительно
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

        private List<Vector3> GenerateCrystalPosition(int X, int Y, int numSamples, int relaxationSteps)
        {
            var positions = new List<Vector3>();
            for (int i = 0; i < numSamples; i++)
            {
                // Генерация случайной точки в пределах сетки
                var point = new Vector3(Random.Range(0, X), Random.Range(0, Y), 0);
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
                    Vector3 newSample = sample + direction * (X / (float)numSamples);

                    // Проверяем, не выходит ли новая точка за границы
                    if (newSample.x < 2) newSample.x = (X/2 - Random.RandomRange(0,5));
                    if (newSample.y < 2) newSample.y = Y/2- UnityEngine.Random.RandomRange(0,5);
                    if (newSample.x > X-2) newSample.x = X/2 - UnityEngine.Random.RandomRange(0,5);
                    if (newSample.y > Y-2) newSample.y = Y/2- UnityEngine.Random.RandomRange(0,5);

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
            var gen = new GeneratorPlatformPosition(new Vector2(3f, 4f), grid);
            //return gen.Position();
            //var gen2 = new GeneratorMazePlatform(new Vector2(2f,3f));
            //return gen2.GeneratePlatforms(new Rect(1,1,grid.X-1, grid.Y-1));

            Vector2 labelSize = new Vector2(1.0f, 1.3f);
            GeneratorMazePlatform mazeGenerator = new GeneratorMazePlatform(labelSize);
            mazeGenerator.GenerateMaze(grid.X, grid.Y, 0.9f);
            Rect region = new Rect(0, 0, grid.X * labelSize.x, grid.Y * labelSize.y);
            List<Vector3> platforms = mazeGenerator.GeneratePlatforms(region);
            return platforms;
        }
        private List<Vector3> GenerateSpecialPlatformPosition(List<Vector3> statics)
        {
            /*
            int platformCount = (int)GetPercentCountSpecialPlatform()*statics.Count; // или любое другое выражение, которое вам подходит
            var selectedPlatforms = statics.OrderBy(x => UnityEngine.Random.value).Take(platformCount).ToList();

            // Удаляем из staticplatform все платформы, которые есть в selectedPlatforms
            var remainingPlatforms = statics.Except(selectedPlatforms).ToList();
            return remainingPlatforms;*/
            int platformCount = (int)(GetPercentCountSpecialPlatform() * statics.Count); 
            var selectedPlatforms = new List<Vector3>();

            // Выбираем случайные платформы из statics и добавляем их в selectedPlatforms
            for (int i = 0; i < platformCount; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, statics.Count);
                selectedPlatforms.Add(statics[randomIndex]);
                statics.RemoveAt(randomIndex); // Удаляем выбранную платформу из исходного массива
            }
            
            return selectedPlatforms;
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
            float platformX = 1.1f;
            float platformY = 0.5f;

            // Расчет количества платформ для каждой стены
            int numPlatformsX = Mathf.CeilToInt((size.X ) / platformX)+2;
            int numPlatformsY = Mathf.CeilToInt((size.Y) / platformY)+2;

            // Создание платформ для каждой стены
            for (int i = -1; i < numPlatformsX-1; i++)
            {
                platforms.Add(new Vector3(i * platformX, -platformY*2, 0)); // Нижняя стена
                platforms.Add(new Vector3(i * platformX,  (size.Y)+platformY*2, 0)); // Верхняя стена
            }
            for (int i =-1; i < numPlatformsY-1; i++)
            {
                platforms.Add(new Vector3(-platformY*2, i * platformY, 0)); // Левая стена
                platforms.Add(new Vector3((size.X )+platformY*2, i * platformY, 0)); // Правая стена
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
