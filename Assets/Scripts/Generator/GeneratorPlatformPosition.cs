 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Drawing;
using System.Linq;

namespace Platformer2D
{
    public class GeneratorPlatformPosition 
    {
        private Vector2 labelSize;
        private Size grid;
        public GeneratorPlatformPosition (Vector2 labelSize, Size grid)
        {
            this.labelSize = labelSize;
            this.grid = grid;
        }

        public List<Vector3> Position()
        {
            var platforms = new List<Vector3>();

            int randomNumber = UnityEngine.Random.Range(2, 5);
            var regions = RecursiveBinarySpacePartition(new Rect(0, 0, grid.Width, grid.Height), randomNumber, true);

            var regionIndices = new List<int>(Enumerable.Range(0, regions.Count));
            Shuffle(regionIndices);
            for (int i = 0; i < regions.Count; i++)
            {
                var region = regions[i];
                List<Vector3> regionPlatforms;
                
                regionPlatforms = GridBasedPlatformPlacementWithoutIntersection(4, (int)region.width, labelSize, region);
                
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
                var split = UnityEngine.Random.Range(0.4f, 0.6f); // Случайное разбиение пространства
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

        private List<Vector3> GridBasedPlatformPlacementWithoutIntersection(int pointCount, int gridSize, Vector2 labelSize, Rect region)
        {
            var platforms = new List<Vector3>();
            for (int i = 0; i < pointCount; i++)
            {
                // Генерация случайной точки в пределах сетки
                var point = new Vector2(UnityEngine.Random.Range(region.x, region.xMax), UnityEngine.Random.Range(region.y, region.yMax));
                // Размещение платформы ряд
                    platforms.Add(new Vector3(point.x, point.y, 0));
            }
                return platforms;
        }
    }
}
