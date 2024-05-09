using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using Platformer2D.Crystal;
using Platformer2D.Platform;
using Platformer2D.Background;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

namespace Platformer2D.Level
{
    public struct Size
    {
        public int Width;
        public int Height;
    }
    public class LocationModel 
    {
        public int IndexLocation {get;set;}
        public bool IsChestLocation {get;set;}
        public LocationType TypeLocation {get;set;}
        public int TotalScore { get; set; }
        public int CurrentScore { get; set; }
        public int TargerScore {get;set;}

        public Dictionary<Vector3, CrystalModel> PositionCrystal {get;set;}
        public Dictionary<Vector3, PlatformModel> PositionPlatformStatic{get;set;}
        public Dictionary<Vector3, PlatformModel> PositionPlatformSpecial{get;set;}
        public Dictionary<Vector3, PlatformModel> PositionPlatformBounds{get;set;}
        public Dictionary<Vector3, DoorModel> PositionDoors {get;set;}

        public Vector3 PositionBackground {get;set;}
        public Vector3 PlayerPositionStart {get;set;}
       
        public Size SizeLocation{get;set;}

        public LocationModel(int indexLocation, bool isChestLocation, LocationType typeLocation, int totalScore, int targetScore,
            Vector3 playerPositionStart, Size sizeLocation,
            List<Vector3> crystalPositions, 
            List<Vector3> staticPlatformPositions,
            List<Vector3> specialPlatformPositions,
            List<Vector3> doorPositions,
            List<int> doorIndex
        )
        {
            //численые параметры уровня
            IndexLocation = indexLocation;
            IsChestLocation = isChestLocation;
            TypeLocation = typeLocation;
            TotalScore = totalScore;
            TargerScore = targetScore;
            CurrentScore = 0;
            PositionBackground = new Vector3 (0,0,0);

            //координаты уровня и персонажа на нем
            PlayerPositionStart = playerPositionStart;
            SizeLocation = sizeLocation;

            //координаты платформ, кристаллов, дверей

            PositionCrystal = new Dictionary<Vector3, CrystalModel>();
            foreach (var pos in crystalPositions)
            {
                PositionCrystal[pos] = new CrystalModel();
            }

            PositionPlatformStatic = new Dictionary<Vector3, PlatformModel>();
            foreach (var pos in staticPlatformPositions)
            {
                PositionPlatformStatic[pos] = new PlatformModel();
            }

            PositionPlatformSpecial = new Dictionary<Vector3, PlatformModel>();
            foreach (var pos in specialPlatformPositions)
            {
                PositionPlatformSpecial[pos] = new PlatformModel();
            }

            PositionDoors = new Dictionary<Vector3, DoorModel>();
            int difficulty = PlayerPrefs.GetInt("Difficulty");
            int countIndex=0;
            int[] threasholds = {60,75,90};
            int countforopen = (int)(threasholds[difficulty - 1] / 100.0 * PositionCrystal.Count);
            foreach (var pos in doorPositions)
            {
                PositionDoors[pos] = new DoorModel(TargerScore, countforopen, doorIndex[countIndex]);
                countIndex++;
            }

            CalculateScorePerCrystal();
            CalculatePlatformsTargetScore();
        }

        // Метод расчета количества очков, приходящихся на один кристалл
        private void CalculateScorePerCrystal()
        {
            int scorePerCrystal = TotalScore / (PositionCrystal.Count + 1);
            foreach (var crystal in PositionCrystal.Values)
            {
                crystal.Score = scorePerCrystal;
                crystal.Type = TypeLocation;
            }
        }

        //Метод расчета количества очков, приходящихся на каждую платформу 
        public void CalculatePlatformsTargetScore()
        {
            int totalObject = PositionPlatformStatic.Count + PositionPlatformSpecial.Count;
            int step = TotalScore/totalObject;
            int currentScore = 0;
            foreach (var platform in PositionPlatformStatic.Values)
            {
                currentScore +=step;
                platform.TargetScore = currentScore;
            }
            foreach (var platform in PositionPlatformSpecial.Values)
            {
                currentScore +=step;
                platform.TargetScore = currentScore;
            }
        }

        /*public void CalculateDoorValues()
        {
            int difficulty = PlayerPrefs.GetInt("Difficulty");
            foreach (var door in PositionDoors.Values)
            {
                door.TargetScore = TargerScore;
                door.TypeLocation = TypeLocation;
                int[] threasholds = {60,75,90};
                door.CountForOpen = (int)(threasholds[difficulty - 1] / 100.0 * PositionCrystal.Count);
            }
        }*/

        public void CreateBoundsPlatform(Vector2 size)
        {
            // Расчет количества платформ для каждой стены
            int numPlatformsWidth = Mathf.CeilToInt(SizeLocation.Width / size.x);
            int numPlatformsHeight = Mathf.CeilToInt(SizeLocation.Height / size.y);

            // Создание PlatformModel для каждой платформы в каждой стене
            PositionPlatformBounds = new Dictionary<Vector3, PlatformModel>();
            for (int i = 0; i < numPlatformsWidth; i++)
            {
                PositionPlatformBounds[new Vector3(i * size.x, 0, 0)] = new PlatformModel(); // Нижняя стена
                PositionPlatformBounds[new Vector3(i * size.x, SizeLocation.Height, 0)] = new PlatformModel(); // Верхняя стена
            }
            for (int i = 0; i < numPlatformsHeight; i++)
            {
                PositionPlatformBounds[new Vector3(0, i * size.y, 0)] = new PlatformModel(); // Левая стена
                PositionPlatformBounds[new Vector3(SizeLocation.Width, i * size.y, 0)] = new PlatformModel(); // Правая стена
            }

            int totalObject = PositionPlatformBounds.Count;
            int step = TotalScore/totalObject;
            int currentScore = 0;
            foreach (var platform in PositionPlatformBounds.Values)
            {
                currentScore +=step;
                platform.TargetScore = currentScore;
            }

        }

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

