using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Platformer2D.Crystal;
using Platformer2D.Background;
using Platformer2D.Player;
using Platformer2D.Platform;


/*
Класс LevelView представляет визуальное отображение уровня в игре. 
Он содержит ссылки на префабы для кристаллов, платформ и фона, которые используются для создания этих элементов на уровне.
Необходима доработка - параметры будут задаваться генератором
*/

namespace Platformer2D.Level
{
    // Класс LevelView используется как префаб для сборки визуальной части уровня на игровой сцене
    public class LevelView : MonoBehaviour
    {
        public LevelModel model;
        // Префабы для кристаллов, платформ и фона - задаются на игровой сцене
        [SerializeField] public CrystalView crystalPrefab;
        [SerializeField] public PlatformView platformPrefab;
        [SerializeField] public PlatformView platformPrefabBounds;
        [SerializeField] public PlatformView platformPrefabSpecial;
        [SerializeField] public BackgroundView backgroundPrefab;

        [SerializeField] public DoorView[] doors;  // Все двери
        
        [SerializeField] public ChestView ChestPrefab;  // Все двери

        public LocationType crystalType;

        // Ссылка на контроллер уровня
        private LevelController controller;

        // Вызывается перед первым обновлением кадра
        void Start()
        {
            //CreateModel();
            
        }
        public void SetController()
        {   SetModel();
            //controller = new LevelController(model, this); 
        }

        public void SetModel()
        {

            // Создание GameObjectModel для каждого префаба
            model.Crystal.ForEach(c => c.Prefab = crystalPrefab);
            if (model.Chest.Item2 != null)
            {
                model.Chest.Item1.Prefab = ChestPrefab;
            }
            model.Platform.ForEach(p => p.Prefab = platformPrefab);
            model.SpecialPlatform.ForEach(sp => sp.Prefab = platformPrefabSpecial);
            model.Bounds.ForEach(sp => sp.Prefab = platformPrefabBounds);
            model.Background.Prefab = backgroundPrefab;
            model.Bounds.ForEach(b => b.Prefab = platformPrefabBounds);
            Debug.Log("model is " + (model == null ? "null" : "not null"));
            Debug.Log("model.Doors is " + (model.Doors == null ? "null" : "not null"));

            // Назначение префабов для model.Doors
            foreach (var door in model.Doors)
            {
                // Получение индекса префаба двери на основе типа локации
                int doorPrefabIndex = (int)door.Value.TypeDoor;
                // Назначение соответствующего префаба двери
                door.Key.Prefab = doors[doorPrefabIndex];
            }
            model.CurrentScore = 0;
            controller = new LevelController(model, this); 
        }
        
        private void OnEnable()
        {
            controller?.OnEnable();
        }

        private void OnDisable()    
        {
            controller.OnDisable();
        }

        private void OnDestroy()
        {
            controller.OnDisable();
        }


        // Update is called once per frame
        void Update()
        {
            
        }
    }

}
