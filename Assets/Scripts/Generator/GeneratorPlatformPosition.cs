 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Platformer2D.Level;
using System.Linq;

namespace Platformer2D
{
    public class GeneratorPlatformPosition 
    {
        //private int platformX;
        //private int platformY;
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

            int randomNumber = UnityEngine.Random.Range(4, 10);
            var regions = RecursiveBinarySpacePartition(new Rect(0, 0, grid.X, grid.Y), 2, true);
            int mazeRegionCount =0;  // 10% of regions will have a maze
            var regionIndices = new List<int>(Enumerable.Range(0, regions.Count));
            Shuffle(regionIndices);
            for (int i = 0; i < regions.Count; i++)
            {
                var region = regions[i];
                List<Vector3> regionPlatforms;
                if (i < mazeRegionCount)
                {
                    // Generate a maze for this region
                    var maze = GenerateMaze((int)region.width, (int)region.height, region);
                    regionPlatforms = MazeToPlatforms(maze, region);
                }
                else
                {
                    // Fill this region with random platforms
                    // countPlatform  на регион надо расчитывать, пока передается 10
                    regionPlatforms = GridBasedPlatformPlacementWithoutIntersection(4, (int)region.width, labelSize, region);
                }
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
            
        public int[,] GenerateMaze(int width, int height, Rect region)
        {
            var maze = new int[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    maze[i, j] = 1;
                }
            }

            void Generate(int x, int y)
            {
                var directions = new List<Vector2> { new Vector2(0, -2), new Vector2(0, 3), new Vector2(-2, 0), new Vector2(3, 0) };
                var shuffledDirections = directions.OrderBy(a => UnityEngine.Random.value).ToList();
                foreach (var direction in shuffledDirections)
                {
                    var nx = x + (int)direction.x;
                    var ny = y + (int)direction.y;
                    if (nx >= 0 && nx < width && ny >= 0 && ny < height && maze[ny, nx] == 1)
                    {
                        maze[y - (int)region.y + (int)direction.y / 2, x - (int)region.x + (int)direction.x / 2] = 0;
                        maze[ny - (int)region.y, nx - (int)region.x] = 0;
                        Generate(nx, ny);
                    }
                }
            }

            Generate(UnityEngine.Random.Range((int)region.x, (int)region.xMax), UnityEngine.Random.Range((int)region.y, (int)region.yMax));

            // Добавление случайных "прорезей" в клетках
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    if (UnityEngine.Random.value < 0.1f)  // Вероятность создания "прорези"
                    {
                        maze[y, x] = 0;
                    }
                 }
            }

            return maze;
        }

        public List<Vector3> MazeToPlatforms(int[,] maze, Rect region)
        {
            var platforms = new List<Vector3>();
            var cellWidth = region.width / maze.GetLength(1);
            var cellHeight = region.height / maze.GetLength(0);
            for (int y = 0; y < maze.GetLength(0); y++)
            {
                for (int x = 0; x < maze.GetLength(1); x++)
                {
                    if (maze[y, x] == 0)
                    {
                        var platformX = region.x + x * cellWidth + cellWidth / 2 - labelSize.x / 2;
                        var platformY = region.y + y * cellHeight + cellHeight / 2 - labelSize.y / 2;
                        platforms.Add(new Vector3(platformX, platformY, 0));
                    }
                }
            }
            return platforms;
        }
    }

}
