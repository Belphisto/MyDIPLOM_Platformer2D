using System.Collections;
using System.Collections.Generic;
using Platformer2D.Level;
using UnityEngine;
using Platformer2D.Generator;
using Platformer2D.Platform;
using System.Linq;

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
        private Dictionary<int, DoorModel> doorModels;
        private List<int> indexCreatedLocation;
        private List<LevelView> levelViews;
        private int indexCurrentLocation;

        // Start is called before the first frame update
        void Start()
        {
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
            doorToLocations = GeneratorGraph.GraphToLocations(_locationNetwork);
            //Debug.Log(doorToLocations[0].ToString());
            // добавить двери 
            levelViews = new List<LevelView>();
            doorModels = new Dictionary<int, DoorModel>();
             indexCreatedLocation = new List<int>();
            CreateDoorModels();

            generatorLocations = new GeneratorLocation();
            
            GeneratorGraph.PrintGraphToLocations(_locationNetwork);
            indexCurrentLocation = 0;
            //TestCreateLocation();
            StartNewLocation(0);
        }

        private void CreateOrLoadLocation(int indexNext)
        {
            /*if (indexCreatedLocation.Contains(indexNext))
            {

            }
            else
            {
                StartNewLocation(indexNext);
            }*/
            levelViews[0].gameObject.SetActive(false);
            StartNewLocation(indexNext);
        }

        private void StartNewLocation(int index)
        {
            indexCreatedLocation.Add(index);
            indexCurrentLocation = index;
            var doorInNewModel = GetDoorModelsForLocation(index);
            LocationType locationType = _locationNetwork.Rooms[index];
            LevelView levelPrefab = levelPrefabs[locationType];
            int edgeCount = _locationNetwork.Transitions[index].Count;

            LevelModel newModel = generatorLocations.GenerateNewLocation(2, index, doorInNewModel);
            LevelView levelInstance = Instantiate(levelPrefab);
            levelViews.Add(levelInstance);
            levelInstance.model = newModel;
            levelInstance.SetModel();

        }

        private void Deactive(int index)
        {

        }

        private void Active(int index)
        {
            
        }

        private void CreateDoorModels()
        {
            Debug.Log(doorToLocations.ToString());
            foreach (var door in doorToLocations)
            {
                //модель двери
                DoorModel doorModel = new DoorModel();
                doorModel.IndexDoor = door.Key;
                doorModel.TypeDoor = _locationNetwork.Rooms[door.Key];
                doorModel.TypeLocation = _locationNetwork.Rooms[door.Value.Item2];
                doorModel.IndexLocation = door.Value.Item1;

                //модель двери в словарь
                doorModels[door.Key] = doorModel;
            }
        }

        private List<DoorModel> GetDoorModelsForLocation(int currentLocationIndex)
        {
            //d=> d.Value.Item1 == currentLocationIndex ||
            var doorsForLocation = doorToLocations.Where(d=> d.Value.Item2 == currentLocationIndex || d.Value.Item1 == currentLocationIndex).Select(d => d.Key);

            var doorModelsForLocation = doorsForLocation.Select(d => doorModels[d]).ToList();
            return doorModelsForLocation;
        }

        private void TestCreateLocation()
        {
            // индекс и тип локации
            int locationIndex = _locationNetwork.ChestLocationIndex;
            LocationType locationType = _locationNetwork.Rooms[locationIndex];
            int edgeCount = _locationNetwork.Transitions[locationIndex].Count;
            //  соответствующий префаб LevelView
            LevelView levelPrefab = levelPrefabs[locationType];
            //  экземпляр префаба LevelView
            LevelModel newModel = generatorLocations.GenerateNewLocation(2, 0);
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
