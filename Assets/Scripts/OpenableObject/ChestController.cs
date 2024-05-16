using System.Collections;
using System.Collections.Generic;
using Platformer2D.Inventory;

using UnityEngine;
namespace Platformer2D.Platform
{
    public class ChestController : DoorController
    {
        public ChestController(DoorModel model, DoorView view) : base(model, view)
        {

        }

        public override void Update()
        {
            if (!model.IsOpen)
            {
                if (isCorrectActiveSlot && Input.GetKeyDown(KeyCode.F))
                {
                    //Debug.Log("PressF");
                    if (activeslot.Count >= model.CountForOpen)
                    {
                        Debug.Log("Door Opened");
                        model.IsOpen = true;
                        InventoryView.Instance.DecrementSlot(view.type, model.CountForOpen);
                        EndGame();
                    }
                    else
                    {
                        Debug.Log("Недостаточно средств");
                    }
                }
            }
            else
            {
                EndGame();
                Debug.Log("Door was open early");
            }
        }

        public void EndGame()
        {
            Debug.Log("GAME WINNNN");
        }
    }

}
