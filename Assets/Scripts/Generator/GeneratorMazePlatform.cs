using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D
{
    public class GeneratorMazePlatform 
    {
        private Vector2 labelSize;
        private int[,] maze;

        public GeneratorMazePlatform(Vector2 labelSize)
        {
            this.labelSize = labelSize;
            Debug.Log($"[Generator][MazePlatform] labelSize = {labelSize}");
        }
        /*public List<Vector3> GenerateBorderPlatforms(Rect region)
        {
            var platforms = new List<Vector3>();
            var cellWidth = region.width / maze.GetLength(1);
            var cellHeight = region.height / maze.GetLength(0);

            // Генерируем верхнюю и нижнюю границы
            for (int x = 0; x < maze.GetLength(1); x++)
            {
                platforms.Add(new Vector3(region.x + x * cellWidth + cellWidth / 2 - labelSize.x / 2, region.y - cellHeight / 2, 0)); // Верхняя граница
                platforms.Add(new Vector3(region.x + x * cellWidth + cellWidth / 2 - labelSize.x / 2, region.y + region.height - cellHeight / 2, 0)); // Нижняя граница
            }

            // Генерируем левую и правую границы
            for (int y = 0; y < maze.GetLength(0); y++)
            {
                platforms.Add(new Vector3(region.x - cellWidth / 2, region.y + y * cellHeight + cellHeight / 2 - labelSize.y / 2, 0)); // Левая граница
                platforms.Add(new Vector3(region.x + region.width - cellWidth / 2, region.y + y * cellHeight + cellHeight / 2 - labelSize.y / 2, 0)); // Правая граница
            }

            return platforms;
        }*/

        public List<Vector3> GenerateBorderPlatforms(Rect region)
        {
            var platforms = new List<Vector3>();
            var cellWidth = region.width / maze.GetLength(1);
            var cellHeight = region.height / maze.GetLength(0);

            // Генерируем верхнюю и нижнюю границы
            for (int x = -2; x < maze.GetLength(1); x++)
            {
                platforms.Add(new Vector3(region.x + x * cellWidth + cellWidth / 2 - labelSize.x / 2, region.y - cellHeight / 2, 0)); // Верхняя граница
                platforms.Add(new Vector3(region.x + x * cellWidth + cellWidth / 2 - labelSize.x / 2, region.y + region.height - cellHeight / 2, 0)); // Нижняя граница
            }

            // Генерируем левую и правую границы
            for (int y = 0; y < maze.GetLength(0); y++)
            {
                platforms.Add(new Vector3(region.x - cellWidth / 2 - 2, region.y + y * cellHeight + cellHeight / 2 - labelSize.y / 2, 0)); // Левая граница
                platforms.Add(new Vector3(region.x + region.width - cellWidth / 2, region.y + y * cellHeight + cellHeight / 2 - labelSize.y / 2, 0)); // Правая граница
            }

            return platforms;
        }

        // Генерация лабиринта
        public void GenerateMaze(int width, int height, float removalProbability)
        {
            Debug.Log($"[Generator][MazePlatform] width = {width}");
            Debug.Log($"[Generator][MazePlatform] height = {height}");
            Debug.Log($"[Generator][MazePlatform] removalProbability = {removalProbability}");
            maze = new int[height, width];
            Stack<Vector2Int> stack = new Stack<Vector2Int>();
            stack.Push(new Vector2Int(0, 0));

            while (stack.Count > 0)
            {
                Vector2Int current = stack.Peek();
                maze[current.y, current.x] = 1;

                List<Vector2Int> neighbors = new List<Vector2Int>();

                if (current.x > 1) neighbors.Add(new Vector2Int(current.x - 2, current.y));
                if (current.x < width - 2) neighbors.Add(new Vector2Int(current.x + 2, current.y));
                if (current.y > 1) neighbors.Add(new Vector2Int(current.x, current.y - 2));
                if (current.y < height - 2) neighbors.Add(new Vector2Int(current.x, current.y + 2));

                neighbors.Shuffle(); // Now you can shuffle the neighbors

                bool found = false;
                foreach (var neighbor in neighbors)
                {
                    if (maze[neighbor.y, neighbor.x] == 0)
                    {
                        stack.Push(neighbor);
                        maze[(current.y + neighbor.y) / 2, (current.x + neighbor.x) / 2] = 1;
                        if (Random.value < removalProbability)
                        {
                            maze[current.y, current.x] = 0; // Remove the platform with the defined probability
                        }
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    stack.Pop();
                }
            }
        }

        public List<Vector3> GeneratePlatforms(Rect region)
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
    public static class ListExtensions
    {
        private static System.Random rng = new System.Random();

        // Fisher-Yates shuffle algorithm
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
