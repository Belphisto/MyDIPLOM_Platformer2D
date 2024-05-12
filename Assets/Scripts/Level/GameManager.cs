using System.Collections;
using System.Collections.Generic;
using Platformer2D.Level;
using UnityEngine;
using Platformer2D.Generator;
using Platformer2D.Platform;

namespace Platformer2D
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] public LevelView redLevelPrefab;
        [SerializeField] public LevelView greenLevelPrefab;
        [SerializeField] public LevelView blueLevelPrefab;
        [SerializeField] public LevelView skyLevelPrefab;

        [SerializeField] public DoorView redDoorPrefab;
        [SerializeField] public DoorView greenDoorPrefab;
        [SerializeField] public DoorView blueDoorPrefab;
        [SerializeField] public DoorView skyDoorPrefab;
        [SerializeField] public ChestView chestPrefab;

        private Dictionary<LocationType, LevelView> levelPrefabs;
        private GeneratorGraph _locationNetwork;

        private GeneratorLocation generatorLocations;
        private Dictionary<int, (int, int)> doorToLocations;

        // Start is called before the first frame update
        void Start()
        {
            levelPrefabs = new Dictionary<LocationType, LevelView>
            {
                { LocationType.Red, redLevelPrefab},
                { LocationType.Green, greenLevelPrefab},
                { LocationType.Blue, blueLevelPrefab},
                { LocationType.Sky, skyLevelPrefab},
            };
            

            var (countLocation, countStartVertix) = GetCountLocationWithDifficilty();
            _locationNetwork = new GeneratorGraph(countLocation, countStartVertix);
            generatorLocations = new GeneratorLocation();
            doorToLocations = GeneratorGraph.GraphToLocations(_locationNetwork);
            GeneratorGraph.PrintGraphToLocations(_locationNetwork);
            
            TestCreateLocation();
        }

        private void TestCreateLocation()
        {
            // индекс и тип локации
            int locationIndex = _locationNetwork.ChestLocationIndex;
            LocationType locationType = _locationNetwork.Rooms[locationIndex];
            int edgeCount = _locationNetwork.Transitions[locationIndex].Count;
            // Выбираем соответствующий префаб LevelView
            LevelView levelPrefab = levelPrefabs[locationType];
            // Создаем экземпляр префаба LevelView
            LevelModel newModel = generatorLocations.GenerateNewLocation(2);
            LevelView levelInstance = Instantiate(levelPrefab);
            levelInstance.model = newModel;
            levelInstance.SetModel();
            // добавить как-то двери
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        private (int,int) GetCountLocationWithDifficilty()
        {
            int difficulty = PlayerPrefs.GetInt("Difficulty");
            switch (difficulty)
            {
                case 1:
                    return (5, 1); // для уровня сложности 1
                case 2:
                    return (10, 1); // для уровня сложности 2
                case 3:
                    return (15, 2); // для уровня сложности 3
                default:
                    return (10, 1); // значения по умолчанию
            }
        }
    }

}
