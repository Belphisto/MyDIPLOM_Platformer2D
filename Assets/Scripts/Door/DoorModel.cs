using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace Platformer2D.Platform
{
    public class DoorModel 
    {
        private int _targetScore;
        private Vector3 _startPosition;
        public Vector3 StartPosition { get => _startPosition; set => _startPosition = value; }
        public int TargetScore {get => _targetScore; set => _targetScore = value;}
        public int CountForOpen;
        public bool IsColor {get; set;}
        public bool IsOpen {get; set;}
        public int IndexDoor {get;set;}
        public LocationType TypeDoor {get; set;}
        public LocationType TypeLocation {get; set;}
        public List<int> IndexLocation{get;set;}

        // Конструктор класса PlatformModel
        // Принимает целевой счет
        public DoorModel(int score, int countCrystal, int index)
        {
            _targetScore = score;
            CountForOpen = countCrystal;
            IndexDoor = index;
            IsColor = false;
            IsOpen = false;
        }
    }
}

