using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public KeyCode activationKey;
    public bool _isActive = false;
    public TypeSlot locationType;
    private Image icon;
    public bool IsActive {get => _isActive;}
    public delegate void ActivateAction(InventorySlot slot);
    public event ActivateAction OnActivate;

    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<Image>();
    }

    private static Dictionary<TypeSlot, Color> locationColors = new Dictionary<TypeSlot, Color>
    {
        { TypeSlot.Red, new Color(255f / 255f, 89f / 255f, 86f / 255f) },
        { TypeSlot.Green, new Color(86f / 255f, 255f / 255f, 105f / 255f)},
        { TypeSlot.Blue, new Color(122f / 255f, 117f / 255f, 255f / 255f)},// Темно-синий цвет
        { TypeSlot.Sky,  new Color(141f / 255f, 255f / 255f, 238f / 255f)  }  
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

    private void ChangeColor(float count)
    {
        float newG = Mathf.Min(icon.color.g + count / 255f, 1f);
        float newB = Mathf.Min(icon.color.b + count / 255f, 1f);
        icon.color = new Color(icon.color.r, newG, newB, icon.color.a);
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

}
