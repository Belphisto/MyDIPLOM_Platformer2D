using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using  Platformer2D.Level;
using System.Linq;
using Platformer2D.Platform;

namespace Platformer2D.Generator
{
    public class GeneratorLocation
    {
        private int _difficulty;
        public GeneratorLocation()
        {
            _difficulty= PlayerPrefs.GetInt("Difficulty");
        }

        /*public LevelModel GenerateNewLocation(int coefXLoc, int indexLocation)
        {
            Level.Size size = GeneratorModel.GenerateLocationSize();
            Vector2 labelSize = new Vector2(1.0f, 1.3f);
            
            // генерация кристаллов
            var genCrystal = new GeneratorCrystalPosition();
            var countCrystal = GeneratorModel.GetCountCrystal(coefXLoc, _difficulty);
            var crystalPositions = genCrystal.GenerateCrystalPosition(size.X, size.Y, countCrystal,3);

           
            (List<Vector3>staticplatform, List<Vector3>specialplatform, List<Vector3>borderplatform) = RandomGridOrMazePlatforms(labelSize, size);

            var newModel = new LevelModel(
                //GenerateCrystalPosition(size.X, size.Y, countCrystal,3),
                crystalPositions,
                staticplatform,
                specialplatform,
                borderplatform,
                //GenerateBoundaryPlatforms(size),
                //mazeGenerator.GenerateBorderPlatforms(region),
                countCrystal*GeneratorModel.GetScorePerCrystal(coefXLoc, _difficulty),
    
                countCrystal,
                size.Y, size.X,
                indexLocation
            );
            Debug.Log(countCrystal*GeneratorModel.GetScorePerCrystal(coefXLoc, _difficulty));
            return newModel;
        }*/

        public LevelModel GenerateNewLocation(int coefXLoc, int indexLocation, List<Platform.DoorModel> doorModels)
        {
            Size size = GeneratorModel.GenerateLocationSize();
            Vector2 labelSize = new Vector2(1.0f, 1.3f);
            
            // генерация кристаллов
            var genCrystal = new GeneratorCrystalPosition();
            var countCrystal = GeneratorModel.GetCountCrystal(coefXLoc, _difficulty);
            var crystalPositions = genCrystal.GenerateCrystalPosition(size.Width, size.Height, countCrystal,3);

            (List<Vector3>staticplatform, List<Vector3>specialplatform, List<Vector3>borderplatform) = RandomGridOrMazePlatforms(labelSize, size);

            int percent = GeneratorModel.GetTargetCountForOpenPercent(_difficulty);
            int countForOpen = (int)(countCrystal*((double)percent/100));
            Debug.Log("countCrystal" + countCrystal);
            Debug.Log("percent" + percent);
            Debug.Log("countForOpen" + countForOpen);
            var newModel = new LevelModel(

                crystalPositions,
                staticplatform,
                specialplatform,
                borderplatform,

                countCrystal*GeneratorModel.GetScorePerCrystal(coefXLoc, _difficulty),
    
                countCrystal,
                size.Height, size.Width,
                indexLocation,
                GenerateDoorPosition(staticplatform, doorModels, size.Height),
                countForOpen
            );
            Debug.Log($"GeneratorLocation: CountForOpen= {countForOpen}, countCrystal = {countCrystal} ");
            return newModel;
        }

