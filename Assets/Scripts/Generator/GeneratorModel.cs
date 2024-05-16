using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
namespace Platformer2D.Generator
{
    public static class GeneratorModel 
    {
        public static int GetScorePerCrystal(int locationIndex, int _difficulty)
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
        public static int GetCountCrystal(int locationIndex, int _difficulty)
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

        public static int GetTargetScore(int _difficulty)
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

        public static int GetTargetCountForOpenPercent(int _difficulty)
        {
            switch (_difficulty)
            {
                case 1:
                    return 20;
                case 2:
                    return 30;
                case 3:
                    return 50;
                default:
                    return 0;
            }
        }

        public static float GetPercentCountSpecialPlatform(int _difficulty)
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

        public static Size GenerateLocationSize()
        {
            var random = new System.Random();
            return new Size
            {
                Width = random.Next(15, 20), // Генерирует случайное число от 500 до 900 включительно
                Height = random.Next(10, 20) // Генерирует случайное число от 500 до 700 включительно
            };
        }
    }
}

