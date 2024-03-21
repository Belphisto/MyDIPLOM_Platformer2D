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
        // Префабы для кристаллов, платформ и фона - задаются на игровой сцене
        [SerializeField] public CrystalView crystalPrefab;
        [SerializeField] public PlatformView platformPrefab;
        [SerializeField] public BackgroundView backgroundPrefab;

        // Ссылка на контроллер уровня
        private LevelController controller;

        // Вызывается перед первым обновлением кадра
        void Start()
        {
            // Координаты для размещения кристаллов и платформ
            // Будут задаваться из процедурной генерации
            // На данном этапе задаются вручную для тестовой сборки уровння
            List<Vector3> coordinatesCrystal = new List<Vector3>
            {
                new Vector3(-1,-1, 0),
                new Vector3(0, -1, 0),
                new Vector3(1, -1, 0)
            };

            List<Vector3> coordinatesPlatforms = new List<Vector3>
            {
                new Vector3(-7,-2, 0),
                new Vector3(-6, -1, 0),
                new Vector3(-2, -0, 0),
                new Vector3(-2, -1, 0)
            };

            // Создание модели и контроллера уровня
            LevelModel model = new LevelModel
            (
                crystalPrefab,
                platformPrefab,
                backgroundPrefab,
                coordinatesCrystal,
                coordinatesPlatforms,
                // параметр целевого значения счета будет задаваться генератором
                100,
                // параметр количества кристаллов на уровне будет задаваться генератором
                4
            );
            controller = new LevelController(model, this);
        }


        // Update is called once per frame
        void Update()
        {
            
        }
    }

}
