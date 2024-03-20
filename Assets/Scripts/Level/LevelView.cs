using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer2D.Crystal;
using Platformer2D.Background;
using Platformer2D.IInterface;

namespace Platformer2D.Level
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private CrystalView crystalPrefab;
        [SerializeField] private BackgroundView backgroundPrefab;

        private LevelController controller;

        // Start is called before the first frame update
        void Start()
        {
            List<Vector3> coordinates = new List<Vector3>
            {
                new Vector3(-1,-1, 0),
                new Vector3(0, -1, 0),
                new Vector3(1, -1, 0)
            };

            LevelModel model = new LevelModel
            (
                crystalPrefab,
                backgroundPrefab,
                coordinates,
                100,
                10
            );
            controller = new LevelController(model, this);
        }


        // Update is called once per frame
        void Update()
        {
            
        }
    }

}
