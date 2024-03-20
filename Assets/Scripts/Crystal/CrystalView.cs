using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.Crystal
{
    public class CrystalView : MonoBehaviour
    {
        private CrystalController controller;

        // Start is called before the first frame update
        void Start()
        {
            controller = new CrystalController(new CrystalModel(), this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            controller.HandleTriggerEnter(other);
        }

        public void DestroyPoint()
        {
            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }

}
