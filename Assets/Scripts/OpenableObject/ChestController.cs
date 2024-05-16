using System.Collections;
using System.Collections.Generic;
using Platformer2D.Inventory;

using UnityEngine;
namespace Platformer2D.Platform
{
    public class ChestController 
    {
        ChestModel model;
        ChestView view;
        protected bool isCorrectActiveSlot = false;
        protected bool isPlayerInside = false;
        protected InventorySlot activeslot;
        public ChestController(ChestModel model, ChestView view)
        {
            this.model = model;
            this.view = view;

        }
        
        internal void Update()
        {
            if (!model.IsOpen)
            {
                if (isCorrectActiveSlot)
                {
                    CameraManager.Instance.UpadteText($"Press F for chest");
                    CameraManager.Instance.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (activeslot.Count >= model.TargetScore)
                        {
                            model.IsOpen = true;
                            InventoryView.Instance.DecrementSlot(view.type, model.TargetScore);
                        }
                    }
                    
                    else
                    {
                        CameraManager.Instance.UpadteText($"Not count");
                        CameraManager.Instance.SetActive(true);
                    }
                }
            }
            else
            {
                if (isPlayerInside && Input.GetKeyDown(KeyCode.Return))
                {
                    CameraManager.Instance.SetActive(false);
                    Debug.Log($"GameWin");
                    EndGame();
                }
            }
        }

        public void EndGame()
        {
            Bus.Instance.SendGameWin(Player.PlayerController.Instance.SendTotalScore());
        }

        public  void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                activeslot = InventoryView.Instance.GetActiveSlot();
                CameraManager.Instance.UpadteText($"Need: {model.TargetScore}");
                CameraManager.Instance.SetActive(true);
                if (activeslot != null && (activeslot.locationType == LocationType.Default))
                {
                    //Debug.Log("Active correctSlot");
                    
                    isCorrectActiveSlot = true;
                }
            }
        }

        public void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("OnTriggerStay2D(Player)");
            if (model.IsOpen)
            {
                CameraManager.Instance.UpadteText($"Press Enter");
                CameraManager.Instance.SetActive(true);

                isPlayerInside = true;
            }
            }
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("OnTriggerStay2D(Player)");
            if (model.IsOpen)
            {
                CameraManager.Instance.SetActive(false);

                isPlayerInside = false;
            }
            }
        }

    }

}
