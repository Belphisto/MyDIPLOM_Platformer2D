using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer2D.Platform
{
    public class ChestView : DoorView
    {
        public override void SetModel(DoorModel model)
        {
            controller = new ChestController(model, this);
        }
        protected override void Awake()
        {
            DoorModel doorModel= new DoorModel(1, 1, 1);
            SetModel(doorModel);
        }
        public override void ChangeState()
        {
            
        }

    }
    

}
