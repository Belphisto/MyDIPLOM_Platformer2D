using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using System.Linq;

namespace Platformer2D.Generator
{
    public class GeneratorGridPlatform : MonoBehaviour
    {
        private Vector2 labelSize;
        private Size grid;
        public GeneratorGridPlatform(Vector2 labelSize, Size grid)
        {
            this.labelSize = labelSize;
            this.grid = grid;
            Debug.Log($"[Generator][GeneratorGridPlatform] labelSize = {labelSize}");
            Debug.Log($"[Generator][GeneratorGridPlatform] grid = {grid}");
        }

        public List<Vector3> PositionStaticPlatforms()
        {
            var platforms = new List<Vector3>();

            int randomNumber = 5;
            var regions = RecursiveBinarySpacePartition(new Rect(0, 0, grid.Width, grid.Height), randomNumber, true);

            Debug.Log($"[Generator][GeneratorGridPlatform] regions = {regions.Count}");

            var regionIndices = new List<int>(Enumerable.Range(0, regions.Count));
            Shuffle(regionIndices);

            for (int i = 0; i < regions.Count; i++)
            {
                var region = regions[i];
                List<Vector3> regionPlatforms;

                regionPlatforms = PlacePlatformsInRegion(region);

                platforms.AddRange(regionPlatforms);
            }
            return platforms;
        }

        // Метод для перемешивания списка (алгоритм Фишера-Йетса)
        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        private List<Rect> RecursiveBinarySpacePartition(Rect area, int depth, bool horizontal)
        {
            var regions = new List<Rect>();
            if (depth == 0)
            {
                regions.Add(area);
            }
            else
            {
                var split = UnityEngine.Random.Range(0.4f, 0.8f); // Случайное разбиение пространства
                Debug.Log($"[Generator][GeneratorGridPlatform] split = {split}");
                Rect first, second;
                if (horizontal)
                {
                    first = new Rect(area.x, area.y, area.width * split, area.height);
                    second = new Rect(area.x + area.width * split, area.y, area.width * (1 - split), area.height);
                }
                else
                {
                    first = new Rect(area.x, area.y, area.width, area.height * split);
                    second = new Rect(area.x, area.y + area.height * split, area.width, area.height * (1 - split));
                }
                regions.AddRange(RecursiveBinarySpacePartition(first, depth - 1, !horizontal));
                regions.AddRange(RecursiveBinarySpacePartition(second, depth - 1, !horizontal));
            }
            return regions;
        }

        private List<Vector3> PlacePlatformsInRegion(Rect region)
        {
            var platforms = new List<Vector3>();

            // Добавление входа и выхода в регион
            platforms.Add(AddEntryPoint(region));
            platforms.Add(AddExitPoint(region));

            // Соединение входа и выхода платформами по минимальному расстоянию
            ConnectEntryAndExit(platforms, region);

            // Размещение дополнительных платформ в пустых областях
            platforms.AddRange(FillEmptyAreaWithPlatforms(region, platforms));

            // Удаление излишних платформ (например, оставляем 20% от исходного количества)
            platforms = RemoveExcessPlatforms(platforms, 0.2f);

            return platforms;
        }

        private Vector3 AddEntryPoint(Rect region)
        {
            // Логика для добавления входа в регион
            return new Vector3(region.x, region.y, 0);
        }

        private Vector3 AddExitPoint(Rect region)
        {
            // Логика для добавления выхода из региона
            return new Vector3(region.xMax, region.yMax, 0);
        }