        public LevelModel GenerateNewLocation(int coefXLoc, int indexLocation, List<Platform.DoorModel> doorModels, int countForChest)
        {
            Size size = GeneratorModel.GenerateLocationSize();
            Vector2 labelSize = new Vector2(1.0f, 1.3f);
            
            // генерация кристаллов
            var genCrystal = new GeneratorCrystalPosition();
            var countCrystal = GeneratorModel.GetCountCrystal(coefXLoc, _difficulty);
            var crystalPositions = genCrystal.GenerateCrystalPosition(size.Width, size.Height, countCrystal,3);

            (List<Vector3>staticplatform, List<Vector3>specialplatform, List<Vector3>borderplatform) = RandomGridOrMazePlatforms(labelSize, size);

            int percent = GeneratorModel.GetTargetCountForOpenPercent(_difficulty);
            int countForOpen = (int)(countCrystal*((double)percent/100));
            Debug.Log("countCrystal" + countCrystal);
            Debug.Log("percent" + percent);
            Debug.Log("countForOpen" + countForOpen);
            var chestModel = new ChestModel();
            chestModel.TargetScore = countForChest;

            var newModel = new LevelModel(

                crystalPositions,
                staticplatform,
                specialplatform,
                borderplatform,

                countCrystal*GeneratorModel.GetScorePerCrystal(coefXLoc, _difficulty),
    
                countCrystal,
                size.Height, size.Width,
                indexLocation,
                GenerateDoorPosition(staticplatform, doorModels, size.Height),
                countForOpen,
                (SelectPlatformCoordinateForChest(staticplatform), chestModel)
            );
            Debug.Log($"GeneratorLocation: CountForOpen= {countForOpen}, countCrystal = {countCrystal} ");
            return newModel;
        }

        private (List<Vector3>, List<Vector3>, List<Vector3>) RandomGridOrMazePlatforms(Vector2 labelSize, Size grid)
        {
            // Случайное число от 0 до 9 с равной вероятностью
            int randomNumber = UnityEngine.Random.Range(0, 10);

            // Выбор метода в зависимости от случайного числа
            /*if (randomNumber < 8)
            {
                // Метод для генерации платформ на основе сетки (вероятность 80%)
                return RandomGridPlatforms(labelSize, grid);
            }
            else*/
            {
                // Метод для генерации платформ на основе лабиринта (вероятность 20%)
                return MazePlatforms(labelSize, grid);
            }
        }

        private (List<Vector3>, List<Vector3>, List<Vector3>) RandomGridPlatforms(Vector2 labelSize, Size grid)
        {
            var gen = new GeneratorGridPlatform(labelSize, grid);

            List<Vector3> positionStaticPlatforms = gen.PositionStaticPlatforms();
            List<Vector3> positionSpecialPlatforms = GenerateSpecialPlatformPosition(positionStaticPlatforms);
            List<Vector3> positionBoundaryPlatforms = gen.GenerateBoundaryPlatforms();
            
            return (positionStaticPlatforms,positionSpecialPlatforms, positionBoundaryPlatforms);
        }

        private (List<Vector3>, List<Vector3>, List<Vector3>) MazePlatforms(Vector2 labelSize, Size grid)
        {
            GeneratorMazePlatform mazeGenerator = new GeneratorMazePlatform(labelSize);
            mazeGenerator.GenerateMaze(grid.Width, grid.Height, 0.9f);
            Rect region = new Rect(0, 0, grid.Width * labelSize.x, grid.Height* labelSize.y);
            List<Vector3> platformsStatic = mazeGenerator.GeneratePlatforms(region);
            List<Vector3> platformsSpecial =GenerateSpecialPlatformPosition(platformsStatic);
            List<Vector3> platformsBorder = mazeGenerator.GenerateBorderPlatforms(region);
            return(platformsStatic, platformsSpecial, platformsBorder);
        }
        
        


        private List<Vector3> GenerateStaticPlatformPosition(Size grid)
        {
            var gen = new GeneratorPlatformPosition(new Vector2(3f, 4f), grid);
            //return gen.Position();
            //var gen2 = new GeneratorMazePlatform(new Vector2(2f,3f));
            //return gen2.GeneratePlatforms(new Rect(1,1,grid.X-1, grid.Y-1));

            Vector2 labelSize = new Vector2(1.0f, 1.3f);
            GeneratorMazePlatform mazeGenerator = new GeneratorMazePlatform(labelSize);
            mazeGenerator.GenerateMaze(grid.Width, grid.Height, 0.9f);
            Rect region = new Rect(0, 0, grid.Width * labelSize.x, grid.Height * labelSize.y);
            List<Vector3> platforms = mazeGenerator.GeneratePlatforms(region);
            return platforms;
        }

