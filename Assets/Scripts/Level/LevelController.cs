using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer2D.Crystal;
using Platformer2D.Background;
using UnityEngine.Animations;
using System;

using Platformer2D.Player;
using Platformer2D.Platform;

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
        public static event Action<int> OnScoreUpdatePlatfroms;
        
        // Конструктор класса
        // Принимает модель и представление
        // Вызывает методы создания объектов на сцене
        public LevelController(LevelModel model, LevelView view)
        {
            this.model = model;
            this.view = view;

            SpawnCrystals();
            SpawnColorChangeBackground();
            SpawnPlatforms();
            SpawnPlayer();
            //Подписка на событие отправки обновления счетчика очков на уровне в платформу
            Bus.Instance.SendScore += HandleScoreUpdate;
        }

        // Метод для создания кристаллов на сцене
        private void SpawnCrystals()
        {
            int scorePerCrystal = model.TotalScore / model.CrystalCount;
            Debug.Log($"LevelController :SpawnCrystals() scorePerCrystal= {scorePerCrystal}");
            foreach (var gameObjectModel in model.Crystal)
            {
                var crystal = (CrystalView) UnityEngine.Object.Instantiate(gameObjectModel.Prefab, gameObjectModel.Position, Quaternion.identity);
                var crystalModel = new CrystalModel(scorePerCrystal, view.crystalType);
                // Созданная с заданными параметрами модель кристалла передается CrystalModel в представление CrystalView
                crystal.SetModel(crystalModel); 
            }
        }

        // Метод для создания изменяющегося фона на сцене
        private void SpawnColorChangeBackground()
        {
            var colorChangeBackground = UnityEngine.Object.Instantiate(view.backgroundPrefab);
            var controller = new BackgroundControlller(new BackgroundModel(), colorChangeBackground);
            controller.SetTargerScore(model.TotalScore);
            colorChangeBackground.SetController(controller);
        }

        // Метод для создания платформ на уровне
        public void SpawnPlatforms()
        {
            SpawnPlatform(model.Platform, 20, 0);
            SpawnPlatform(model.SpecialPlatform, 40, 0.8f);
            SpawnPlatform(model.Bounds, 40, 0f);

        }

        // Метод для создания платформы
        private void SpawnPlatform(List<GameObjectModel> platformModels, int score, float speed)
        {
            foreach (var gameObjectModel in platformModels)
            {
                var platform = (PlatformView) UnityEngine.Object.Instantiate(gameObjectModel.Prefab, gameObjectModel.Position, Quaternion.identity);
                var platformModel = new PlatformModel(score, speed);
                platformModel.StartPosition = gameObjectModel.Position;
                platform.SetModel(platformModel);
            }
        }


        // Метод для обработки обновления счета от игрока
        public void HandleScoreUpdate(int score)
        {
            // Увеличивает счетчик текущего количества очков, собранных на уровне
            model.IncrementScore(score);
            // Вызывает событие для передачи текущего счета на уровне в платформу
            // Обновляет счет платформ, когда счет игрока меняется
            OnScoreUpdatePlatfroms?.Invoke(model.CurrentScore);
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
    }
}

