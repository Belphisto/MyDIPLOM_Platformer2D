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
            //SpawnLevelBounds(model.Width, model.Height);
            SpawnCrystals();
            SpawnColorChangeBackground();
            SpawnPlatforms();
            //Подписка на событие отправки обновления счетчика очков на уровне в платформу
            Bus.Instance.SendScore += HandleScoreUpdate;
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
            //Размещение статичных платформ
            foreach (var position in model.PositionsPlatfroms)
            {
                var platform = UnityEngine.Object.Instantiate(model.PlatformPrefab, position, Quaternion.identity);
                // Создание модели платформы с количеством очков, необходимым для изменения цвета платформы
                var platformModel = new PlatformModel(20,0);
                platformModel.StartPosition = position;
                platform.SetModel(platformModel);
            }

            //Размещение платформ препятствия 1
            foreach (var position in model.PositionsPlatfromsSpecial1)
            {
                var platform = UnityEngine.Object.Instantiate(model.PlatformPrefabSpecial1, position, Quaternion.identity);
                // Создание модели платформы с количеством очков, необходимым для изменения цвета платформы
                var platformModel = new PlatformModel(40, 0.8f);
                platformModel.StartPosition = position;
                platform.SetModel(platformModel);
            }
            
            //Размещение платформ препятствия 2
            foreach (var position in model.PositionsPlatfromsSpecial2)
            {
                var platform = UnityEngine.Object.Instantiate(model.PlatformPrefabSpecial2, position, Quaternion.identity);
                // Создание модели платформы с количеством очков, необходимым для изменения цвета платформы
                var platformModel = new PlatformModel(60, 0f);
                platformModel.StartPosition = position;
                platform.SetModel(platformModel);
            }
        }

        // Метод для создания границ уровня
        /*public void SpawnLevelBounds(float width, float height)
        {
            // Размеры платформы
            float platformWidth = model.PlatformPrefabBounds.GetComponent<Collider2D>().bounds.size.x;
            float platformHeight = model.PlatformPrefabBounds.GetComponent<Collider2D>().bounds.size.y;

            // Количество платформ по горизонтали и вертикали
            int horizontalPlatformCount = Mathf.RoundToInt(width / platformWidth);
            int verticalPlatformCount = Mathf.RoundToInt(height / platformWidth);

            // Создание горизонтальных границ
            for (int i = 0; i < horizontalPlatformCount; i++)
            {
                // Верхняя граница
                Vector3 topPosition = new Vector3(i * platformWidth, height, 0);
                SpawnPlatform(topPosition);

                // Нижняя граница
                Vector3 bottomPosition = new Vector3(i * platformWidth, 0, 0);
                SpawnPlatform(bottomPosition);
            }

            // Создание вертикальных границ
            for (int i = 1; i < verticalPlatformCount - 1; i++)
            {
                // Левая граница
                Vector3 leftPosition = new Vector3(0, i * platformHeight, 0);
                SpawnPlatform(leftPosition, true);

                // Правая граница
                Vector3 rightPosition = new Vector3((horizontalPlatformCount - 1) * platformWidth, i * platformWidth, 0);
                SpawnPlatform(rightPosition, true);
            }

        }

        // Метод для создания платформы на заданной позиции
        private void SpawnPlatform(Vector3 position, bool rotate = false)
        {
            var platform = UnityEngine.Object.Instantiate(model.PlatformPrefab, position, Quaternion.identity);
            var platformModel = new PlatformModel(20,0);
            platformModel.StartPosition = position;
            platform.SetModel(platformModel);

           // Если нужно повернуть платформу
            if (rotate)
            {
                // Поворачиваем платформу на 90 градусов
                platform.transform.Rotate(0, 0, 90);

                // Изменяем размеры платформы
                var collider = platform.GetComponent<BoxCollider2D>(); // Изменено на BoxCollider2D
                var size = collider.size;
                collider.size = new Vector2(size.y, size.x);
            }
        }*/

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