        private List<Vector3> GenerateSpecialPlatformPosition(List<Vector3> statics)
        {
            /*
            int platformCount = (int)GetPercentCountSpecialPlatform()*statics.Count; // или любое другое выражение, которое вам подходит
            var selectedPlatforms = statics.OrderBy(x => UnityEngine.Random.value).Take(platformCount).ToList();

            // Удаляем из staticplatform все платформы, которые есть в selectedPlatforms
            var remainingPlatforms = statics.Except(selectedPlatforms).ToList();
            return remainingPlatforms;*/
            int platformCount = (int)(GeneratorModel.GetPercentCountSpecialPlatform(_difficulty) * statics.Count); 
            var selectedPlatforms = new List<Vector3>();

            // Выбираем случайные платформы из statics и добавляем их в selectedPlatforms
            for (int i = 0; i < platformCount; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, statics.Count);
                selectedPlatforms.Add(statics[randomIndex]);
                statics.RemoveAt(randomIndex); // Удаляем выбранную платформу из исходного массива
            }
            
            return selectedPlatforms;
        }

        private List<Vector3> SelectPlatformCoordinatesForDoors(List<Vector3> coordinates,  int doorCount)
        {
            //  случайные координаты для дверей
            var doorCoordinates = coordinates.OrderBy(x => UnityEngine.Random.value).Take(doorCount).ToList();
            for (int i = 0; i < doorCoordinates.Count; i++)
            {
                doorCoordinates[i] = new Vector3(doorCoordinates[i].x, doorCoordinates[i].y + 1, doorCoordinates[i].z);
            }

            return doorCoordinates;
        }

        private List<Vector3> SelectPlatformCoordinatesForDoors(List<Vector3> coordinates, int doorCount, int gridSize)
        {
            // Выбираем случайные координаты для дверей
            var doorCoordinates = coordinates.OrderBy(x => UnityEngine.Random.value).Take(doorCount).ToList();
            for (int i = 0; i < doorCoordinates.Count; i++)
            {
                // Если дверь находится в нижней половине сетки, разместить ее над платформой
                if (doorCoordinates[i].y < gridSize / 2)
                {
                    doorCoordinates[i] = new Vector3(doorCoordinates[i].x, doorCoordinates[i].y + 1, doorCoordinates[i].z);
                }
                // Иначе, разместить ее под платформой
                else
                {
                    doorCoordinates[i] = new Vector3(doorCoordinates[i].x, doorCoordinates[i].y - 1, doorCoordinates[i].z);
                }
            }

            return doorCoordinates;
        }

        private Dictionary<Vector3, DoorModel> GenerateDoorPosition(List<Vector3> coordinatesPlatform, List<DoorModel> models, int gridSizeY)
        {
            Dictionary<Vector3,Platform.DoorModel> doorsPosition = new Dictionary<Vector3,Platform.DoorModel>();
            var position = SelectPlatformCoordinatesForDoors(coordinatesPlatform, models.Count);
            
            // назначение doorsPosition переданные модели doorModels
            for (int i = 0; i < models.Count; i++)
            {
                doorsPosition[position[i]] = models[i];
            }
            return doorsPosition;
        }

        private Vector3 SelectPlatformCoordinateForChest(List<Vector3> coordinates)
        {
            //случайная координата для сундука
            var chestCoordinate = coordinates[UnityEngine.Random.Range(0, coordinates.Count)];

            // сундук на одну единицу выше платформы
            chestCoordinate = new Vector3(chestCoordinate.x, chestCoordinate.y + 1, chestCoordinate.z);

            return chestCoordinate;
        }
    }

}
