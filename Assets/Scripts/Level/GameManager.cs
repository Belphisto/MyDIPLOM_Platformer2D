using System.Collections;
using System.Collections.Generic;
using Platformer2D.Level;
using UnityEngine;
using Platformer2D.Generator;
using Platformer2D.Platform;
using System.Linq;
using UnityEngine.Diagnostics;
using Platformer2D.Player;

namespace Platformer2D
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] public LevelView redLevelPrefab;
        [SerializeField] public LevelView greenLevelPrefab;
        [SerializeField] public LevelView blueLevelPrefab;
        [SerializeField] public LevelView skyLevelPrefab;

        private Dictionary<LocationType, LevelView> levelPrefabs;
        private GeneratorGraph _locationNetwork;

        private GeneratorLocation generatorLocations;
        private Dictionary<int, (int, int)> doorToLocations;
        private Dictionary<int, ((int, LocationType), (int, LocationType))>doorToLocation;
        private Dictionary<int, LevelView> createdLevels;
        private Dictionary<int, DoorModel> doorModels;
        private List<int> indexCreatedLocation;
        private List<LevelView> levelViews;
        private int indexCurrentLocation;

        // Start is called before the first frame update
        void Start()
        {
            Bus.Instance.GetGameResults = GetRoomInfo;
            Bus.Instance.IndexNextLocation += CreateOrLoadLocation;
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

            doorToLocation = GeneratorGraph.GraphToLocations(_locationNetwork.Rooms, _locationNetwork.Transitions);

            createdLevels = new Dictionary<int, LevelView>();
            levelViews = new List<LevelView>();
            doorModels = new Dictionary<int, DoorModel>();
            indexCreatedLocation = new List<int>();

            CreateAllDoors();

            indexCurrentLocation = 0;
            StartNewLocation(indexCurrentLocation);
        }
        private void OnDestroy()
        {
            Bus.Instance.IndexNextLocation -= CreateOrLoadLocation;
        }

        private void CreateOrLoadLocation(int indexNext)
        {

            DeactivateAllLevels();
            if (createdLevels.ContainsKey(indexNext))
            {
                createdLevels[indexNext].gameObject.SetActive(true);
                PlayerController.Instance.CurrentType = createdLevels[indexNext].crystalType;
                Debug.Log($"[GameManager] createdLevels[{indexNext}].gameObject.SetActive(true)");
            }
            else
            {
                Debug.Log($"[GameManager] StartNewLocation(indexNext) {indexNext}");
                StartNewLocation(indexNext);
            }
        }

        private void StartNewLocation(int index)
        {
            indexCurrentLocation = index;
            var doorInNewModel = GetDoorsForIndexLocation(index);
            LocationType locationType = _locationNetwork.Rooms[index];
            LevelView levelPrefab = levelPrefabs[locationType];
            LevelModel newModel;
            int difficulty = PlayerPrefs.GetInt("Difficulty");
            if (index == 0)
            {
                newModel = generatorLocations.GenerateNewLocation(2, index, doorInNewModel, GeneratorModel.GetCountForChest(difficulty));
            }
            else
            {
                newModel = generatorLocations.GenerateNewLocation(2, index, doorInNewModel);
            }
            
            LevelView levelInstance = Instantiate(levelPrefab);
            createdLevels[index] = levelInstance; //  добавление уровня в словарь
            levelInstance.model = newModel;
            levelInstance.SetModel();
            levelInstance.gameObject.SetActive(true);
            PlayerController.Instance.CurrentType = levelInstance.crystalType;
        }

        private void DeactivateAllLevels()
        {
            foreach (var level in createdLevels.Values)
            {
                level.gameObject.SetActive(false);
            }
        }

        //current method for doors
        private void CreateAllDoors()
        {
            foreach (var entry in doorToLocation)
            {
                DoorModel doorModel = new DoorModel();
                doorModel.IndexDoor = entry.Key;

                // Current location
                doorModel.CurrentLocation = entry.Value.Item1;

                // Next location
                doorModel.NextLocation = entry.Value.Item2;

                // Add the door model to the dictionary
                doorModels[entry.Key] = doorModel;
                 // Print the door model values
 Debug.Log($"Door Index: {doorModel.IndexDoor}, Current Location: {doorModel.CurrentLocation.Item1}, {doorModel.CurrentLocation.Item2}, Next Location: {doorModel.NextLocation.Item1}, {doorModel.NextLocation.Item2}");
            }
        }
        
        //current and next doors
        private List<DoorModel> GetDoorsForIndexLocation(int currentLocationIndex)
        {
            var doorModelsForLocation = new List<DoorModel>();
            foreach (var doorModel in doorModels.Values)
            {
                if (doorModel.CurrentLocation.Item1 == currentLocationIndex)
                {
                    doorModelsForLocation.Add(doorModel);
                }
            }
            return doorModelsForLocation;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        private (int,int) GetCountLocationWithDifficilty()
        {
            int difficulty = PlayerPrefs.GetInt("Difficulty");
            return GeneratorModel.GetCountLocationWithDifficilty(difficulty);
        }

        public string GetRoomInfo()
        {
            string roomInfo = "";
            for (int i = 0; i < _locationNetwork.Rooms.Count; i++)
            {
                if (createdLevels.ContainsKey(i))
                {
                    // Если комната была загружена, процент ее прохождения
                    int percentComplete = createdLevels[i].model.GetPercentLevel();
                    roomInfo += $"Room {i}: Progress {percentComplete}%\n";
                }
                else
                {
                    // Иначе сообщение, что комната не активирована
                    roomInfo += $"Room {i}: was inactive\n";
                }
            }
            return roomInfo;
        }
    }

}
