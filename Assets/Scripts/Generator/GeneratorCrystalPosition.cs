using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer2D.Generator
{
    public class GeneratorCrystalPosition
    {
        public List<Vector3> GeneratePoissonDiskSamples(int width, int height, int numSamples, int relaxationSteps = 5)
{
    float radius = 3.0f;
    List<Vector3> samples = new List<Vector3>();
    System.Random rand = new System.Random();

    for (int i = 0; i < numSamples; i++)
    {
        samples.Add(new Vector3((float)rand.NextDouble() * width, (float)rand.NextDouble() * height, 0));
    }

    for (int step = 0; step < relaxationSteps; step++)
    {
        List<Vector3> centroids = ComputeCentroids(samples, width, height);
        for (int i = 0; i < samples.Count; i++)
        {
            if (i < centroids.Count)  // Make sure we have a corresponding centroid
            {
                Vector3 centroid = centroids[i];
                Vector3 direction = centroid - samples[i];
                if (direction.magnitude > float.Epsilon)  // Avoid normalizing zero vector
                {
                    direction.Normalize();
                    samples[i] += direction * radius;
                }
            }
        }
    }

    return samples;
}

private List<Vector3> ComputeCentroids(List<Vector3> samples, int width, int height)
{
    List<Vector3> centroids = new List<Vector3>();
    int cellSize = 10;  // Define cell size explicitly
    int numCellsX = (int)System.Math.Ceiling((float)width / cellSize);
    int numCellsY = (int)System.Math.Ceiling((float)height / cellSize);
    
    int[,] numPoints = new int[numCellsX, numCellsY];
    Vector3[,] cumulativePositions = new Vector3[numCellsX, numCellsY];

    foreach (Vector3 sample in samples)
    {
        int cellX = System.Math.Clamp((int)(sample.x / cellSize), 0, numCellsX - 1);
        int cellY = System.Math.Clamp((int)(sample.y / cellSize), 0, numCellsY - 1);
        
        numPoints[cellX, cellY]++;
        cumulativePositions[cellX, cellY] += sample;
    }

    for (int x = 0; x < numCellsX; x++)
    {
        for (int y = 0; y < numCellsY; y++)
        {
            if (numPoints[x, y] > 0)
            {
                centroids.Add(cumulativePositions[x, y] / numPoints[x, y]);
            }
        }
    }

    return centroids;
}

        public List<Vector3> GenerateCrystalPosition(int X, int Y, int numSamples, int relaxationSteps)
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
                    if (newSample.x < 2) newSample.x = (X / 2 - Random.Range(0, 5));
                    if (newSample.y < 2) newSample.y = Y / 2 - Random.Range(0, 5);
                    if (newSample.x > X - 2) newSample.x = X / 2 - Random.Range(0, 5);
                    if (newSample.y > Y - 2) newSample.y = Y / 2 - Random.Range(0, 5);

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
    }

}
