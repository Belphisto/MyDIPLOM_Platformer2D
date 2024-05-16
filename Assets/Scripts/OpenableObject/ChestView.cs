using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer2D.Platform
{
    public class ChestView : MonoBehaviour
    {
        private ChestController controller;
        public LocationType type;
        public void SetModel(ChestModel model)
        {
            controller = new ChestController(model, this);
            Debug.Log("chest create");
        }
        protected void Awake()
        {
            //DoorModel doorModel= new DoorModel(1, 1, 1);
            //SetModel(doorModel);
        }
        private void Update()
        {
            controller.Update();
        }


        public void OnTriggerEnter2D(Collider2D collision)
        {
            controller.OnTriggerEnter2D(collision);

        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            controller.OnTriggerExit2D(collision);

        }

        public void OnTriggerStay2D(Collider2D collision)
        {
            controller.OnTriggerStay2D(collision);

        }


    }
    

}
