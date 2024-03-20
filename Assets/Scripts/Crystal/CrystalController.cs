using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.Crystal
{
    public class CrystalController
    {
        private CrystalModel model;
        private CrystalView view;

        public CrystalController(CrystalModel model, CrystalView view)
        {
            this.model = model;
            this.view = view;
        }

        public void HandleTriggerEnter(Collider2D other)
            {
                if (other.gameObject == Hero.Instance.gameObject) // Проверяем, что персонаж вошел в триггер
                {
                    Hero.Instance.GetScore(model.Score); 
                    view.DestroyPoint(); 
                }
            }
    }

}