        private void ConnectEntryAndExit(List<Vector3> platforms, Rect region)
        {
            Vector3 entryPoint = platforms[0];
            Vector3 exitPoint = platforms[1];

            float minDistance = Mathf.Sqrt(Mathf.Pow(exitPoint.x - entryPoint.x, 2) + Mathf.Pow(exitPoint.y - entryPoint.y, 2));

            int numPlatforms = Mathf.CeilToInt(minDistance / labelSize.x);
            float dx = (exitPoint.x - entryPoint.x) / numPlatforms;
            float dy = (exitPoint.y - entryPoint.y) / numPlatforms;

            for (int i = 1; i < numPlatforms; i++)
            {
                Vector3 platformPosition = new Vector3(entryPoint.x + i * dx, entryPoint.y + i * dy, 0);
                platforms.Add(platformPosition);
            }
        }

        private List<Vector3> FillEmptyAreaWithPlatforms(Rect region, List<Vector3> existingPlatforms)
        {
            var newPlatforms = new List<Vector3>();

            int regionWidth = Mathf.CeilToInt(region.width);
            int regionHeight = Mathf.CeilToInt(region.height);

            bool[,] grid = new bool[regionWidth, regionHeight];

            foreach (Vector3 platform in existingPlatforms)
            {
                int x = Mathf.FloorToInt(platform.x - region.x);
                int y = Mathf.FloorToInt(platform.y - region.y);

                if (x >= 0 && x < regionWidth && y >= 0 && y < regionHeight)
                {
                    grid[x, y] = true;
                }
            }

            for (int x = 0; x < regionWidth; x++)
            {
                for (int y = 0; y < regionHeight; y++)
                {
                    if (!grid[x, y])
                    {
                        Vector3 platformPosition = new Vector3(x + region.x, y + region.y, 0);
                        newPlatforms.Add(platformPosition);
                    }
                }
            }

            return newPlatforms;
        }

        private List<Vector3> RemoveExcessPlatforms(List<Vector3> platforms, float targetPlatformRatio)
        {
            var keptPlatforms = new List<Vector3>();
            int targetPlatformCount = Mathf.CeilToInt(platforms.Count * targetPlatformRatio);

            if (targetPlatformCount > 0)
            {
                int startIndex = UnityEngine.Random.Range(0, platforms.Count);
                keptPlatforms.Add(platforms[startIndex]);
                platforms.RemoveAt(startIndex);

                while (keptPlatforms.Count < targetPlatformCount)
                {
                    Vector3 lastKeptPlatform = keptPlatforms.Last();
                    int nearestIndex = -1;
                    float nearestDistance = float.MaxValue;

                    for (int i = 0; i < platforms.Count; i++)
                    {
                        float distance = Vector3.Distance(platforms[i], lastKeptPlatform);
                        if (distance < nearestDistance)
                        {
                            nearestIndex = i;
                            nearestDistance = distance;
                        }
                    }
                    keptPlatforms.Add(platforms[nearestIndex]);
                    platforms.RemoveAt(nearestIndex);
                }
            }

            return keptPlatforms;
        }

        public List<Vector3> GenerateBoundaryPlatforms()
        {
            var platforms = new List<Vector3>();

            // Размер платформы
            float platformX = 1.1f;
            float platformY = 0.85f;

            // Расчет количества платформ для каждой стены
            int numPlatformsX = Mathf.CeilToInt((grid.Width) / platformX) + 3;
            int numPlatformsY = Mathf.CeilToInt((grid.Height) / platformY) + 3;

            // Создание платформ для каждой стены
            for (int i = -1; i < numPlatformsX - 1; i++)
            {
                platforms.Add(new Vector3(i * platformX, -platformY * 2.2f, 0)); // Нижняя стена
                platforms.Add(new Vector3(i * platformX, (grid.Height) + platformY * 2.2f, 0)); // Верхняя стена
            }
            for (int i = -1; i < numPlatformsY - 1; i++)
            {
                if (i % 2 == 0)
                {
                    platforms.Add(new Vector3(-platformY * 2.2f, i * platformY, 0)); // Левая стена
                    platforms.Add(new Vector3((grid.Width) + platformY * 2.2f, i * platformY, 0)); // Правая стена
                }
            }

            return platforms;
        }
    }
}