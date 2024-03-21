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
            // Подписка на событие обновления счета от игрока
            PlayerController.OnScoreUpdate += HandleScoreUpdate;
        }

        // Метод для создания кристаллов на сцене
        private void SpawnCrystals()
        {
            int scorePerCrystal = model.TotalScore / model.CrystalCount;
            Debug.Log($"LevelController :SpawnCrystals() scorePerCrystal= {scorePerCrystal}");
            foreach (var position in model.PositionsCrystal)
            {
                var crystal = UnityEngine.Object.Instantiate(model.СrystalPrefab, position, Quaternion.identity);
                var crystalModel = new CrystalModel(scorePerCrystal);
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
            foreach (var position in model.PositionsPlatfroms)
            {
                var platform = UnityEngine.Object.Instantiate(model.PlatformPrefab, position, Quaternion.identity);
                // Создание модели платформы с количеством очков, необходимым для изменения цвета платформы
                var platformModel = new PlatformModel(20);
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

    }
}

