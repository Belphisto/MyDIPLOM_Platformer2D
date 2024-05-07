using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Platformer2D.Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        public KeyCode activationKey;
        public bool _isActive = false;
        //public TypeSlot type;
        public LocationType locationType;
        private Image icon;
        public bool IsActive {get => _isActive;}
        public int Count {get;set;}
        public delegate void ActivateAction(InventorySlot slot);
        public event ActivateAction OnActivate;

        // Start is called before the first frame update
        void Start()
        {
            icon = GetComponent<Image>();
            Count = 0;
        }

        private static Dictionary<LocationType, Color> locationColors = new Dictionary<LocationType, Color>
        {
            { LocationType.Red, new Color(255f / 255f, 89f / 255f, 86f / 255f) },
            { LocationType.Green, new Color(86f / 255f, 255f / 255f, 105f / 255f)},
            { LocationType.Blue, new Color(122f / 255f, 117f / 255f, 255f / 255f)},// Темно-синий цвет
            { LocationType.Sky,  new Color(141f / 255f, 255f / 255f, 238f / 255f)  } , 
            { LocationType.Default,  new Color(255f / 255f, 136f / 255f, 225f / 255f)  } 
        };

        // Update is called once per frame
        void Update()
        {
            // Проверить, нажата ли клавиша активации
            if (Input.GetKeyDown(activationKey))
            {
                // Активировать слот
                Activate();
            }
        }

        public void UpdateSlot(int score)
        {   
            Count = score;
        }

        public void Activate()
        {
            _isActive = true;
            icon.color = locationColors[locationType]; // Установить цвет в зависимости от типа локации
            OnActivate?.Invoke(this);
        }

        public void Deactivate()
        {
            _isActive = false;
            icon.color = Color.white;  // Установить цвет обратно на белый
        }

        public void IncrementSlot()
        {
            Count ++;
        }

        public void DecrementSlot(int count)
        {
            Count = Count-count;
        }

    }

}
