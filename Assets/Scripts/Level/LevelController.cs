using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer2D.Crystal;
using Platformer2D.Background;
using UnityEngine.Animations;
using System;

using Platformer2D.Player;
using Platformer2D.Platform;
using System.Threading;

/*
Класс LevelController отвечает за управление уровнем игры. 
Он содержит модель данных и представление уровня, а также методы для создания:
    - кристаллов,
    - фона
    - и платформ на уровне. 
Класс также подписывается на событие обновления счета игрока и обрабатывает это событие, увеличивая счет уровня и вызывая событие обновления счета платформ.
Этот класс является ключевым элементом единицы игрового уровня, связывающим вместе все игровые объекты.
*/

namespace Platformer2D.Level
{
    // Класс LevelController управляет уровнем игры
    public class LevelController 
    {
        // Модель данных уровня
        private LevelModel model;
        // Представление уровня в игровой сцене
        private LevelView view;
        //Событие для вызова обновления счетчика очков платформы
        //public static event Action<int> OnScoreUpdatePlatfroms;
        
        // Конструктор класса
        // Принимает модель и представление
        // Вызывает методы создания объектов на сцене
        public LevelController(LevelModel model, LevelView view)
        {
            this.model = model;
            this.view = view;
            SpawnCrystals(); //
            SpawnPlatforms();
            SpawnPlayer();
            SpawnColorChangeBackground(); //
            SpawnDoor();
            SpawnChest();
            //Подписка на событие отправки обновления счетчика очков на уровне в платформу
            Bus.Instance.SendScore += HandleScoreUpdate;
            model.CurrentScore = 0;
        }

        public void OnEnable()
        {
            Bus.Instance.SendScore += HandleScoreUpdate;
        }

        public void OnDisable()
        {
            Bus.Instance.SendScore -= HandleScoreUpdate;
        }

        // Метод для создания кристаллов на сцене
        private void SpawnCrystals()
        {
            int scorePerCrystal = model.TotalScore / model.CrystalCount;
            //Debug.Log($"LevelController :SpawnCrystals() scorePerCrystal= {scorePerCrystal}");
            foreach (var gameObjectModel in model.Crystal)
            {
                var crystal = (CrystalView) UnityEngine.Object.Instantiate(gameObjectModel.Prefab, gameObjectModel.Position, Quaternion.identity);
                crystal.transform.SetParent(view.transform);
                var crystalModel = new CrystalModel(scorePerCrystal);
                //Debug.LogWarning($"{view.crystalType}");
                // Созданная с заданными параметрами модель кристалла передается CrystalModel в представление CrystalView
                crystal.SetModel(crystalModel); 
            }
        }

        // Метод для создания изменяющегося фона на сцене
        private void SpawnColorChangeBackground()
        {
            var colorChangeBackground = UnityEngine.Object.Instantiate(view.backgroundPrefab);
            colorChangeBackground.transform.SetParent(view.transform);
            var controller = new BackgroundControlller(new BackgroundModel(), colorChangeBackground);
            controller.SetTargerScore(model.TotalScore);
            colorChangeBackground.SetController(controller);
        }

        // Метод для создания платформ на уровне
        public void SpawnPlatforms()
        {
            SpawnPlatform(model.Platform);
            SpawnPlatform(model.SpecialPlatform);
            SpawnPlatform(model.Bounds);

        }



        // Метод для создания платформы
        private void SpawnPlatform(List<GameObjectModel> platformModels)
        {
            int totalScore = model.TotalScore;
            int totalPlatforms = platformModels.Count;
            int averageScorePerPlatform = (totalScore/platformModels.Count);
            if (averageScorePerPlatform == 0) averageScorePerPlatform = 1;
            Debug.Log(averageScorePerPlatform);
            int score = 0;
            for (int i = 0; i < platformModels.Count; i++)
            {
                // значение score для текущей платформы
                score+=averageScorePerPlatform;

                var platform = (PlatformView)UnityEngine.Object.Instantiate(platformModels[i].Prefab, platformModels[i].Position, Quaternion.identity);
                platform.transform.SetParent(view.transform);
                //  модель платформы и начальное значение score
                var platformModel = new PlatformModel(score);
                //Debug.Log($"TargetScoreForPlatform = {score}");
                platformModel.StartPosition = platformModels[i].Position;

                // модель для платформы
                platform.SetModel(platformModel);
            }
        }


        // Метод для обработки обновления счета от игрока
        public void HandleScoreUpdate(int score)
        {
            Debug.Log("model.CurrentScore LevelController: " + model.CurrentScore);
            // Увеличивает счетчик текущего количества очков, собранных на уровне
            model.IncrementScore(score);
            Bus.Instance.SendBackground(model.CurrentScore);
            //Bus.Instance.SendAllScore(model.CurrentScore);
            // Вызывает событие для передачи текущего счета на уровне в платформу
            // Обновляет счет платформ, когда счет игрока меняется
            //OnScoreUpdatePlatfroms?.Invoke(model.CurrentScore); 
            Bus.Instance.SendForPlatform(model.CurrentScore);
            Bus.Instance.SendLevelPercent(model.GetPercentLevel());
        }

        private void SpawnPlayer()
        {
            // Найти персонажа по тегу
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            // Проверить, найден ли персонаж
            if (player != null)
            {
                // Изменить координаты персонажа
                player.transform.position = new Vector3(2, 2, 0);
            }
            else
            {
                Debug.Log("Персонаж с тегом 'Player' не найден");
            }
        }


        private void SpawnDoor()
        {
            foreach (var positionDoor in model.Doors)
            {
                //DoorView doorPrefab = new DoorView();
                //DoorView doorPrefab = Array.Find(view.doors, door => door.type == positionDoor.Value.TypeDoor);
                /*if (positionDoor.Value.IndexDoor == positionDoor.Value.IndexesLocation.Item1)
                {
                    doorPrefab = Array.Find(view.doors, door => door.type == positionDoor.Value.TypesDoors.Item1);
                }
                else
                {
                    doorPrefab = Array.Find(view.doors, door => door.type == positionDoor.Value.TypesDoors.Item2);
                }*/
                DoorView doorPrefab = Array.Find(view.doors, door => door.type == positionDoor.Value.NextLocation.Item2);
                Debug.Log("model.Doors is " + (doorPrefab == null ? "null" : "not null"));

                if (doorPrefab != null)
                {
                    var door = (DoorView) UnityEngine.Object.Instantiate(doorPrefab, positionDoor.Key.Position, Quaternion.identity);
                    door.transform.SetParent(view.transform);
                    var doorModel = positionDoor.Value;
                    
                    doorModel.CountForOpen = model.TargetCountForDoors;
                    doorModel.TargetScore = model.TotalScore/2;
                    Debug.Log($"door.SetModel: CountForOpen= {doorModel.CountForOpen}, TargetScore= {doorModel.TargetScore }");
                    door.SetModel(doorModel);
                }
            }
        }
        private void SpawnChest()
        {
            if (model.Chest.Item2 != null && model.Chest.Item1 != null)
            {
                // Создание экземпляра сундука
                var chest = (ChestView)UnityEngine.Object.Instantiate(model.Chest.Item1.Prefab, model.Chest.Item1.Position, Quaternion.identity);
                chest.transform.SetParent(view.transform);
                var chestModel = model.Chest.Item2;
                Debug.Log($"door.SetModel: CountForOpenChest= {chestModel.CountForOpen}, TargetScore= {chestModel.TargetScore }");
                chest.SetModel(chestModel);
            }
        }

        private void HandleUpdateLevelPercent()
        {

        }
    }
}

